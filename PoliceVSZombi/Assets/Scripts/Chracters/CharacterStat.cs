using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat
{
    private int baseHealth;
    private float baseSpeed;
    private int baseAttack;

    private const int minHealth = 0;
    private const int maxHealth = 1000;
    private const float minSpeed = 1.0f;
    private const float maxSpeed = 10.0f;
    private const int minAttack = 0;
    private const int maxAttack = 500;

    public float BaseSpeed
    {
        get => baseSpeed;
        set => baseSpeed = Mathf.Clamp(value, minSpeed, maxSpeed);
    }

    public int BaseHealth
    {
        get => baseHealth;
        set => baseHealth = Mathf.Clamp(value, minHealth, maxHealth);
    }

    public int BaseAttack
    {
        get => baseAttack;
        set => baseAttack = Mathf.Clamp(value, minAttack, maxAttack);
    }

    public void Initialize(int health, float speed, int attack)
    {
        BaseHealth = health;
        BaseSpeed = speed;
        BaseAttack = attack;
    }

}
