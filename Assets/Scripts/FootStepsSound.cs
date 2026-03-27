using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.5f; // Time between steps
    private float stepTimer;

    private void Start()
    {
        if (!characterController) characterController = GetComponent<CharacterController>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // Reset step timer if not moving
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClips.Length > 0)
        {
            // Select a random clip
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
