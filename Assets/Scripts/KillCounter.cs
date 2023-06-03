using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static int enemyKillCount; // Static variable for shared kill count
    public int threshold;

    public void IncrementKillCount()
    {
        enemyKillCount++; // Increment the shared kill count
    }

    private void Update()
    {
        if (enemyKillCount >= threshold)
        {
            Destroy(gameObject);
        }
    }
}
