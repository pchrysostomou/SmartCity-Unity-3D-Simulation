using UnityEngine;

public class CarSystem : MonoBehaviour
{
    [Header("Required References")]
    public GameObject playerObject;
    public Transform driverSeat;

    [Header("Movement Settings")]
    public float carSpeed = 20f;
    public float turnSpeed = 120f;

    [Header("Interaction")]
    public float interactionDistance = 5f;
    public KeyCode enterExitKey = KeyCode.E;

    // References
    private CharacterController playerController;
    private PlayerMovement playerMovement;  // Your FPS movement script
    private bool isInCar = false;

    void Start()
    {
        // Get components
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<CharacterController>();
            playerMovement = playerObject.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // Handle entering/exiting
        if (Input.GetKeyDown(enterExitKey))
        {
            if (!isInCar)
            {
                float distance = Vector3.Distance(transform.position, playerObject.transform.position);
                if (distance <= interactionDistance)
                {
                    EnterCar();
                }
            }
            else
            {
                ExitCar();
            }
        }

        // Handle car movement
        if (isInCar)
        {
            HandleCarMovement();
        }
    }

    void EnterCar()
    {
        isInCar = true;

        // Disable player movement
        if (playerController != null) playerController.enabled = false;
        if (playerMovement != null) playerMovement.enabled = false;

        // Position player
        playerObject.transform.SetParent(driverSeat);
        playerObject.transform.localPosition = Vector3.zero;
        playerObject.transform.localRotation = Quaternion.identity;

        Debug.Log("Entered car - Player movement disabled");
    }

    void ExitCar()
    {
        isInCar = false;

        // Enable player movement
        if (playerController != null) playerController.enabled = true;
        if (playerMovement != null) playerMovement.enabled = true;

        // Reset player
        playerObject.transform.SetParent(null);
        Vector3 exitPosition = transform.position + (transform.right * 2f);
        playerObject.transform.position = exitPosition;

        Debug.Log("Exited car - Player movement enabled");
    }

    void HandleCarMovement()
    {
        // Get input
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Move car forward/backward
        transform.Translate(Vector3.forward * vertical * carSpeed * Time.deltaTime);

        // Turn car left/right (only when moving)
        if (Mathf.Abs(vertical) > 0.1f)
        {
            transform.Rotate(Vector3.up * horizontal * turnSpeed * Time.deltaTime);
        }

        // Move the player with the car (ensure they stay in driver seat)
        if (playerObject != null)
        {
            playerObject.transform.position = driverSeat.position;
            playerObject.transform.rotation = driverSeat.rotation;
        }
    }

    void OnDrawGizmos()
    {
        // Visualize interaction range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}