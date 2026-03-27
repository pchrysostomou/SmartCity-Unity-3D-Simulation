using UnityEngine;

public class AutoDoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Animator doorAnimator;
    public float detectionRadius = 5f;
    public float closeDelay = 1f;  // Delay before closing
    public string openParameterName = "IsOpening";

    [Header("Tags to Detect")]
    public string[] validTriggerTags = { "Player", "Car" };

    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private float closeTimer = 0f;

    void Update()
    {
        CheckForNearbyObjects();
        HandleDoorState();
    }

    void CheckForNearbyObjects()
    {
        isPlayerNearby = false;
        foreach (string tag in validTriggerTags)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in taggedObjects)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance <= detectionRadius)
                {
                    isPlayerNearby = true;
                    return;
                }
            }
        }
    }

    void HandleDoorState()
    {
        if (isPlayerNearby)
        {
            if (!isOpen)
            {
                OpenDoor();
            }
            closeTimer = 0f; // Reset timer when player is nearby
        }
        else if (isOpen)
        {
            closeTimer += Time.deltaTime;
            if (closeTimer >= closeDelay)
            {
                CloseDoor();
            }
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(openParameterName, true);
            Debug.Log("Door Opening");
        }
    }

    void CloseDoor()
    {
        isOpen = false;
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(openParameterName, false);
            Debug.Log("Door Closing");
        }
        closeTimer = 0f;
    }

    // Optional: Manual control methods that can be called from other scripts
    public void ForceOpenDoor()
    {
        OpenDoor();
    }

    public void ForceCloseDoor()
    {
        CloseDoor();
    }

    // Visualize the detection radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Optional: Event triggers that you can use to hook up sounds or other effects
    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in validTriggerTags)
        {
            if (other.CompareTag(tag))
            {
                isPlayerNearby = true;
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (string tag in validTriggerTags)
        {
            if (other.CompareTag(tag))
            {
                isPlayerNearby = false;
                break;
            }
        }
    }
}