using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition_2 : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Screen3"; // Target scene name
    public float spawnDistance = 5f; // Distance to save respawn position
    private bool isPlayerNearby = false; // Tracks if the player is near the NPC

    private void Update()
    {
        // Only allow interaction when the player is nearby and presses "E"
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Saving position and loading {targetSceneName}...");

            // Save player's position relative to the NPC
            Vector3 spawnPosition = transform.position + (-transform.forward * spawnDistance);
            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
            PlayerPrefs.SetFloat("SpawnZ", spawnPosition.z);
            PlayerPrefs.SetInt("ReturnFromQuiz", 1);
            PlayerPrefs.Save();

            // Load the target scene
            SceneManager.LoadScene(targetSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player is near the NPC. Press 'E' to start the quiz.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the NPC interaction area.");
        }
    }
}