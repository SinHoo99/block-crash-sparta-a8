using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    private TMP_InputField input;
    private Text scoreTxt;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        input = transform.Find("InputField").GetComponent<TMP_InputField>();
        scoreTxt = transform.Find("FinalScoreTxt").GetComponent<Text>();
        scoreTxt.text = MainSceneManager.Instance.CurrentScore.ToString();
    }

    public void ReportScore()
    {
        int finalScore = MainSceneManager.Instance.CurrentScore;
        scoreTxt.text = finalScore.ToString();
        RankBoardManager.Instance.AddNewScore(input.text, finalScore);
        SceneManager.LoadScene("StartScene");
    }
}
