using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : BaseChracterAnimation
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsHit = Animator.StringToHash("IsHit");

    HealthSystem healthSystem;

    protected override void Awake()
    {
        base.Awake();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Start()
    {
        controller.OnMoveEvent += Move;

        if(healthSystem != null)
        {
            healthSystem.OnDamage += Hit;
            healthSystem.OnInvicibilityEnd += InvicibilityEnd;
        }
    }

    void Move(Vector2 obj)
    {
        animator.SetBool(IsWalking, obj.magnitude > .5f);
    }
    void Hit()
    {
        animator.SetBool(IsHit, true);
    }

    void InvicibilityEnd()
    {
        animator.SetBool(IsHit, false);
    }

}
