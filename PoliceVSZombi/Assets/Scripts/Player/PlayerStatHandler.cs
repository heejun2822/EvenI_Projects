using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : CharacterStatHandler
{
    public int currentLevel;
    public int currentExperience;

    private ExperienceDatas experienceData;
    public ExperienceDatas ExperienceData
    {
    get { return experienceData; } 
    }

    private CharacterDatas characterData;

    private int maxLevel;

    HealthSystem healthSystem;
    private DataManager dataManager;

    protected override void Awake()
    {
        base.Awake();
        healthSystem = GetComponent<HealthSystem>();
    }

    public override void InitializeByKey(int characterID)
    {
        dataManager = DataManager.Instance;

        characterData = dataManager.CharacterDatasLoader.GetByKey(characterID);

        maxLevel = characterData.maxLevel - characterData.minLevel + 1;

        experienceData = dataManager.ExperienceDatasLoader.GetByKey(characterData.minLevel);

        PlayerStatDatas playerStatData = dataManager.PlayerStatDatasLoader.GetByKey(experienceData.statID);

        CharacterStat characterStat = new CharacterStat();
        characterStat.Initialize(playerStatData.baseHealth, playerStatData.baseSpeed, playerStatData.baseAttack);

        base.Initialize(characterStat);

        currentLevel = 1;
        currentExperience = 0;
    }

    public bool GainExperience(int amount)
    {
        currentExperience += amount;

        if(currentExperience >= experienceData.totalExperience)
        {
            LevelUp();
            healthSystem.InitHealth();
            return true;
        }

        return false;
    }

    private void LevelUp()
    {
        if (currentLevel >= maxLevel)
        {
            Debug.Log("Max level reached. No more leveling up.");
            return;
        }

        currentLevel++;

        ExperienceDatas nextLevelData = dataManager.ExperienceDatasLoader.GetByKey(currentLevel + characterData.minLevel - 1);
        if (nextLevelData != null)
        {
            experienceData = nextLevelData;
            PlayerStatDatas playerStatData = dataManager.PlayerStatDatasLoader.GetByKey(experienceData.statID);
            CharacterStat characterStat = new CharacterStat();
            characterStat.Initialize(playerStatData.baseHealth, playerStatData.baseSpeed, playerStatData.baseAttack);

            base.Initialize(characterStat);
        }
        else
        {
            Debug.Log("No experience data for the next level");
        }

    }
}
