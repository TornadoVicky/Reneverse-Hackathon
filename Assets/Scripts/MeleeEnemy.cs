using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] public float damage;
    [SerializeField] public float detectionRange;
    [SerializeField] private float moveSpeed;

    public float originalMovementSpeed;


    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private Health playerHealth;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isMoving = false;
    private bool isStopped = false;
    private bool isFacingRight = true;
    private Transform playerTransform;

    public float scaleMultiplier = 2f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

private void Start()
    {
        StartCoroutine(AttackCoroutine());
        playerHealth = null;
        originalMovementSpeed = moveSpeed;
    }

    private void Update()
{
    if (GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezeAll)
    {
        moveSpeed = 0;
        isStopped = true;
        anim.SetBool("isStopped", true);
    }
    else
    {
        moveSpeed = originalMovementSpeed;
        isStopped = false;
        anim.SetBool("isStopped", false);
    }
}

    private void FixedUpdate()
    {
    if (PlayerInSight() && !PlayerInDetectionRange())
    {
    GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
    GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

    float distanceToPlayer1 = player1 != null ? Vector2.Distance(transform.position, player1.transform.position) : Mathf.Infinity;
    float distanceToPlayer2 = player2 != null ? Vector2.Distance(transform.position, player2.transform.position) : Mathf.Infinity;

    float direction;
    Transform playerTransform;

    if (distanceToPlayer2 < distanceToPlayer1)
    {
        direction = (player2.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        playerTransform = player2.transform;
    }
    else
    {
        direction = (player1.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        playerTransform = player1.transform;
    }

    transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
    transform.localScale = new Vector3(scaleMultiplier * direction, transform.localScale.y, transform.localScale.z);
        isMoving = true;
        anim.SetBool("isMoving", true);
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
    
    // Check if player is in detection range
    else if (PlayerInDetectionRange())
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
    }
    else
    {
        isMoving = false;
        anim.SetBool("isMoving", false);
        StopAttack();
    }
}

private bool PlayerInDetectionRange()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);

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
        if (PlayerInDetectionRange())
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
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
