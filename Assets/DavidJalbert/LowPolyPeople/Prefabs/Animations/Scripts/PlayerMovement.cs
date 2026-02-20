using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;

    [Header("References")]
    public Transform characterModel;
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (characterModel != null)
            animator = characterModel.GetComponent<Animator>();
    }

    void Update()
    {
        // Stop movement if dead
        if (isDead) return;

        // Safety checks
        if (controller == null || cameraTransform == null || animator == null)
            return;

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get camera directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Remove vertical influence
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate move direction
        Vector3 move = cameraForward * vertical + cameraRight * horizontal;

        // Select speed
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (move.magnitude > 0.1f)
        {
            // Move player
            controller.Move(move.normalized * speed * Time.deltaTime);

            // Rotate player toward movement
            transform.forward = move;

            // Play walk/run animation
            animator.SetFloat("Speed", speed);
        }
        else
        {
            // Idle animation
            animator.SetFloat("Speed", 0);
        }
    }

    // Called by Zombie when attacking
    public void Die()
    {
        isDead = true;

        // Stop animation movement
        if (animator != null)
        {
            animator.SetFloat("Speed", 0);
        }
    }
}
