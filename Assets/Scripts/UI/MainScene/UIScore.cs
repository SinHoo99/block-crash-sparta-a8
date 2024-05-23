using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   

public class UIScore : MonoBehaviour
{
    private Text highScoreText;
    private Text currentScoreText;

    private void Awake()
    {
        highScoreText = GetComponentsInChildren<Text>()[0];
        currentScoreText = GetComponentsInChildren<Text>()[1];        
    }

    private void Start()
    {
        MainSceneManager.Instance.OnChangeScoreEvent += ChangeScoreUI;        
    }

    private void OnDestroy()
    {
        MainSceneManager.Instance.OnChangeScoreEvent -= ChangeScoreUI; 
    }

    private void ChangeScoreUI()
    {
        highScoreText.text = MainSceneManager.Instance.HighScore.ToString();
        currentScoreText.text = MainSceneManager.Instance.CurrentScore.ToString();        
    }
}
