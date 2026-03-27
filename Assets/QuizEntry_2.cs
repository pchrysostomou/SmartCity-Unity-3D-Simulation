using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizEntry_2 : MonoBehaviour
{
    private bool isPlayerNearby = false;

    // New fixed respawn position
    private Vector3 fixedRespawnPosition = new Vector3(54.3f, 19.71f, 80.67f);

    void Start()
    {
        // Check if the player is returning from the quiz
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            if (PlayerPrefs.GetInt("ReturnFromQuiz", 0) == 1)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    // Set the player's position to the fixed respawn position
                    player.transform.position = fixedRespawnPosition;

                    // Reset Rigidbody to avoid falling issues
                    Rigidbody rb = player.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    Debug.Log($"Player respawned at: {fixedRespawnPosition}");
                }

                // Reset the return flag
                PlayerPrefs.SetInt("ReturnFromQuiz", 0); // Reset the return flag to 0
                PlayerPrefs.Save();
            }
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player pressed 'E' to start the quiz.");

            // Save the return flag
            PlayerPrefs.SetInt("ReturnFromQuiz", 1);  // Set the return flag to 1
            PlayerPrefs.Save();

            // Load the quiz scene
            SceneManager.LoadScene("Screen3");
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
