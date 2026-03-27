using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftInteraction : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.E) && canOpen)
        {
            m_open = !m_open;
            if (m_animator)
            {
                m_animator.SetBool("Activate", m_open);

            }
            Debug.Log("Lift Opened");
        }

    }

    public bool Open()
    {
        return m_open;
    }
}
