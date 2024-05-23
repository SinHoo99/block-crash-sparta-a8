using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject[] players = new GameObject[2];
    private StageController stageController;

    public event Action OnCopyBallEvent;
    public event Action OnFinishStageEvent;
    public event Action<bool> OnChangeLifeEvent;


    private int highScore;
    private int currentScore;
    private int blockCount;
    private int ballCount;
    private int stageLevel = 1;
    private bool isMulti = false;
    private int life;

    public ObjectPool ObjectPool { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        ObjectPool = GetComponent<ObjectPool>();
        players[0] = GameObject.Find("Player").transform.GetChild(0).gameObject;
        players[1] = GameObject.Find("Player").transform.GetChild(1).gameObject;
        players[1].SetActive(isMulti);
        stageController = GameObject.Find("Stage").gameObject.GetComponent<StageController>();
        StartStage(stageLevel);
        life = 0;
        for (int i = 0; i < 2; i++) AddLife();
    }

    private void StartStage(int stageLevel)
    {
        CallFinishStageEvent();
        ballCount = 0;
        currentScore = 0;
        if (stageLevel < 5) blockCount = stageController.StartStage(stageLevel);
        else stageController.BossStage();
        ResetPlayerPos();
        ResetBallPos();
    }

    public void CallFinishStageEvent()
    {
        OnFinishStageEvent?.Invoke();
    }

    private void ResetPlayerPos()
    {
        Vector3 startPosX = isMulti ? Vector3.left * 2 : Vector3.zero;
        Vector3 startPos = Vector3.down * 4.2f;
        players[0].transform.position = startPos + startPosX;
        if (isMulti) players[1].transform.position = startPos + startPosX;
    }

    private void ResetBallPos()
    {
        GameObject obj = CreateBalls();

        obj.GetComponent<Rigidbody2D>().velocity = (Vector3.down + Vector3.right) * 5f;
        obj.transform.position = new Vector3(0, -4f, 0);
    }

    // 블록 파괴 시 
    public void DestroyBlock(int score)
    {
        currentScore += score;
        if (currentScore > highScore) highScore = currentScore;

        if (--blockCount == 0)
            StartStage(++stageLevel);
    }

    public void CallChangeLifeEvent(bool isAdd)
    {
        OnChangeLifeEvent?.Invoke(isAdd);
    }

    // 목숨 추가 아이템
    public void AddLife()
    {
        life++;
        CallChangeLifeEvent(true);
    }

    private void ReduceLife()
    {
        if (life-- > 0)
        {
            ResetPlayerPos();
            CallChangeLifeEvent(false);
            return;
        }
        GameOver();
    }

    // 볼 복사 아이템
    public GameObject CreateBalls()
    {
        GameObject obj = ObjectPool.SpawnFromPool("Ball");
        if (obj == null) return null;
        BallController ball = obj.GetComponent<BallController>();
        ballCount++;

        return obj;
    }

    private void CallCopyBallEvent()
    {
        OnCopyBallEvent?.Invoke();
    }
    public void Copyballs()
    {
        CallCopyBallEvent();
    }

    public void DestroyBalls()
    {
        if (--ballCount == 0)
            ReduceLife();
    }

    private void GameOver()
    {

    }
}
