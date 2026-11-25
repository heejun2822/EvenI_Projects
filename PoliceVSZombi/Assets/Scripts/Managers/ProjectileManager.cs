using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] ParticleSystem impactParicleSyatem;

    private ObjectPool objectPool;

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void ShootBullet(Vector2 startPoistion, Vector2 direction,CharacterStatHandler characterStatHandler, RangedWeaponSO rangedWeaponSO)
    {
        GameObject obj = objectPool.SpawnFromPool(rangedWeaponSO.bulletNameTag);

        obj.transform.position = startPoistion;
        BulletController bulletController = obj.GetComponent<BulletController>();
        bulletController.Initialize(direction,characterStatHandler.CurrentStat, rangedWeaponSO, this);

        obj.SetActive(true);
    }

    public void CreateImpactParticleAtPosition(Vector3 position)
    {
        impactParicleSyatem.transform.position = position;
        impactParicleSyatem.Play();
    }

    public void ResetObjecPool()
    {
        objectPool.ResetPoolDictionary();
    }

}
