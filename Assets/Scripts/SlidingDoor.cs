using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    Animator m_animator; // Animator component of the garage door
    bool m_open; // Tracks whether the garage door is open

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_open = true; // Set the door to open state
            if (m_animator)
            {
                m_animator.SetBool("activate", true); // Open the door
            }
            Debug.Log("sliding Door Opened");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_open = false; // Set the door to closed state
            if (m_animator)
            {
                m_animator.SetBool("activate", false); // Close the door
            }
            Debug.Log("sliding Door Closed");
        }
    }

    public bool Open()
    {
        return m_open;
    }
}