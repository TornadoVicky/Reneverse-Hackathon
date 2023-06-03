using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    public float originalMovementSpeed;
    private Vector3 initialDirection;
    public float damage = 10f;
    private Animator anim;

    private bool isStopped = false;

    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        playerHealth = null;
        Destroy(this.gameObject, 2);
        originalMovementSpeed = speed;
    }

    private void Update()
    {
        if (GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezeAll)
    {
        speed = 0;
        isStopped = true;
        anim.SetBool("isStopped", true);
    }
    else
    {
        speed = originalMovementSpeed;
        isStopped = false;
        anim.SetBool("isStopped", false);
    }
        transform.position += initialDirection * speed * Time.deltaTime;
    }

    public void SetInitialDirection(Vector3 direction)
    {
        initialDirection = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("Ground"))
        {
            // Get the Health component from the player
            Health playerHealth = other.GetComponent<Health>();

            // Deal damage to the player
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
