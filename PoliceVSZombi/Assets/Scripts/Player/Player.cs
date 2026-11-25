using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatHandler))]
public class Player : MonoBehaviour
{
    public int characterID = 1;
    PlayerStatHandler statHandler;
    HealthSystem healthSystem;
    public HealthSystem HealthSystem
    {
        get { return healthSystem; } 
    }

    CharacterAimController characterAimController;
    PlayerStatHandler playerStatHandler;
    UIPanelManager panelManager;

    [SerializeField] private RangedWeaponSO rangedWeaponSO;
    RangedWeaponSO defaultWeaponSO;
    public AudioClip weaponSwitchClip;

    PlayInfoPanel playInfoPanel;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        statHandler = GetComponent<PlayerStatHandler>();
        characterAimController = GetComponent<CharacterAimController>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
    }

    public void Initialize()
    {
        panelManager = UIPanelManager.Instance;
        defaultWeaponSO = rangedWeaponSO;
    }

    public void ResetPlayer()
    {
        transform.position = Vector3.zero;

        statHandler.InitializeByKey(characterID);
        if (defaultWeaponSO != null)
            statHandler.EquipWeapon(defaultWeaponSO);
        else
            statHandler.UnEquipWeapon();

        healthSystem.InitHealth();

        panelManager.OpenPanel(UIType.PlayInfoPanel);
        playInfoPanel = panelManager.GetPanel<PlayInfoPanel>(UIType.PlayInfoPanel);
        playInfoPanel.InitUI(this, panelManager);
    }


    public void GainExperience(int experience)
    {
        if (statHandler.GainExperience(experience))
            playInfoPanel.UpdateLevelUI();

        playInfoPanel.UpdateExpUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DropItem dropItem = collision.GetComponent<DropItem>();
        if(dropItem != null )
        {
            characterAimController.EquipItem(dropItem);
            playerStatHandler.EquipWeapon(dropItem.WeaponSo);

            Destroy(dropItem.gameObject);

            SoundManager.PlayClip(weaponSwitchClip);
        }
    }
}
