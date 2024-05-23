using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BallController : MonoBehaviour
{
    private float defaultSpeed = 5f;
    private Rigidbody2D rigidbody;
    private TrailRenderer trailRenderer;    


    // 공 Copy에서 사용할 각도
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
        // 현재 공의 속도
        Vector2 currentVelocity = rigidbody.velocity;

        Vector2 direction = Quaternion.Euler(0, 0, rotationAngle) * currentVelocity.normalized;

        // 복사할 공의 위치 설정
        ball.transform.position = transform.position;

        // 복사할 공의 Rigidbody2D Component 가져오기
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        // 복사할 공의 방향과 속도 설정
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

    //패들에 닿았을때
    private void ProcessPaddleCollision(Collision2D collision)
    {
        Vector2 collisionPoint = collision.contacts[0].point; // 패들에 어느위치에 충돌이 일어났는지 확인 하기위한 Vector
        Vector2 paddleCenter = collision.transform.position; // 패들 중심 가져오기위한 Vector

        // 충돌 지점이 패들 중심보다 왼쪽에 있는지 확인
        bool isLeftCollision = collisionPoint.x < paddleCenter.x;

        // X축 방향의 속도를 왼쪽 또는 오른쪽으로 반전
        float direction = isLeftCollision ? -1f : 1f;
        rigidbody.velocity = new Vector2(direction * Mathf.Abs(rigidbody.velocity.x), rigidbody.velocity.y);
    }
    //블록에 닿았을때
    private void ProcessBlockCollision(Collision2D collision)
    {
        BlockHandler blockHandler = collision.gameObject.GetComponent<BlockHandler>();
        if (blockHandler != null)
        {
            blockHandler.TakeDamage(1); // 블록의 HP를 감소.
        }
        else
        {
            Debug.Log("BlockHandler가 null입니다.");
        }
    }
    //패들을 제외한 오브젝트에 닿았을때
    private void ObjectCollision(Collision2D collision)
    {
        Vector2 newDirection = rigidbody.velocity.normalized;

        Vector2 localXAxis = new Vector2(1f, 0f); // 오브젝트의 로컬 x축
        Vector2 localYAxis = new Vector2(0f, 1f); // 오브젝트의 로컬 y축

        float angleWithXAxis = Vector2.Angle(newDirection, localXAxis);
        float angleWithYAxis = Vector2.Angle(newDirection, localYAxis);

        if (Mathf.Approximately(angleWithXAxis, 180f) || Mathf.Approximately(angleWithYAxis, 180f))
        { 
            float angleChangeRadians = Mathf.Deg2Rad * Random.Range(-30f, 30f);
            newDirection = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angleChangeRadians) * newDirection;
        }


        rigidbody.velocity = newDirection * defaultSpeed;
    }
    // 바닥에 닿았을때
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
