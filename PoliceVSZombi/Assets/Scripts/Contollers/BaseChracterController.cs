using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChracterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;

    public event Action<CharacterStatHandler> OnAttackEvent;
    private float timeSinceLastAttack = float.MaxValue;

    protected bool IsAttacking { get; set; }

    protected CharacterStatHandler Stat { get; private set; }

    protected virtual void Awake()
    {
        Stat = GetComponent<CharacterStatHandler>();
    }

    protected virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if (Stat.CurrentWeapon == null)
            return;

        if (timeSinceLastAttack <= Stat.CurrentWeapon.attackCooldown)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (IsAttacking && timeSinceLastAttack > Stat.CurrentWeapon.attackCooldown)
        {
            timeSinceLastAttack = 0f;
            CallAttackEvent(Stat);
        }
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvnet(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent(CharacterStatHandler characterStat)
    {
        OnAttackEvent?.Invoke(characterStat);
    }

}
