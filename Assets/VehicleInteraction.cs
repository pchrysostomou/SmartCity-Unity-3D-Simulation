using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform driverSeat;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject carCamera;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private KeyCode interactionKey = KeyCode.F;

    private bool isInCar = false;
    private CharacterController characterController;
    private Vector3 originalPlayerPosition;
    private Rigidbody carRigidbody;

    private void Start()
    {
        characterController = playerObject.GetComponent<CharacterController>();
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if (!isInCar)
            {
                // Check if player is close enough to enter
                float distanceToVehicle = Vector3.Distance(playerObject.transform.position, driverSeat.position);
                if (distanceToVehicle <= interactionDistance)
                {
                    EnterVehicle();
                }
            }
            else
            {
                ExitVehicle();
            }
        }
    }

    private void EnterVehicle()
    {
        isInCar = true;

        // Store original position for exiting
        originalPlayerPosition = playerObject.transform.position;

        // Disable player controller and hide player model
        if (characterController != null)
            characterController.enabled = false;
        playerObject.SetActive(false);

        // Switch cameras
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);
        if (carCamera != null)
            carCamera.SetActive(true);

        // Enable car controls
        if (GetComponent<CarController>() != null)
            GetComponent<CarController>().enabled = true;
    }

    private void ExitVehicle()
    {
        isInCar = false;

        // Find safe exit position
        Vector3 exitPosition = transform.position + transform.right * 2f;
        RaycastHit hit;
        if (Physics.Raycast(exitPosition + Vector3.up * 2f, Vector3.down, out hit, 4f))
        {
            exitPosition = hit.point;
        }

        // Re-enable and reposition player
        playerObject.SetActive(true);
        playerObject.transform.position = exitPosition;
        if (characterController != null)
            characterController.enabled = true;

        // Switch cameras back
        if (carCamera != null)
            carCamera.SetActive(false);
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);

        // Disable car controls
        if (GetComponent<CarController>() != null)
            GetComponent<CarController>().enabled = false;
    }
}