using UnityEngine;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{
    public float grabDistance = 3f;
    public Text descriptionText; // Reference to the UI Text element
    public float rotationSpeed = 5f;
    public Transform holdPosition; // Position to hold the object
    private GrabbableObject currentlyHeldObject;
    private Camera mainCamera;
    private bool isInFocusMode = false;
    // References to components
    private CharacterController characterController;
    private MonoBehaviour[] playerScripts;
    // Store focus mode position
    private Vector3 focusModePosition;


    void Start()
    {
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        playerScripts = GetComponents<MonoBehaviour>();

        // Hide the UI Text element
        if (descriptionText != null)
        {
            descriptionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentlyHeldObject == null)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }

        if (currentlyHeldObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EnterFocusMode();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ExitFocusMode();
            }

            if (isInFocusMode)
            {
                // Keep position locked
                currentlyHeldObject.transform.position = focusModePosition;

                // Simple direct rotation
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                if (mouseX != 0)
                {
                    currentlyHeldObject.transform.Rotate(0f, mouseX * rotationSpeed, 0f, Space.World);
                }
                if (mouseY != 0)
                {
                    currentlyHeldObject.transform.Rotate(mouseY * rotationSpeed, 0f, 0f, Space.World);
                }
            }
            else
            {
                UpdateHeldObjectPosition();
            }
        }
    }

    void EnterFocusMode()
    {
        isInFocusMode = true;

        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        if (characterController != null)
        {
            characterController.enabled = false; // Disable the character controller
        }

        focusModePosition = currentlyHeldObject.transform.position;
    }

    void ExitFocusMode()
    {
        isInFocusMode = false;

        foreach (MonoBehaviour script in playerScripts)
        {
            script.enabled = true;
        }

        if (characterController != null)
        {
            characterController.enabled = true;  // Enable the character controller
        }
    }

    void TryGrabObject()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            GrabbableObject grabbable = hit.collider.GetComponent<GrabbableObject>();
            if (grabbable != null)
            {
                currentlyHeldObject = grabbable;
                grabbable.Grab();

                // LockPlayerMovement(); //

                // Update the UI Text element

                if (descriptionText != null)
                {
                    descriptionText.text = grabbable.GetItemDescription();
                    descriptionText.gameObject.SetActive(true);
                }
            }
        }
    }

    void ReleaseObject()
    {
        if (currentlyHeldObject != null)
        {
            if (isInFocusMode)
            {
                ExitFocusMode();
            }

            currentlyHeldObject.Release();
            currentlyHeldObject = null;

            // Hide the UI Text element

            if (descriptionText != null)
            {
                descriptionText.gameObject.SetActive(false);
            }
        }
    }

    void UpdateHeldObjectPosition()
    {
        currentlyHeldObject.transform.position = Vector3.Lerp(
            currentlyHeldObject.transform.position,
            holdPosition.position,
            Time.deltaTime * 10f
        );
    }
}
