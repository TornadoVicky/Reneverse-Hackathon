using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float attackRange = 0.5f; // The radius of the attack range
    public float attackDuration = 0.5f; // The duration of the attack in seconds

    private bool isAttacking = false;
    private CircleCollider2D attackCollider;

    private void Start()
    {
        // Find the attack collider component
        attackCollider = transform.Find("Attack Check").GetComponent<CircleCollider2D>();
        // Disable the attack collider by default
        attackCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
        {
            // Start the attack coroutine
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        // Enable the attack collider
        attackCollider.enabled = true;
        // Wait for the attack duration
        yield return new WaitForSeconds(attackDuration);
        // Disable the attack collider
        attackCollider.enabled = false;
        isAttacking = false;
    }
}
