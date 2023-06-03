using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(WaitTime());
        if (collision.gameObject.CompareTag("Player2"))
        {
            CompleteLevel();
        }
        else if (collision.gameObject.CompareTag("Player1"))
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);
    }
}
