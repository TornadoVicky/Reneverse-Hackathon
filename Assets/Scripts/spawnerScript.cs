using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public float detectionRange;
    private Transform playerTransform;
    public LayerMask playerLayer;
    public GameObject parent;
    public GameObject bullet;
    public float fireRate = 1f;
    private float nextFireTime;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player2").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if (PlayerInDetectionRange() && nextFireTime<Time.time)
       {
        Instantiate(bullet, parent.transform.position, Quaternion.identity);
        nextFireTime = Time.time + fireRate;
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
