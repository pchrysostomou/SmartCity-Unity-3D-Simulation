using System;
using UnityEngine;
using UnityEngine.UI;





public class GrabbableObject : MonoBehaviour


{
    private Rigidbody rb;
    private bool isGrabbed = false;

    // added by me
    public Text itemDescription; // Description of the item
    public String itemName; // Name of the item
    public Text nameTagText; // Reference to the UI Text element


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Hide the UI Text element
        if (nameTagText != null)
        {
            nameTagText.gameObject.SetActive(false);
        }
    }

    public void Grab()
    {
        isGrabbed = true;
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    public void Release()
    {
        isGrabbed = false;
        rb.useGravity = true;
        rb.freezeRotation = false;
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }

    // added by me
    public string GetItemDescription()
    {
        return itemDescription.text;
    }

    public string GetItemName()
    {
        return itemName;
    }

    // detect mouse hover
    void OnMouseEnter()
    {
        // Show the UI Text element
        if (nameTagText != null)
        {
            nameTagText.text = itemName; // Set the name of the item
            nameTagText.gameObject.SetActive(true); // Show the UI Text element
        }
        else
        {
            Debug.Log("nameTagText is null, please assign");
        }
    }

    // detect mouse exit
    void OnMouseExit()
    {
        // Hide the UI Text element
        if (nameTagText != null)
        {
            nameTagText.gameObject.SetActive(false);
        }
    }
}
