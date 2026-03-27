using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizEntry_3 : MonoBehaviour
{
    private static Vector3 cubePosition;
    public float spawnDistance = 5f; // Increased to 5 units (you can adjust this in the Inspector)

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            if (PlayerPrefs.GetInt("ReturnFromQuiz", 0) == 1)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 spawnPos = new Vector3(
                        PlayerPrefs.GetFloat("SpawnX", 0),
                        PlayerPrefs.GetFloat("SpawnY", 0),
                        PlayerPrefs.GetFloat("SpawnZ", 0)
                    );
                    player.transform.position = spawnPos;
                }
                PlayerPrefs.SetInt("ReturnFromQuiz", 0);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Using spawnDistance for further distance
            Vector3 spawnPosition = transform.position + (-transform.forward * spawnDistance);

            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
            PlayerPrefs.SetFloat("SpawnZ", spawnPosition.z);
            PlayerPrefs.SetInt("ReturnFromQuiz", 1);

            SceneManager.LoadScene("Screen4");
        }
    }
}