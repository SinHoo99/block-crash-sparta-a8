using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BallController : MonoBehaviour
{
    private float defaultSpeed = 5f;
    private Rigidbody2D rigidbody;
    private TrailRenderer trailRenderer;    


    // �� Copy���� ����� ����
    private float minRotationAngle = 30f;
    private float maxRotationAngle = 50f;
    
    private void OnEnable()
    {
        GameManager.Instance.OnCopyBallEvent += Copy;
        GameManager.Instance.OnFinishStageEvent += Destroyed;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnCopyBallEvent -= Copy;
        GameManager.Instance.OnFinishStageEvent -= Destroyed;
    }

    private void Awake()
    {  
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void Copy()
    {
        float rotationAngle = Random.Range(minRotationAngle, maxRotationAngle);
        RotateAngle(rotationAngle);
        RotateAngle(-rotationAngle);
    }

    public void RotateAngle(float rotationAngle)
    {
        GameObject ball = GameManager.Instance.CreateBalls();
        if (ball == null)
        {
            return;
        }
        // ���� ���� �ӵ�
        Vector2 currentVelocity = rigidbody.velocity;

        Vector2 direction = Quaternion.Euler(0, 0, rotationAngle) * currentVelocity.normalized;

        // ������ ���� ��ġ ����
        ball.transform.position = transform.position;

        // ������ ���� Rigidbody2D Component ��������
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        // ������ ���� ����� �ӵ� ����
        rb.velocity = direction * currentVelocity.magnitude;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionLayerName = LayerMask.LayerToName(collision.gameObject.layer);
        
        switch (collisionLayerName)
        {
            case "Player":
                ProcessPaddleCollision(collision);
                break;
            case "Block":
                ProcessBlockCollision(collision);
                ObjectCollision(collision);
                break;
            case "Bottom":
                //Destroyed();
                break;
            case "Wall":
                ObjectCollision(collision);
                break;
            default:
                break;
        }
    }

    //�е鿡 �������
    private void ProcessPaddleCollision(Collision2D collision)
    {
        Vector2 collisionPoint = collision.contacts[0].point; // �е鿡 �����ġ�� �浹�� �Ͼ���� Ȯ�� �ϱ����� Vector
        Vector2 paddleCenter = collision.transform.position; // �е� �߽� ������������ Vector

        // �浹 ������ �е� �߽ɺ��� ���ʿ� �ִ��� Ȯ��
        bool isLeftCollision = collisionPoint.x < paddleCenter.x;

        // X�� ������ �ӵ��� ���� �Ǵ� ���������� ����
        float direction = isLeftCollision ? -1f : 1f;
        rigidbody.velocity = new Vector2(direction * Mathf.Abs(rigidbody.velocity.x), rigidbody.velocity.y);
    }
    //��Ͽ� �������
    private void ProcessBlockCollision(Collision2D collision)
    {
        BlockHandler blockHandler = collision.gameObject.GetComponent<BlockHandler>();
        if (blockHandler != null)
        {
            blockHandler.TakeDamage(1); // ����� HP�� ����.
        }
        else
        {
            Debug.Log("BlockHandler�� null�Դϴ�.");
        }
    }
    //�е��� ������ ������Ʈ�� �������
    private void ObjectCollision(Collision2D collision)
    {
        Vector2 newDirection = rigidbody.velocity.normalized;

        Vector2 localXAxis = new Vector2(1f, 0f); // ������Ʈ�� ���� x��
        Vector2 localYAxis = new Vector2(0f, 1f); // ������Ʈ�� ���� y��

        float angleWithXAxis = Vector2.Angle(newDirection, localXAxis);
        float angleWithYAxis = Vector2.Angle(newDirection, localYAxis);

        if (Mathf.Approximately(angleWithXAxis, 180f) || Mathf.Approximately(angleWithYAxis, 180f))
        { 
            float angleChangeRadians = Mathf.Deg2Rad * Random.Range(-30f, 30f);
            newDirection = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angleChangeRadians) * newDirection;
        }


        rigidbody.velocity = newDirection * defaultSpeed;
    }
    // �ٴڿ� �������
    public void Destroyed()
    {        
        if (!gameObject.activeSelf) return;
        GameManager.Instance.ObjectPool.ReturnObject(this.gameObject);
        GameManager.Instance.DestroyBalls();
    }
    private bool IsLayerMatched(int value, int layer)
    {
        return value == (value | 1 << layer);
    }

}
