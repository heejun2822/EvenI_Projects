using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text scoreText;

    private void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("score").ToString();
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }
}
