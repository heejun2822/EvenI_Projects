using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Owns the title scene's start action.</summary>
public sealed class TitleSceneDirector : BaseSceneDirector
{
    [SerializeField] private Button startButton;

    protected override bool InitializeScene()
    {
        if (startButton == null)
        {
            Debug.LogError("TitleSceneDirector is missing the start button reference.", this);
            return false;
        }

        startButton.onClick.AddListener(StartGame);
        return true;
    }

    protected override void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(StartGame);
        }

        base.OnDestroy();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
