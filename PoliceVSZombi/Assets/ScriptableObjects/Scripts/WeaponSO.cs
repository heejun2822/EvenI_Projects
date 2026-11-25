using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/Weapon Data")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public int baseDamage;
    public float attackCooldown;

    public LayerMask target;

    public bool isOnKnockback;
    public float knockbackPower;
    public float knockbackDuration;

    public StatModifier[] modifiers;

    public AudioClip soundClip;
    public float volumeMultiplier;
}
