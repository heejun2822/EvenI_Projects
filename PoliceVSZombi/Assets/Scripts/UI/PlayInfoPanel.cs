using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayInfoPanel : UIPanel
{
    private Player player;
    private HealthSystem playerHealthSystem;
    private PlayerStatHandler playerStatHandler;

    UIPanelManager uiPanelManager;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private Slider expGaugeSlider;


    public void OnClikedPauseButton()
    {
        int value = Time.timeScale == 0 ? 1 : 0;

        PausePanel pausePanel = uiPanelManager.GetPanel<PausePanel>(UIType.PausePanel);
        if(pausePanel == null )
        {
            uiPanelManager.OpenPanel(UIType.PausePanel);
            pausePanel = uiPanelManager.GetPanel<PausePanel>(UIType.PausePanel);
        }

        pausePanel.Active = (value == 0);
    }

    public void InitUI(Player player, UIPanelManager uIPanelManager)
    {
        this.player = player;
        playerHealthSystem = player.GetComponent<HealthSystem>();
        playerStatHandler = player.GetComponent<PlayerStatHandler>();

        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;

        this.uiPanelManager = uIPanelManager;

        GameManager.Instance.OnWaveStart += UpdateWaveUI;

        UpdateWaveUI(1);
        UpdateExpUI();
        UpdateLevelUI();
        UpdateHealthUI();
    }
    
    public void UpdateWaveUI(int wave)
    {
        waveText.text = wave.ToString();
    }

    public void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }

    public void UpdateLevelUI()
    {
        int playerLevel = playerStatHandler.currentLevel;

        levelText.text = playerLevel.ToString();
        UpdateHealthUI();
    }

    public void UpdateExpUI()
    {
        int playerExp = playerStatHandler.currentExperience;
        ExperienceDatas experienceData = playerStatHandler.ExperienceData;

        int previousExpValue = experienceData.totalExperience - experienceData.experienceRequired;
        int maxExpValue = experienceData.experienceRequired;

        expGaugeSlider.value = (playerExp - previousExpValue) / (float)maxExpValue;
    }


}
