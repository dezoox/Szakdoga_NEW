using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenHandler : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button exitButton;
    private string gameScene = "MainMap";

    void Start()
    {
        exitButton.onClick.AddListener(exitGame);
        restartButton.onClick.AddListener(restartGame);
    }
    private void exitGame()
    {
        defaultTimeScale();
        Application.Quit();
    }
    private void restartGame()
    {
        SceneManager.LoadScene(gameScene);
        defaultTimeScale();
    }
    /// <summary>
    /// Resets the time scale back to 1.
    /// </summary>
    private void defaultTimeScale()
    {
        Time.timeScale = 1;
    }
}
