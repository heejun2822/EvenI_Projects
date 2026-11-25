using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnDeath : MonoBehaviour
{
    HealthSystem healthSystem;
    Rigidbody2D rigid;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += OnDeath;
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnDeath()
    {
        rigid.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
