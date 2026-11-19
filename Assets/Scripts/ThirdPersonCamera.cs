
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
    private bool cursorLocked = true;

    void Start()
    {
        yaw = transform.eulerAngles.y;
        LockCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        if (Input.GetMouseButtonDown(0) && !cursorLocked)
        {
            LockCursor();
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (cursorLocked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition =
            target.position + Vector3.up * height;

        transform.position =
            targetPosition -
            rotation * Vector3.forward * distance;

        transform.LookAt(targetPosition);
    }

    void LockCursor()
    {
        cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        cursorLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        UnlockCursor();
    }
}
