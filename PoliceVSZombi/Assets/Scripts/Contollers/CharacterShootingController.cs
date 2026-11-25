using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShootingController : MonoBehaviour
{
    ProjectileManager projectileManager;
    BaseChracterController characterController;
    [SerializeField] Transform projectileSpawnPosition;
    private Vector2 aimDirection = Vector2.right;

    private void Awake()
    {
        characterController = GetComponent<BaseChracterController>();   
    }

    private void Start()
    {
        projectileManager = ProjectileManager.Instance;

        characterController.OnLookEvent += OnAim;
        characterController.OnAttackEvent += OnShoot;
    }

    void OnAim(Vector2 newAimDirection)
    {
        aimDirection = newAimDirection;
    }

    void OnShoot(CharacterStatHandler characterStatHandler)
    {
        WeaponSO weaponSO = characterStatHandler.CurrentWeapon;
        RangedWeaponSO rangedWeaponSO = weaponSO as RangedWeaponSO;

        float projectileAngelSpace = rangedWeaponSO.multipleProjectilesAngel;
        float numberOfProjectilesPerShot = rangedWeaponSO.numberOfProjectilesPerShot;

        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectileAngelSpace + 
            0.5f * rangedWeaponSO.multipleProjectilesAngel;

        for(int i = 0; i< numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectileAngelSpace * i;
            float randomSpread = Random.Range(-rangedWeaponSO.spread, rangedWeaponSO.spread);
            angle += randomSpread;
            CreateProjectile(characterStatHandler,rangedWeaponSO, angle);
        }

        if (rangedWeaponSO.soundClip != null)
        {
            SoundManager.PlayClip(rangedWeaponSO.soundClip, rangedWeaponSO.volumeMultiplier);
        }
    }

    private void CreateProjectile(CharacterStatHandler characterStatHandler, RangedWeaponSO weaponSO, float angle)
    {
        projectileManager.ShootBullet(
            projectileSpawnPosition.position,
            RotateVector2(aimDirection, angle),
            characterStatHandler,
            weaponSO
            );
    }

    public static Vector2 RotateVector2(Vector2 v , float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
