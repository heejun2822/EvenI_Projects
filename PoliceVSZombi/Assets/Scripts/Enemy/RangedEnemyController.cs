using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float shootRange = 10f;

    private RangedWeaponSO weaponSO;

    public override void EquipWeapon()
    {
        if (Stat.CurrentWeapon == null)
            return;

        weaponSO = Stat.CurrentWeapon as RangedWeaponSO;
        shootRange = weaponSO.shootRange;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = false;

        if (distance <= followRange)
        {
            if (distance <= shootRange)
            {
                int layerMaskTarget = Stat.CurrentWeapon.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootRange, (1 << LayerMask.NameToLayer("Level") | layerMaskTarget));

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | 1 << hit.collider.gameObject.layer))
                {
                    CallLookEvnet(direction);
                    CallMoveEvent(Vector2.zero);
                    IsAttacking = true;
                    return;
                }
            }


            CallMoveEvent(direction);
        }
    }

}
