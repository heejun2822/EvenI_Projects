using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : UIPanel
{
    GameManager gameManager;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        retryButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);

        gameManager.Pause(true);
    }

    private void OnDisable()
    {
        retryButton.onClick.RemoveListener(RestartGame);
        exitButton.onClick.RemoveListener(ExitGame);
    }

    public void RestartGame()
    {
        Active = false;
        gameManager.ResetGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
