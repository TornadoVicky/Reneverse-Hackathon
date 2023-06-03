using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doppelganger : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] public float damage;
    [SerializeField] public float attackingRange;
    [SerializeField] private float moveSpeed;


    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider1;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private Health playerHealth;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isDashing = false;
    private bool isMoving = false;
    private bool isFacingRight = true;
    private Transform playerTransform;

    public float scaleMultiplier = 2f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player2").transform;
        StartCoroutine(AttackCoroutine());
        playerHealth = null;
    }

    private void FixedUpdate()
{
    float direction = (playerTransform.position.x - transform.position.x) > 0 ? 1 : -1;
    transform.localScale = new Vector3(scaleMultiplier * direction, transform.localScale.y, transform.localScale.z);
    float distance = Vector3.Distance(playerTransform.position, transform.position);

    if (PlayerInSight() && !PlayerInAttackingRange() && distance < range / 6)
    {
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
        isDashing = false;
        anim.SetBool("isDashing", false);
        isMoving = true;
        anim.SetBool("isMoving", true);
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
    else if (PlayerInSight() && !PlayerInAttackingRange() && distance > range / 6)
    {
        transform.position += new Vector3(direction * (moveSpeed + 2) * Time.deltaTime, 0, 0);
        isDashing = true;
        anim.SetBool("isDashing", true);
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
    // Check if player is in detection range
    else if (PlayerInAttackingRange())
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
    }
    else if (!PlayerInSight())
    {
        isDashing = false;
        anim.SetBool("isDashing", false);
        isMoving = false;
        anim.SetBool("isMoving", false);
        StopAttack();
    }
}


private bool PlayerInAttackingRange()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackingRange, playerLayer);

    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("Player1") || collider.CompareTag("Player2"))
        {
            playerTransform = collider.transform;
            return true;
        }
    }

    return false;
}

    IEnumerator AttackCoroutine()
{
    while (!isDead)
    {
        // Wait for the attack cooldown time
        yield return new WaitForSeconds(attackCooldown);

        // Check if player is still in attack range
        if (PlayerInAttackingRange())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}

    private void StopAttack()
    {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
            playerHealth = null;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider1.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider1.bounds.size.x * range, boxCollider1.bounds.size.y, boxCollider1.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider1.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider1.bounds.size.x * range, boxCollider1.bounds.size.y, boxCollider1.bounds.size.z));
        Gizmos.DrawWireSphere(transform.position, attackingRange);
    }
}
