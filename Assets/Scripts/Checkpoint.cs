using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player2"))
        {
            // Get the player's controller component
            Player2Controller playerController = other.GetComponent<Player2Controller>();
            
            if (playerController != null)
            {
                // Update the current respawn position in the player controller
                playerController.UpdateRespawnPosition(transform.position);
            }
        }
        else if (other.CompareTag("Player1"))
        {
            // Get the player's controller component
            Player1Controller playerController = other.GetComponent<Player1Controller>();
            
            if (playerController != null)
            {
                // Update the current respawn position in the player controller
                playerController.UpdateRespawnPosition(transform.position);
            }
        }
    }
}
