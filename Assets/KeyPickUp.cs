using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool hasMainKey = false; // Tracks if the player has the key
    private bool canPickup = false; // Tracks if the player is in the trigger zone

    void Update()
    {
        // Check if the player is in the trigger zone and presses "F"
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            hasMainKey = true; // Player picks up the key
            Debug.Log("Main Key Picked Up!");
            Destroy(gameObject); // Remove the key from the scene
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true; // Player is near the key
            Debug.Log("Press F to pick up the key.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false; // Player is no longer near the key
        }
    }
}
