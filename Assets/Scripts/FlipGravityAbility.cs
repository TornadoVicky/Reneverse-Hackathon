using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipGravityAbility : MonoBehaviour
{
    public float gravityDirection = 1f;
    public float jumpForce = 5f;
    public AudioSource abilitySound1;
    public AudioSource abilitySound2;

    private Rigidbody2D rb;
    public int abilityCounter = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void IncrementCounter()
    {
        abilityCounter++;
        abilityCounter++;
    }

    public void ActivateAbility()
    {
        if (abilityCounter > 0)
        {
            FlipGravity();
            abilityCounter--;
        }
    }

    private void FlipGravity()
    {
        // Invert the gravity direction
        gravityDirection *= -1;

        // Flip the character on the Y-axis
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);

        // Play the ability sound based on the gravity direction
        if (gravityDirection == -1)
        {
            abilitySound1.Play();
        }
        else
        {
            abilitySound2.Play();
        }

        // Reverse the character's rigidbody gravity scale
        rb.gravityScale = gravityDirection;

        // Reverse the jump force direction
        jumpForce *= -1;
    }
}
