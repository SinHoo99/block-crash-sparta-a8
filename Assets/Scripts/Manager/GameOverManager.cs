using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject gameObject3;
    public GameObject gameObject4;

    public GameObject GameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    public void SetOff()
    {
        gameObject1.SetActive(false);
        gameObject2.SetActive(false);
        gameObject3.SetActive(false);
        gameObject4.SetActive(false);        
    }

    public void MakeGameOverUI()
    {
        SetOff();
        Instantiate(GameOverUI).SetActive(true);
        
    }

    public void ConfirmPlayerName()
    {

    }
}
