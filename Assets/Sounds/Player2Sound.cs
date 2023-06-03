using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Sound : MonoBehaviour
{
    [SerializeField] AudioSource P2Run1;
    [SerializeField] AudioSource P2Run2;
    [SerializeField] AudioSource P2Attack1;
    [SerializeField] AudioSource P2Attack2;
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
        P2Run1.Play();
    }
    void Run2Sound()
    {
        P2Run2.Play();
    }
    void Attack1Sound()
    {
        P2Attack1.Play();
    }
    void Attack2Sound()
    {
        P2Attack2.Play();
    }
}
