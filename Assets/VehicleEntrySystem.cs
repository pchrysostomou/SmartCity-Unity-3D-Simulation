using UnityEngine;

public class VehicleEntrySystem : MonoBehaviour
{
    [Header("Player References")]
    public GameObject playerObject;
    public Camera playerCamera;
    public Transform driverSeat;
    public Transform exitPoint;

    [Header("Vehicle Camera")]
    public Camera vehicleCamera;
    public float cameraDistance = 6f;
    public float cameraHeight = 2f;
    public float smoothSpeed = 5f;

    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;

    // References
    private CharacterController characterController;
    private MonoBehaviour playerMovementScript; // Reference to your FPS movement script
    private bool isInVehicle = false;
    private CarController carController;
    private Vector3 cameraOffset;
    private Transform originalCameraParent;
    private Vector3 originalCameraLocalPos;
    private Quaternion originalCameraLocalRot;

    void Start()
    {
        // Setup references
        if (playerObject != null)
        {
            characterController = playerObject.GetComponent<CharacterController>();
            // Find the player movement script (adjust the name if different)
            playerMovementScript = playerObject.GetComponent<MonoBehaviour>();
        }

        carController = GetComponent<CarController>();

        // Setup vehicle camera
        if (vehicleCamera != null)
        {
            vehicleCamera.gameObject.SetActive(false);
            cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
        }

        // Store original camera setup if using player camera
        if (playerCamera != null)
        {
            originalCameraParent = playerCamera.transform.parent;
            originalCameraLocalPos = playerCamera.transform.localPosition;
            originalCameraLocalRot = playerCamera.transform.localRotation;
            playerCamera.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if (!isInVehicle)
            {
                float distance = Vector3.Distance(transform.position, playerObject.transform.position);
                if (distance <= interactionDistance)
                {
                    HandleVehicleEntry();
                }
            }
            else
            {
                HandleVehicleExit();
            }
        }

        if (isInVehicle && vehicleCamera != null)
        {
            UpdateVehicleCameraPosition();
        }
    }

    void HandleVehicleEntry()
    {
        isInVehicle = true;

        // Disable character controller and player movement
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // Switch cameras
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
        }
        if (vehicleCamera != null)
        {
            vehicleCamera.gameObject.SetActive(true);
        }

        // Handle player object
        playerObject.transform.SetParent(driverSeat);
        playerObject.transform.localPosition = Vector3.zero;
        playerObject.transform.localRotation = Quaternion.identity;

        // Enable car controls
        if (carController != null)
        {
            carController.enabled = true;
        }

        // Disable any physics on the player if present
        Rigidbody playerRb = playerObject.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = true;
        }
    }

    void HandleVehicleExit()
    {
        isInVehicle = false;

        // Unparent and position the player
        playerObject.transform.SetParent(null);

        // Position player at exit point
        if (exitPoint != null)
        {
            playerObject.transform.position = exitPoint.position;
            playerObject.transform.rotation = exitPoint.rotation;
        }
        else
        {
            Vector3 exitPosition = transform.position + (transform.right * 2f);
            exitPosition.y = transform.position.y;
            playerObject.transform.position = exitPosition;
        }

        // Re-enable character controller and player movement
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Switch cameras back
        if (vehicleCamera != null)
        {
            vehicleCamera.gameObject.SetActive(false);
        }
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            // Restore original camera setup
            playerCamera.transform.SetParent(originalCameraParent);
            playerCamera.transform.localPosition = originalCameraLocalPos;
            playerCamera.transform.localRotation = originalCameraLocalRot;
        }

        // Disable car controls
        if (carController != null)
        {
            carController.enabled = false;
        }

        // Re-enable any physics on the player if present
        Rigidbody playerRb = playerObject.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
        }
    }

    void UpdateVehicleCameraPosition()
    {
        Vector3 desiredPosition = transform.TransformPoint(cameraOffset);
        vehicleCamera.transform.position = Vector3.Lerp(
            vehicleCamera.transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
        vehicleCamera.transform.LookAt(transform.position + Vector3.up);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);

        if (exitPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(exitPoint.position, 0.3f);
            Gizmos.DrawLine(transform.position, exitPoint.position);
        }
    }
}