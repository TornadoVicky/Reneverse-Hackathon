using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReneverseMintManager : MonoBehaviour
{
    public static ReneverseMintManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to be used for minting
    public async Task Mint(string templateID)
    {
        try
        {
            var response = await ReneverseManager.ReneAPI.Game().AssetMint(templateID);
            Debug.Log(response);
            Debug.Log("Asset Minting in progress");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
