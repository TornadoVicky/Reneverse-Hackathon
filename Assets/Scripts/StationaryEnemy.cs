using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : MonoBehaviour
{
    public float detectionRange;
    private Transform playerTransform;
    public LayerMask playerLayer;
    public GameObject parent;
    public GameObject bullet;
    public float fireRate = 1f;
    private float nextFireTime;

    private Animator anim;
    private bool isFiring = false;
    private bool isStopped = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player2").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezeAll)
    {
        isStopped = true;
        anim.SetBool("isStopped", true);
    }
    else
    {
        isStopped = false;
        anim.SetBool("isStopped", false);
    }

        if (PlayerInDetectionRange())
        {
            isFiring = true;
            anim.SetBool("isFiring", true);

            if (nextFireTime < Time.time)
            {
                GameObject newBullet = Instantiate(bullet, parent.transform.position, Quaternion.identity);
                FireBall bulletScript = newBullet.GetComponent<FireBall>();

                Vector3 direction = (playerTransform.position - parent.transform.position).normalized;
                bulletScript.SetInitialDirection(direction);

                nextFireTime = Time.time + fireRate;
            }

            // Face left or right based on player's location
            if (playerTransform.position.x < transform.position.x)
                transform.localScale = new Vector3(-1f, 1f, 1f); // Flip to face left
            else
                transform.localScale = new Vector3(1f, 1f, 1f); // Flip to face right
        }
        else
        {
            isFiring = false;
            anim.SetBool("isFiring", false);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
