using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemy : MonoBehaviour
{
    public List<Transform> teleportTargets;
    private int currentTargetIndex = 0;
    
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float handAttackRange;
    [SerializeField] private float spellAttackRange;
    [SerializeField] public float handDamage;
    [SerializeField] public float spellDamage;
    [SerializeField] public float teleportRange;
    [SerializeField] private float moveSpeed = 0;

    public float originalTeleportRange;


    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private Health playerHealth;
    private bool isHandAttacking = false;
    private bool isSpellAttacking = false;
    private bool startTeleporting = false;
    private bool endTeleporting = false;
    private bool isDead = false;
    private bool isStopped = false;
    private bool isFacingRight = true;
    private Transform player1Transform;
    private Transform player2Transform;

    private bool canTeleport = true;

    public float scaleMultiplier = 2f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

private void Start()
    {
        StartCoroutine(SpellAttackCoroutine());
        StartCoroutine(HandAttackCoroutine());
        playerHealth = null;
        originalTeleportRange = teleportRange;
    }

    private void Update()
    {
       if (GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezeAll)
       {
           canTeleport = false;
           isStopped = true;
           anim.SetBool("isStopped", true);
       }
       else
       {
           canTeleport = true;
           isStopped = false;
           anim.SetBool("isStopped", false);
       }
    }

    private void FixedUpdate()
    {
        if (PlayerInSpellRange() && !PlayerInHandAttackRange() && !PlayerInTeleportRange())
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
    isSpellAttacking = true;
    anim.SetBool("isSpellAttacking", true);
    isHandAttacking = false;
    anim.SetBool("isHandAttacking", false);
}

    else if (PlayerInHandAttackRange() && !PlayerInTeleportRange())
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
    isHandAttacking = true;
    anim.SetBool("isHandAttacking", true);
    isSpellAttacking = false;
    anim.SetBool("isSpellAttacking", false);
}

    
    // Check if player is in detection handAttackRange
    else if (PlayerInTeleportRange() && canTeleport)
    {
        StartCoroutine(Teleporting());
        startTeleporting = true;
        anim.SetBool("startTeleporting", true);
    }
    else
    {
        startTeleporting = false;
        anim.SetBool("startTeleporting", false);
        isHandAttacking = false;
        anim.SetBool("isHandAttacking", false);
        StopHandAttack();
        isSpellAttacking = false;
        anim.SetBool("isSpellAttacking", false);
        StopSpellAttack();
    }
}

private bool PlayerInTeleportRange()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, teleportRange, playerLayer);

    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("Player1") || collider.CompareTag("Player2")) // Teleport only when the player enters the trigger
        {
            return true;
        }

        return false;
    }

    return false;
}

    IEnumerator HandAttackCoroutine()
{
    while (!isDead)
    {
        // Wait for the attack cooldown time
        yield return new WaitForSeconds(attackCooldown);

        // Check if player is still in attack handAttackRange
        if (PlayerInHandAttackRange())
        {
            playerHealth.TakeDamage(handDamage);
        }
    }
}

IEnumerator SpellAttackCoroutine()
{
    while (!isDead)
    {
        // Wait for the attack cooldown time
        yield return new WaitForSeconds(attackCooldown);

        // Check if player is still in attack handAttackRange
        if (PlayerInSpellRange())
        {
            playerHealth.TakeDamage(spellDamage);
        }
    }
}

private IEnumerator Teleporting()
{
    teleportRange = 0;
    yield return new WaitForSeconds(0.8f);
    Teleport();
}


    private void StopHandAttack()
    {
            isHandAttacking = false;
            anim.SetBool("isHandAttacking", false);
            playerHealth = null;
    }

    private void StopSpellAttack()
    {
            isSpellAttacking = false;
            anim.SetBool("isSpellAttacking", false);
            playerHealth = null;
    }

    private bool PlayerInHandAttackRange()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * handAttackRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * handAttackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private bool PlayerInSpellRange()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spellAttackRange, playerLayer);

    foreach (Collider2D collider in colliders)
    {
        if (collider != null)
        {
            playerHealth = collider.transform.GetComponent<Health>();
        }

        return collider != null;
    }

    return GetComponent<Collider>() != null;
}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * handAttackRange * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * handAttackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.DrawWireSphere(transform.position, teleportRange);
        Gizmos.DrawWireSphere(transform.position, spellAttackRange);
    }

    private void Teleport()
    {
        if (teleportTargets.Count > 0)
    {
        transform.position = teleportTargets[currentTargetIndex].position; // Teleport to the current target position

        currentTargetIndex++; // Move to the next target position
        if (currentTargetIndex >= teleportTargets.Count)
        {
            currentTargetIndex = 0; // Reset to the first target position if we reach the end
        }
    }

        startTeleporting = false;
        anim.SetBool("startTeleporting", false);

        endTeleporting = true;
        anim.SetBool("endTeleporting", true);

        StartCoroutine(TeleportWait());

        teleportRange = 3;
        // Additional logic, if needed, after teleportation
    }

    private IEnumerator TeleportWait()
{
    yield return new WaitForSeconds(0.8f);
    endTeleporting = false;
    anim.SetBool("endTeleporting", false);
}
}