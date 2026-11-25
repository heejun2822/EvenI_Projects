using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Speed,
    Attack
}

public enum StatModType
{
    Flat,       // °íÁ¤°ª Áõ°¡ (µ¡¼À)
    Percent,    // ºñ¿ï·Î Áõ°¡ (°ö¼À)
}

[Serializable]
public class StatModifier
{
    public StatType statType;
    public StatModType modType;
    public float value;

    public StatModifier(StatType statType, StatModType modType, float value)
    {
        this.statType = statType;
        this.modType = modType;
        this.value = value;
    }
}
