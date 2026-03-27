using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition_3 : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Screen4";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Loading Screen4...");
            SceneManager.LoadScene(targetSceneName);
        }
    }
}