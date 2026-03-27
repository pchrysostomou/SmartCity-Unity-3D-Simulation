using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public Animator elevatorAnimator; // Reference to the elevator's Animator
    public GameObject buttonIndicator; // Visual indicator for interaction
    public float buttonCooldown = 1f; // Cooldown time for button press

    private bool isActivated = false; // Tracks whether the elevator is active
    private bool canPressButton = false; // Tracks if the player is near the button
    private float nextPressTime = 0f; // Tracks the next allowed button press time

    void Start()
    {
        // Ensure necessary components are assigned
        if (elevatorAnimator == null)
            Debug.LogError("Elevator Animator is not assigned in the inspector!");
        if (buttonIndicator != null)
            buttonIndicator.SetActive(false); // Hide indicator by default
    }

    void Update()
    {
        // Check if the player presses the button and the cooldown has passed
        if (Input.GetKeyDown(KeyCode.E) && canPressButton && Time.time >= nextPressTime)
        {
            isActivated = !isActivated; // Toggle elevator state
            elevatorAnimator.SetBool("Activate", isActivated); // Update Animator
            nextPressTime = Time.time + buttonCooldown; // Set cooldown timer

            Debug.Log(isActivated ? "Elevator activated (moving up)" : "Elevator deactivated (moving down)");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the player
        if (other.CompareTag("Player"))
        {
            canPressButton = true;
            if (buttonIndicator != null)
                buttonIndicator.SetActive(true); // Show interaction indicator
            Debug.Log("Player can press the elevator button.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving is the player
        if (other.CompareTag("Player"))
        {
            canPressButton = false;
            if (buttonIndicator != null)
                buttonIndicator.SetActive(false); // Hide interaction indicator
            Debug.Log("Player left the button area.");
        }
    }
}
