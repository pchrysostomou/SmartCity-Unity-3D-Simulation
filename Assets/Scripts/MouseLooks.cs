using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLooks : MonoBehaviour
{
    public float mouseSensitivity = 150f; // this is to control the sensitivity of the mouse
    public Transform playerBody;
    float xRotation = 0f;  // this is to invert the mouse movement


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // this is to lock the cursor in the center of the screen
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        xRotation -= mouseY; // this is to invert the mouse movement
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // this is to prevent the player from looking up and down too much


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
