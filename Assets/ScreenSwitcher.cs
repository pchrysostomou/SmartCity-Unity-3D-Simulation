using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenSwitcher : MonoBehaviour
{
    [SerializeField] private KeyCode switchKey = KeyCode.E; // Change this to your preferred key
    [SerializeField] private string secondScreenName = "Screen2"; // Name of your second screen/scene
    [SerializeField] private Vector3 spawnPosition = Vector3.zero; // Where the player should spawn in screen 2

    void Update()
    {
        // Check if the specified key was pressed
        if (Input.GetKeyDown(switchKey))
        {
            // Save player's position if needed for returning later
            PlayerPrefs.SetFloat("LastPositionX", transform.position.x);
            PlayerPrefs.SetFloat("LastPositionY", transform.position.y);
            PlayerPrefs.SetFloat("LastPositionZ", transform.position.z);

            // Load the second screen
            SceneManager.LoadScene(secondScreenName);
        }
    }

    void Start()
    {
        // If this is screen 2, move player to spawn position
        if (SceneManager.GetActiveScene().name == secondScreenName)
        {
            transform.position = spawnPosition;
        }
    }
}