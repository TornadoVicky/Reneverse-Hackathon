using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Health playerHealth;
    public Gradient healthGradient; // Gradient for the health bar fill color

    // Start is called before the first frame update
    void Start()
    {
        // Set the color of the health slider fill using the gradient
        Image fillImage = healthSlider.fillRect.GetComponent<Image>();
        fillImage.color = healthGradient.Evaluate(1f); // Set initial color at maximum health
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = playerHealth.GetCurrentHealthPercentage();

        // Update the color of the health slider fill based on the current health percentage
        Image fillImage = healthSlider.fillRect.GetComponent<Image>();
        fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
