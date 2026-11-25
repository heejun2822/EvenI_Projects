using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChracterAnimation : MonoBehaviour
{
    protected Animator animator;
    protected BaseChracterController controller;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<BaseChracterController>();
    }

}
