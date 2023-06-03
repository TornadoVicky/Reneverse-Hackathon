using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animator animator;
    public bool isDead = false;
    private bool isHurt = false;
    private bool isMoving = false;
    private bool isAttacking = false;

    public float maxHealth = 100f;
    public float currentHealth;
    public float damageMultiplier = 1;

    private KillCounter killCounter;

    // Movement script that should be disabled upon death
    public MonoBehaviour healthScript;
    public MonoBehaviour movementScript;

    Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        killCounter = FindObjectOfType<KillCounter>();
    }

    public void TakeDamage(float amount)
    {
        if(!isDead)
        {
            currentHealth -= amount * damageMultiplier;
            if (damageMultiplier * amount > 0) // Check if damage amount is greater than 0
            {
                isHurt = true;
                animator.SetBool("isHurt", true);
                StartCoroutine(ResetHurtState());
            }

            StartCoroutine(ResetHurtState());

            if (currentHealth <= 0)
            {
                if (gameObject.CompareTag("Player2"))
                {
                    animator.SetBool("isDead", true);
                    isDead = true;
                    StartCoroutine(DeadTime());

                    GameObject player = GameObject.FindGameObjectWithTag("Player2");
                    Player2Controller playerController = player.GetComponent<Player2Controller>();
                    playerController.StartCoroutine(playerController.Respawn());
                }
                else if (gameObject.CompareTag("Player1"))
                {
                    animator.SetBool("isDead", true);
                    isDead = true;
                    StartCoroutine(DeadTime());

                    GameObject player = GameObject.FindGameObjectWithTag("Player1");
                    Player1Controller playerController = player.GetComponent<Player1Controller>();
                    playerController.StartCoroutine(playerController.Respawn());
                }
                else
                {
                    damageMultiplier = 0;
                
                    animator.SetBool("isDead", true);
                    isDead = true;

                    animator.SetBool("isAttacking", false);
                    isAttacking = false;

                    animator.SetBool("isMoving", false);
                    isMoving = false;

                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

                    movementScript.enabled = false;
                    healthScript.enabled = false;

                    SetColliderHeightToZero();

                    StartCoroutine(DestroyAfterDelay(1f));
                }
            }
        }
    }

    private IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(0.9f);
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        animator.SetBool("isDead", false);
    }

    private IEnumerator ResetHurtState()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isHurt", false);
        isHurt = false;
    }

    private void SetColliderHeight(float height)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Vector2 originalSize = boxCollider.size;
        Vector2 originalOffset = boxCollider.offset;

        float heightDifference = originalSize.y - height;
        boxCollider.size = new Vector2(originalSize.x, height);
        boxCollider.offset = new Vector2(originalOffset.x, originalOffset.y - (heightDifference / 2f));
    }

    private void SetColliderHeightToZero()
    {
        SetColliderHeight(0.01f);
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        killCounter.IncrementKillCount();
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public float GetCurrentHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
