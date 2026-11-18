
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 6f;
    public float height = 2f;
    public float mouseSensitivity = 3f;

    [Header("Rotation Limits")]
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        yaw = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse Rotation
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition =
            target.position + Vector3.up * height;

        transform.position =
            targetPosition -
            rotation * Vector3.forward * distance;

        transform.LookAt(targetPosition);
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
