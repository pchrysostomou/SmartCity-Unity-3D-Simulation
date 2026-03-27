using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToWorld : MonoBehaviour
{
    void Start()
    {
        Button exitButton = GetComponent<Button>();
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitToWorld);
        }
    }

    public void ExitToWorld()
    {
        SceneManager.LoadScene("SampleScene");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}