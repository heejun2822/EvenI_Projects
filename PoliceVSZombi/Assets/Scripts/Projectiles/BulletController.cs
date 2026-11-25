using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedWeaponSO rangedWeaponSO;
    private CharacterStat characterStat;
    private float currentDuration;
    private Vector2 direction;
    private bool isReady;

    private Rigidbody2D rigidbody;

    private TrailRenderer trailRenderer;
    // 

    public bool fxOnDestroy = true;
    ProjectileManager projectileManager;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void Initialize(Vector2 direction, CharacterStat characterStat, RangedWeaponSO rangedWeaponSO, ProjectileManager projectileManager)
    {
        this.projectileManager = projectileManager;

        this.rangedWeaponSO = rangedWeaponSO;
        this.characterStat = characterStat;
        this.direction = direction;

        trailRenderer.Clear();
        currentDuration = 0;

        transform.right = direction;

        isReady = true;
    }

    private void Update()
    {
        if (!isReady) return;

        currentDuration += Time.deltaTime;

        if (currentDuration > rangedWeaponSO.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        rigidbody.velocity = direction * rangedWeaponSO.speed;
    }

    void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            projectileManager.CreateImpactParticleAtPosition(position);
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        if(levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer )))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestroy);
            return;
        }

        if (!rangedWeaponSO)
            return;

        if (rangedWeaponSO.target.value == (rangedWeaponSO.target.value | (1 << collision.gameObject.layer)))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                bool changed = healthSystem.ChangeHealth(-(characterStat.BaseAttack + rangedWeaponSO.baseDamage));
                if (changed && rangedWeaponSO.isOnKnockback)
                {
                    CharacterMovementController movement = collision.GetComponent<CharacterMovementController>();
                    if(movement != null)
                    {
                        movement.ApplyKnockback(transform, rangedWeaponSO.knockbackPower, rangedWeaponSO.knockbackDuration);
                    }
                }
            }

            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }

    }
}
