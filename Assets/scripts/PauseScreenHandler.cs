using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreenHandler : MonoBehaviour
{
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button exitButton;

    void Start()
    {
        resumeButton.onClick.AddListener(resumeGame);
        exitButton.onClick.AddListener(exit);
    }

    private void resumeGame()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
    private void exit()
    {
        Application.Quit();
    }
    
}
