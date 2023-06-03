using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioSource PickupSound;

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
            // Get the player's time manipulation component
            TimeManipulation timeManipulation = other.GetComponent<TimeManipulation>();
            // Get the player's gravity flip ability component
            FlipGravityAbility flipGravityAbility = other.GetComponent<FlipGravityAbility>();

            if (timeManipulation != null)
            {
                // Increase the time stop counter
                timeManipulation.IncreaseCounter();
            }
            else if (flipGravityAbility != null)
            {
                // Increase the gravity flip counter
                flipGravityAbility.IncrementCounter();
            }

            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        spriteRenderer.enabled = false;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        collider.enabled = false;

        PickupSound.Play();
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }
}
