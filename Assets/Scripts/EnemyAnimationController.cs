using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private float originalTimeMultiplier;

    private void Start()
    {
        animator = GetComponent<Animator>();
        originalTimeMultiplier = animator.GetFloat("TimeMultiplier");
    }

    public void SetTimeMultiplier(float multiplier)
    {
        animator.SetFloat("TimeMultiplier", multiplier);
    }
}
