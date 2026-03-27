using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoorInteraction : MonoBehaviour
{
    Animator m_animator;
    bool m_open;
    bool canOpen = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
        }
    }

    void Update()
    {
        // Only allow the door to open if the player is near and has the key
        if (Input.GetKeyDown(KeyCode.F) && canOpen && KeyPickup.hasMainKey)
        {
            m_open = !m_open;
            if (m_animator)
            {
                m_animator.SetBool("Open", m_open);
            }
            Debug.Log("Door Opened");
        }
        else if (Input.GetKeyDown(KeyCode.F) && canOpen && !KeyPickup.hasMainKey)
        {
            Debug.Log("You need the Main Key to open this door.");
        }
    }

    public bool Open()
    {
        return m_open;
    }
}
