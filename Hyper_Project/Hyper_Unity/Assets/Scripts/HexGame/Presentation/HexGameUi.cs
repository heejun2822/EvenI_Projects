using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Updates individual runtime UI elements and owns button bindings.</summary>
public sealed class HexGameUi
{
    private const float WarningSeconds = 3f;
    private const float TimerShakeFrequency = 30f;
    private const float TimerShakeDistance = 8f;

    private readonly TextMeshProUGUI scoreText;
    private readonly TextMeshProUGUI healthText;
    private readonly TextMeshProUGUI goalText;
    private readonly TextMeshProUGUI timerText;
    private readonly Vector2 timerBasePosition;
    private readonly Button[] formulaButtons;
    private readonly TextMeshProUGUI[] formulaTexts;
    private readonly GameObject gameOverPanel;
    private readonly GameObject clearPanel;
    private readonly Button[] homeButtons;

    public HexGameUi(TextMeshProUGUI scoreText, TextMeshProUGUI healthText, TextMeshProUGUI goalText, TextMeshProUGUI timerText,
        Button[] formulaButtons, GameObject gameOverPanel, GameObject clearPanel, Button[] homeButtons)
    {
        this.scoreText = scoreText;
        this.healthText = healthText;
        this.goalText = goalText;
        this.timerText = timerText;
        this.formulaButtons = formulaButtons;
        this.gameOverPanel = gameOverPanel;
        this.clearPanel = clearPanel;
        this.homeButtons = homeButtons;
        timerBasePosition = timerText.rectTransform.anchoredPosition;

        formulaTexts = new TextMeshProUGUI[formulaButtons.Length];
        for (int index = 0; index < formulaButtons.Length; index++)
        {
            formulaTexts[index] = formulaButtons[index].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void Bind(Action<int> useFormula, Action goHome)
    {
        for (int index = 0; index < formulaButtons.Length; index++)
        {
            int buttonIndex = index;
            formulaButtons[index].onClick.AddListener(() => useFormula(buttonIndex));
        }

        foreach (Button homeButton in homeButtons)
        {
            homeButton.onClick.AddListener(goHome.Invoke);
        }
    }

    public void RefreshStage(GameState state, StageData stage)
    {
        RefreshScore(state);
        RefreshHealth(state);
        RefreshGoal(stage);
        RefreshFormulas(state);
    }

    public void RefreshScore(GameState state)
    {
        scoreText.text = $"Score: {state.Score}";
    }

    public void RefreshHealth(GameState state)
    {
        healthText.text = $"Health: {Mathf.CeilToInt(state.Health)}";
    }

    public void RefreshGoal(StageData stage)
    {
        goalText.text = $"Goal: {stage.GoalScore}";
    }

    public void RefreshFormulas(GameState state)
    {
        for (int index = 0; index < formulaButtons.Length; index++)
        {
            bool hasFormula = index < state.StoredFormulas.Count;
            formulaButtons[index].gameObject.SetActive(hasFormula);
            if (hasFormula)
            {
                formulaTexts[index].text = state.StoredFormulas[index].ToString();
            }
        }
    }

    public void RefreshTimer(float remaining)
    {
        bool isWarning = remaining <= WarningSeconds;
        timerText.gameObject.SetActive(true);
        timerText.text = Mathf.CeilToInt(Mathf.Max(0f, remaining)).ToString();
        timerText.color = isWarning ? new Color(1f, .2f, .2f) : Color.black;
        timerText.rectTransform.anchoredPosition = timerBasePosition + (isWarning
            ? new Vector2(Mathf.Sin(Time.unscaledTime * TimerShakeFrequency) * TimerShakeDistance, 0f)
            : Vector2.zero);
    }

    public void StopTimer()
    {
        timerText.gameObject.SetActive(false);
        timerText.rectTransform.anchoredPosition = timerBasePosition;
    }

    public void HideResultPanels()
    {
        gameOverPanel.SetActive(false);
        clearPanel.SetActive(false);
    }

    public void ShowGameOver() => gameOverPanel.SetActive(true);
    public void ShowClear() => clearPanel.SetActive(true);
}
