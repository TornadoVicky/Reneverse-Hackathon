using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathTracker : MonoBehaviour
{
    public GameObject enemyToTrack;
    public bool isEnemyDead;

    private void Start()
    {
        if (enemyToTrack == null)
        {
            Debug.LogError("Enemy to track is not assigned!");
        }
    }

    private void Update()
    {
        if (!isEnemyDead && enemyToTrack == null)
        {
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        // Perform actions when the enemy dies
        
        // Set the isEnemyDead flag to true
        isEnemyDead = true;

        // Disable or destroy this component if needed
        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
