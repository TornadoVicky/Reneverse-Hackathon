using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FEnemy : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float detectionRange = 3f;

    private Health playerHealth;
    private bool isDead = false;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Animator anim;
    private bool isMoving = false;

    private bool isAttacking = false;

    public float damage = 10f;
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    Seeker seeker;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = null;

        player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        Transform targetPlayer = GetNearestPlayer();
        if(targetPlayer == null || Vector2.Distance(transform.position, targetPlayer.position) > detectionRange)
        {
            isMoving = false;
            anim.SetBool("isMoving", false);
            return; // Exit early if target is null or out of range
        }

        if(targetPlayer != null && seeker.IsDone())
        {
            isMoving = true;
            anim.SetBool("isMoving", true);
            seeker.StartPath(rb.position, targetPlayer.position, OnPathComplete);
        }
    }

   Transform GetNearestPlayer()
{
    Transform nearestPlayer = null;
    float nearestDistance = Mathf.Infinity;
    
    if (Vector2.Distance(rb.position, player1.position) <= detectionRange)
    {
        nearestPlayer = player1;
        nearestDistance = Vector2.Distance(rb.position, player1.position);
    }

    if (Vector2.Distance(rb.position, player2.position) <= detectionRange)
    {
        float distanceToPlayer2 = Vector2.Distance(rb.position, player2.position);
        if (distanceToPlayer2 < nearestDistance)
        {
            nearestPlayer = player2;
            nearestDistance = distanceToPlayer2;
        }
    }

    return nearestPlayer;
}



    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the collided object is a player
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            // Check if enough time has passed since the last damage dealt
            if (Time.time - lastDamageTime > damageCooldown)
            {
                isAttacking = true;
                anim.SetBool("isAttacking", true);

                // Get the Health component from the collided object
                Health playerHealth = other.gameObject.GetComponent<Health>();

                // Deal damage to the player
                playerHealth.TakeDamage(damage);

                // Update last damage time
                lastDamageTime = Time.time;

            }
        }
    }
}
