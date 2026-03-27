using EscalatorPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    private Vector3 platformVelocity = Vector3.zero;
    private Transform currentPlatform;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Stair>() != null && Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
        {
            currentPlatform = hit.transform;
            Rigidbody stairRb = hit.gameObject.GetComponent<Rigidbody>();
            if (stairRb != null)
            {
                platformVelocity = stairRb.velocity;
            }
        }
        else
        {
            currentPlatform = null;
            platformVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        Vector3 totalMovement = (move * speed + platformVelocity) * Time.deltaTime;
        controller.Move(totalMovement);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
