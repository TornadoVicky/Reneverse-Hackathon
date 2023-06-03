using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] private AudioSource HealSound;

    public int healthIncreaseAmount = 20;

    private void Start()
    {
        // Get the reference to the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        
        
        // Check if the colliding object is the player
        if (other.CompareTag("Player2") || other.CompareTag("Player1"))
        {   
            
            // Get the player's health component
            Health playerHealth = other.GetComponent<Health>();
            
            if (playerHealth != null)
            {
                // Increase the player's health
                playerHealth.IncreaseHealth(healthIncreaseAmount);
                
                
                StartCoroutine(Wait());
                
            }
        }
    }

    private IEnumerator Wait()
    {
        spriteRenderer.enabled = false;
        HealSound.Play();
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }
}
