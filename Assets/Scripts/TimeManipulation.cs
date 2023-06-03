using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulation : MonoBehaviour
{
    public float timeStopDuration = 30f;
    public float timeStopCooldown = 60f;
    public GameObject[] enemyControllers;

    [SerializeField] private AudioSource ManipulationSound;

    public GameObject globalLightObject;
    public Color timeStopLightColor = Color.blue;
    public Color normalLightColor = Color.white;
    private UnityEngine.Rendering.Universal.Light2D globalLight;

    private bool isTimeManipulationActive = false;
    private float timeManipulationTimer = 0f;
    private float timeStopCooldownTimer = 0f;
    public float timeStopCounter = 0f;

    private void Start()
    {
        globalLight = globalLightObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && timeStopCounter != 0 && timeStopCooldownTimer <= 0f)
        {
            ManipulationSound.Play();
            StartTimeManipulation();
            timeStopCounter--;
        }

        if (isTimeManipulationActive)
        {
            timeManipulationTimer -= Time.deltaTime;
            if (timeManipulationTimer <= 0f)
            {
                StopTimeManipulation();
            }
        }
        else
        {
            timeStopCooldownTimer -= Time.deltaTime;
        }
    }

    private void StartTimeManipulation()
    {
        isTimeManipulationActive = true;
        timeManipulationTimer = timeStopDuration;
        timeStopCooldownTimer = timeStopCooldown;

        globalLight.color = timeStopLightColor;

        Health health = GetComponent<Health>();
        health.damageMultiplier = 0f;

        foreach (GameObject enemyController in enemyControllers)
        {
            if (enemyController != null)
            {
                Animator animator = enemyController.GetComponent<Animator>();
                Rigidbody2D rigidbody2D = enemyController.GetComponent<Rigidbody2D>();

                animator.SetFloat("TimeMultiplier", 0f);

                // Freeze the Rigidbody constraints
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void StopTimeManipulation()
    {
        isTimeManipulationActive = false;

        globalLight.color = normalLightColor;

        Health health = GetComponent<Health>();
        health.damageMultiplier = 1f;

        foreach (GameObject enemyController in enemyControllers)
        {
            if (enemyController != null)
            {
                Animator animator = enemyController.GetComponent<Animator>();
                Rigidbody2D rigidbody2D = enemyController.GetComponent<Rigidbody2D>();

                animator.SetFloat("TimeMultiplier", 1f);

                // Unfreeze the Rigidbody constraints
                rigidbody2D.constraints = RigidbodyConstraints2D.None;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    public void IncreaseCounter()
{
    timeStopCounter++;
    // Add any additional logic you may need here
}

}
