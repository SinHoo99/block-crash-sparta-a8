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
    public void GoToPlayScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
