using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Sound : MonoBehaviour
{
    [SerializeField] AudioSource P1Run1;
    [SerializeField] AudioSource P1Run2;
    [SerializeField] AudioSource P1Attack1;
    [SerializeField] AudioSource P1Attack2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Run1Sound()
    {
        P1Run1.Play();
    }
    void Run2Sound()
    {
        P1Run2.Play();
    }
    void Attack1Sound()
    {
        P1Attack1.Play();
    }
    void Attack2Sound()
    {
        P1Attack2.Play();
    }
}
