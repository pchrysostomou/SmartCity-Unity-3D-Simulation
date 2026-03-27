using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition_4 : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Screen5";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Loading Screen5...");
            SceneManager.LoadScene(targetSceneName);
        }
    }
}