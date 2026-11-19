
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        // WASD Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection =
            new Vector3(horizontal, 0f, vertical);

        inputDirection = Vector3.ClampMagnitude(
            inputDirection, 1f
        );

        // Camera-relative movement
        Vector3 moveDirection = inputDirection;

        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection =
                forward * inputDirection.z +
                right * inputDirection.x;

            moveDirection = Vector3.ClampMagnitude(
                moveDirection, 1f
            );
        }

        // Character movement
        controller.Move(
            moveDirection * moveSpeed * Time.deltaTime
        );

        // Smooth character rotation
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Gravity
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );

        // Idle / Walking animation
        if (animator != null)
        {
            animator.SetFloat(
                "Speed",
                inputDirection.magnitude
            );
        }
    }
}
