using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStatHandler enemyStatHandler;
    HealthSystem healthSystem;
    public HealthSystem HealthSystem
    {
        get { return healthSystem; } 
    }

    EnemyController enemyController;

    public int enemyID = 1;
    [SerializeField] private WeaponSO weaponSO;

    private void Awake()
    {
        enemyStatHandler = GetComponent<EnemyStatHandler>();
        healthSystem = GetComponent<HealthSystem>();
        enemyController = GetComponent<EnemyController>();
    }

    public void  Initialize(int enemyId)
    {
        enemyID = enemyId;
        Initialize();
    }

    public void Initialize()
    {
        enemyStatHandler.InitializeByKey(enemyID);
        enemyStatHandler.EquipWeapon(weaponSO);

        enemyController.EquipWeapon();

        healthSystem.InitHealth();
    }

    public int GetExperience()
    {
        return enemyStatHandler.Experience;
    }

    public int GetDropRate()
    {
        return enemyStatHandler.DropRate;   
    }
}
