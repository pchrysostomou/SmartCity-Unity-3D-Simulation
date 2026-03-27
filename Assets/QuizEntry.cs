using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizEntry : MonoBehaviour
{
    private bool isPlayerNearby = false;

    // Predefined respawn position
    private Vector3 respawnPosition = new Vector3(0f, 21.7f, 67f);

    void Start()
    {
        // Check if returning from the quiz scene
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            if (PlayerPrefs.GetInt("ReturnFromQuiz", 0) == 1) // Check the return flag value for the quiz scene
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    // Set the player to the predefined respawn position
                    player.transform.position = respawnPosition;

                    // // Reset Rigidbody to prevent falling
                    // Rigidbody rb = player.GetComponent<Rigidbody>();
                    // if (rb != null)
                    // {
                    //     rb.velocity = Vector3.zero;
                    //     rb.angularVelocity = Vector3.zero;
                    // }

                    Debug.Log($"Player respawned at: {respawnPosition}");
                }

                // Reset the return flag
                PlayerPrefs.SetInt("ReturnFromQuiz", 0);   // Reset the return flag to 0
                PlayerPrefs.Save();
            }
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Check if the player is nearby and presses 'E'
        {
            Debug.Log("Player pressed 'E' to start the quiz.");

            // Save the return flag
            PlayerPrefs.SetInt("ReturnFromQuiz", 1); // Set the return flag to 1
            PlayerPrefs.Save();

            // Load the quiz scene
            SceneManager.LoadScene("Screen2");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Press 'E' to start the quiz.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
