using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeEnemyController : EnemyController
{
    [SerializeField] [Range(0f,100f)] private float followRange;

    [SerializeField] private SpriteRenderer characterRenderer;
    private CircleCollider2D characterCollider;

    private HealthSystem healthSystem;

    private HealthSystem collidingTargetHealthSystem;
    private CharacterMovementController collidingTargetMovement;

    private MeleeWeaponSO weaponSo;

    protected override void Start()
    {
        base.Start();

        OnAttackEvent += Attack;

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
    }

    private void Attack(CharacterStatHandler characterStat)
    {
        int damage = characterStat.CurrentStat.BaseAttack + characterStat.CurrentWeapon.baseDamage;
        bool hasBeenChanged = collidingTargetHealthSystem.ChangeHealth(-damage);
        if (hasBeenChanged && Stat.CurrentWeapon.isOnKnockback && collidingTargetMovement)
        {
            collidingTargetMovement.ApplyKnockback(transform, characterStat.CurrentWeapon.knockbackPower, characterStat.CurrentWeapon.knockbackDuration);
        }
    }

    public override void EquipWeapon()
    {
        if (Stat.CurrentWeapon == null) return;

        weaponSo = Stat.CurrentWeapon as MeleeWeaponSO;
        characterCollider = GetComponent<CircleCollider2D>();
        if (characterCollider == null) return;

        characterCollider.radius = weaponSo.attackRange;
        Vector2 offset = characterCollider.offset;
        offset.y = 0.5f;
        characterCollider.offset = offset;
    }

    private void OnDamage()
    {
        followRange = 100f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector2 direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;
        Player player = receiver.GetComponent<Player>();

        if (player == null) return;

        collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if(collidingTargetHealthSystem != null)
        {
            IsAttacking = true;
            collidingTargetMovement = receiver.GetComponent<CharacterMovementController>();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;
        Player player = receiver.GetComponent<Player>();

        if (player == null) return;

        IsAttacking = false;
    }

}
