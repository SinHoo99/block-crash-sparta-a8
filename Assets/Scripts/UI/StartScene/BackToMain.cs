using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    public void BackToMainScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void GoToPlayScene(bool isMulti)
    {
        GameManager.Instance.IsMulti = isMulti;
        SceneManager.LoadScene("MainScene");
    }    
}
