using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseChracterController
{
    GameManager gameManager;

    protected Player ClosetTarget { get; private set; }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;
        ClosetTarget = gameManager.Player;
    }

    public virtual void EquipWeapon()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, ClosetTarget.transform.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (ClosetTarget.transform.position - transform.position).normalized;
    }


}
