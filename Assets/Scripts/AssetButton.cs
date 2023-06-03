using UnityEngine;
using UnityEngine.UI;

public class AssetButton : MonoBehaviour
{
    public string templateID; // Unique template ID for the asset
    public Button button; // Reference to the UI button

    private void Start()
    {
        button.onClick.AddListener(MintAsset);
    }

    public void MintAsset()
    {
        ReneverseMintManager.Instance.Mint(templateID);
    }
}
