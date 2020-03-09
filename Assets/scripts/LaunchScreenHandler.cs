using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchScreenHandler : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button exitButton;
    string gameScene = "MainMap";

    void Start()
    {
        Time.timeScale = 0f;
        playButton.onClick.AddListener(startGame);
        exitButton.onClick.AddListener(exit);
    }
    private void startGame()
    {
        defaultTimeScale();
        SceneManager.LoadScene(gameScene);
    }
    private void exit()
    {
        Application.Quit();
    }
    private void defaultTimeScale()
    {
        Time.timeScale = 1;
    }
}
