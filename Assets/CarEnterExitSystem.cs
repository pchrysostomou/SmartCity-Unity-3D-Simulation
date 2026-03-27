using UnityEngine;

public class CarEnterExitSystem : MonoBehaviour
{
    public MonoBehaviour CarController;
    public Transform Car;
    public Transform Player;

    [Header("Cameras")]
    public GameObject PlayerCam;
    public GameObject CarCam;

    private bool Candrive;

    void Start()
    {
        if (CarController == null || Car == null || Player == null || PlayerCam == null || CarCam == null)
        {
            Debug.LogError("Missing references in CarEnterExitSystem!");
            return;
        }

        // Initialize the game state
        CarController.enabled = false;  // Car controls start disabled
        Player.gameObject.SetActive(true);  // Player starts visible
        PlayerCam.SetActive(true);  // Player camera starts active
        CarCam.SetActive(false);  // Car camera starts inactive

        Debug.Log("CarEnterExitSystem initialized");
    }

    // Rest of the code remains the same...
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"F key pressed. Candrive: {Candrive}");
            if (Candrive)
            {
                EnterCar();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("G key pressed - Exiting car");
            ExitCar();
        }
    }

    private void EnterCar()
    {
        CarController.enabled = true;
        Player.SetParent(Car);
        Player.gameObject.SetActive(false);
        PlayerCam.SetActive(false);
        CarCam.SetActive(true);
        Debug.Log("Entered car successfully");
    }

    private void ExitCar()
    {
        CarController.enabled = false;
        Player.SetParent(null);
        Player.gameObject.SetActive(true);
        PlayerCam.SetActive(true);
        CarCam.SetActive(false);
        Debug.Log("Exited car successfully");
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log($"Trigger entered by: {col.gameObject.name} with tag: {col.gameObject.tag}");
        if (col.CompareTag("Player"))
        {
            Candrive = true;
            Debug.Log("Player can now drive");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Candrive = false;
            Debug.Log("Player left drive zone");
        }
    }
}
