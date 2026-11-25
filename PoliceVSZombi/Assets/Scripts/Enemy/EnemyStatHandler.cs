using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatHandler : CharacterStatHandler
{
    private DataManager dataManager;
    private int experience = 0;
    public int Experience { get { return experience; } }

    private int dropRate = 0;
    public int DropRate {  get { return dropRate; } }

    public override void InitializeByKey(int enemyID)
    {
        dataManager = DataManager.Instance;

        EnemyDatas enemyData = dataManager.EnemyDatasLoader.GetByKey(enemyID);
        EnemyStatDatas enemyStatData = dataManager.EnemyStatDatasLoader.GetByKey(enemyData.statsId);
        experience = enemyStatData.experienceGiven;
        dropRate = enemyData.dropRate;


        CharacterStat stat = new CharacterStat();
        stat.Initialize(enemyStatData.baseHealth, enemyStatData.baseSpeed, enemyStatData.baseAttack);

        base.Initialize(stat);
    }


}
