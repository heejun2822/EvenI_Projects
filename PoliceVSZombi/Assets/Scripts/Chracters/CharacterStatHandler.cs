using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatHandler : MonoBehaviour
{
    private CharacterStat baseStat;
    public CharacterStat CurrentStat { get; private set; }
    public WeaponSO CurrentWeapon { get; private set; }
    private List<StatModifier> modifiers;

    protected virtual void Awake()
    {
        modifiers = new List<StatModifier>();
        CurrentStat = new CharacterStat();
    }

    public void Initialize(CharacterStat baseStat)
    {
        this.baseStat = baseStat;
        CalculateFinalStat();
    }

    public virtual void InitializeByKey(int key)
    {

    }

    public void EquipWeapon(WeaponSO weapon)
    {
        if (CurrentWeapon != null)
        {
            UnEquipWeapon();
        }

        CurrentWeapon = weapon;
        AddModifier(weapon.modifiers);
        CalculateFinalStat();
    }

    public void UnEquipWeapon()
    {
        if (CurrentWeapon == null) return;

        RemoveModifier(CurrentWeapon.modifiers);
        CurrentWeapon = null;
        CalculateFinalStat();
    }

    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        CalculateFinalStat();
    }

    public void AddModifier(StatModifier[] modifierArray)
    {
        modifiers.AddRange(modifierArray);
        CalculateFinalStat();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        modifiers.Remove(modifier);
        CalculateFinalStat();
    }

    public void RemoveModifier(StatModifier[] modifierArray)
    {
        foreach (StatModifier modifier in modifierArray)
        {
            modifiers.Remove(modifier);
        }
        CalculateFinalStat();
    }



    void CalculateFinalStat()
    {
        int health = baseStat.BaseHealth;
        float speed = baseStat.BaseSpeed;
        int attack = baseStat.BaseAttack;

        int flatHealth = 0;
        float flatSpeed = 0;
        int flatAttack = 0;

        float percentHealth = 1;
        float percentSpeed = 1;
        float percentAttack = 1;

        foreach (StatModifier modifier in modifiers)
        {
            switch (modifier.statType)
            {
                case StatType.Attack:
                    if (modifier.modType == StatModType.Flat)
                        flatAttack += (int)modifier.value;
                    else if (modifier.modType == StatModType.Percent)
                        percentAttack += modifier.value / 100f;
                    break;
                case StatType.Speed:
                    if (modifier.modType == StatModType.Flat)
                        flatSpeed += (int)modifier.value;
                    else if (modifier.modType == StatModType.Percent)
                        percentSpeed += modifier.value / 100f;
                    break;
                case StatType.Health:
                    if (modifier.modType == StatModType.Flat)
                        flatHealth += (int)modifier.value;
                    else if (modifier.modType == StatModType.Percent)
                        percentHealth += modifier.value / 100f;
                    break;
            }
        }

        CurrentStat.Initialize(
            (int)((health + flatHealth) * percentHealth),
            (speed + flatSpeed) * percentSpeed,
            (int)((attack + flatAttack) * percentAttack)
        );
    }
}

