using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private BaseChracterController _contoller;
    CharacterStatHandler statHandler;

    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    private void Awake()
    {
        _contoller = GetComponent<BaseChracterController>();
        _rigidbody = GetComponent<Rigidbody2D>();

        statHandler = GetComponent<CharacterStatHandler>();
    }


    void Start()
    {
        _contoller.OnMoveEvent += Move;
    }

    void FixedUpdate()
    {
        ApplyMovement(_movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * statHandler.CurrentStat.BaseSpeed;
        if (knockbackDuration > 0.0f)
        {
            direction += knockback;
        }
        _rigidbody.velocity = direction;
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }

}
