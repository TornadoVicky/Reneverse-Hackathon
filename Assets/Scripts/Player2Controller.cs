using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
public float moveSpeed = 5f;
public float jumpForce = 10f;
public int maxJumps = 2;
public float jumpTimeLimit = 0.25f;
public LayerMask groundLayers;
public LayerMask wallLayers;
public LayerMask spikeLayer;
public Animator animator;

private Rigidbody2D rb;
private Collider2D coll;
private float jumpTimeCounter;
private int jumpsLeft;

// Added a bool to check if player is moving
private bool isMoving = false;

private bool isDead = false;
private bool isHurt = false;
private bool isRespawned = false;

private bool isDashing = false;
public float dashSpeed = 15f; // Changed from moveSpeed


private bool isAttacking = false;
public float attackRadius = 1.0f;
public float attackDuration = 0.2f;
public float attackCooldown = 1f;
public float damage = 50f;

[SerializeField] private AudioSource P2Dash;
[SerializeField] private AudioSource P2Jump;

Vector2 startPos;
Vector3 respawnPosition;

public LayerMask enemyLayer;

private CircleCollider2D attackCollider;

private void Start()
{
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<Collider2D>();
    animator = GetComponent<Animator>();
    jumpTimeCounter = jumpTimeLimit;
    jumpsLeft = maxJumps;
    attackCollider = GetComponentInChildren<CircleCollider2D>();
    attackCollider.enabled = false;

    startPos = transform.position;
}

private IEnumerator Dash()
{
    isDashing = true;
    animator.SetBool("isDashing", true);
    P2Dash.Play();

    // Change the size and offset of the collider while dashing
    BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
    Vector2 originalSize = boxCollider.size;
    Vector2 originalOffset = boxCollider.offset;
    boxCollider.size = new Vector2(originalSize.x, originalSize.y / 2f);
    boxCollider.offset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y / 4f));

    float originalMoveSpeed = moveSpeed; // Store the original move speed
    moveSpeed = dashSpeed; // Set the move speed to dash speed
    float dashDirection = transform.localScale.x;
    rb.velocity = new Vector2(transform.localScale.x * dashSpeed, rb.velocity.y);
    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    animator.SetBool("isDashing", false);
    isDashing = false;
    moveSpeed = originalMoveSpeed; // Set the move speed back to original

    // Restore the original size and offset of the collider
    boxCollider.size = originalSize;
    boxCollider.offset = originalOffset;
}

public void UpdateRespawnPosition(Vector3 newPosition)
{
    respawnPosition = newPosition;
}

    void Update()
{
float moveInput = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

if (moveInput < 0)
{
    transform.localScale = new Vector3(-1, 1, 1);
}
else if (moveInput > 0)
{
    transform.localScale = new Vector3(1, 1, 1);
}


    if (Input.GetKey(KeyCode.UpArrow) && jumpsLeft > 0)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpTimeCounter = jumpTimeLimit;
        jumpsLeft--;
        animator.SetBool("isGrounded", false);
        animator.SetBool("isMoving", false);
        animator.SetBool("isJumping", true);
        P2Jump.Play();
    }

    if (Input.GetKey(KeyCode.UpArrow) && jumpsLeft > 0)
    {
        if (jumpTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            jumpsLeft--;
        }
    }

    

if (coll.IsTouchingLayers(groundLayers))
{
    jumpsLeft = maxJumps;
    jumpTimeCounter = jumpTimeLimit;
    animator.SetBool("isGrounded", true);

    if (!isDashing)
    {
        animator.SetBool("isMoving", Mathf.Abs(rb.velocity.x) > 0.1f);
    }
    
    if (coll.IsTouchingLayers(spikeLayer))
    {
        animator.SetBool("isDead", true);
        isDead = true;
        StartCoroutine(Respawn());
        //gameObject.SetActive(false); // Disable the player object
    }
    
    animator.SetBool("isJumping", false);
}
else if (coll.IsTouchingLayers(wallLayers) && !coll.IsTouchingLayers(groundLayers))
{
    rb.velocity = Vector2.zero;
    animator.SetBool("isJumping", false);

    if (coll.IsTouchingLayers(spikeLayer))
    {
        animator.SetBool("isDead", true);
        StartCoroutine(Respawn());
        // Set the player to be inactive
        //gameObject.SetActive(false);

    // Trigger the death animation
    
    }

    if (Input.GetKey(KeyCode.UpArrow))
    {
        float jumpDirection = transform.localScale.x * -1f;
        rb.velocity = new Vector2(jumpDirection * moveSpeed, jumpForce);
        animator.SetBool("isJumping", true);
    }
}
else if (!coll.IsTouchingLayers(groundLayers) && !coll.IsTouchingLayers(wallLayers) && coll.IsTouchingLayers(spikeLayer))
{
    animator.SetBool("isDead", true);
    isDead = true;
    StartCoroutine(Respawn());
}

else
{
    animator.SetBool("isGrounded", false);
    animator.SetBool("isJumping", true);
    animator.SetBool("isDead", false);
}

if (Input.GetKey(KeyCode.RightShift) && coll.IsTouchingLayers(groundLayers) && Mathf.Abs(rb.velocity.x) > 0.1f && !isDashing)
{
    StartCoroutine(Dash());
}
if (Input.GetKeyDown(KeyCode.RightControl))
{
    StartCoroutine(Attack());
}
}
private IEnumerator Attack()
{
    isAttacking = true;
    animator.SetBool("isAttacking", true);

    // Enable the attack collider for a duration
    yield return new WaitForSeconds(0.3f);
    attackCollider.enabled = true;
    StartCoroutine(DisableColliderAfterDelay());

    // Detect all colliders in the enemy layer that are overlapping with the attackCollider
    Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackRadius, enemyLayer);
    foreach (Collider2D collider in colliders)
    {
        // Check if the collider has a Health component
        Health health = collider.GetComponent<Health>();

        yield return new WaitForSeconds(attackCooldown);

        if (health != null)
        {
            // Apply damage to the Health component
            health.TakeDamage(damage);
        }
    }
    
    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    animator.SetBool("isAttacking", false);
    isAttacking = false;
}

private IEnumerator DisableColliderAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);
        attackCollider.enabled = false;
    }

    public IEnumerator Respawn()
{
    yield return new WaitForSeconds(0.9f);
    isRespawned = true;
    animator.SetBool("isRespawned", true);
    isDead = false;
    animator.SetBool("isDead", false);
    transform.position = respawnPosition;

    Health health = GetComponent<Health>();
    if (health != null)
    {
        Debug.Log("Calling Reset");
        health.ResetHealth();
    }
}


}