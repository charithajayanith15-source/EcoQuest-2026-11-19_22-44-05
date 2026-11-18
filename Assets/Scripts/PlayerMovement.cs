
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Animation Settings")]
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError("Animator NOT FOUND!");
        }
        else
        {
            Debug.Log("Animator Found: " + animator.name);
            Debug.Log("Controller: " +
                animator.runtimeAnimatorController);
        }
    }

    void Update()
    {
        // WASD Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(
            horizontal,
            0f,
            vertical
        );

        move = Vector3.ClampMagnitude(move, 1f);

        // Player Movement
        controller.Move(
            move * moveSpeed * Time.deltaTime
        );

        // Character Rotation
        if (move.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(move);
        }

        // Ground Check
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );

        // Animation Control
        if (animator != null)
        {
            float speed = move.magnitude;

            animator.SetFloat("Speed", speed);

            // Debug Messages
            Debug.Log("Movement Speed: " + speed);

            Debug.Log(
                "Animator Speed: " +
                animator.GetFloat("Speed")
            );
        }
        else
        {
            Debug.LogError("Animator NOT FOUND!");
        }
    }
}

