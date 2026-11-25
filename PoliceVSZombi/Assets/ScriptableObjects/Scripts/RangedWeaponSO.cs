using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Weapon/Ranged Weapon Data")]
public class RangedWeaponSO : WeaponSO
{
    [Header("Ranged Attack Data")]
    public string bulletNameTag;
    public float speed;
    public float duration;
    public float spread;
    public int numberOfProjectilesPerShot;
    public float multipleProjectilesAngel;

    public float shootRange;

    public Sprite weaponSprite;
    public Vector3 offset;

}
