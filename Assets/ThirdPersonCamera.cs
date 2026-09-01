using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [Header("Position Offset")]
    public Vector3 offset = new Vector3(1.5f, 1.2f, -3.5f);

    [Header("Follow")]
    public float followSpeed = 8f;

    [Header("Mouse")]
    public float mouseSensitivity = 3f;
    public float rotationSmoothness = 10f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch = 5f;

    private float currentYaw;
    private float currentPitch;

    [Header("Aim Zoom")]
    public float aimDistance = 2f;
    public float zoomSmoothness = 8f;
    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = offset;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentYaw = yaw;
        currentPitch = pitch;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Target rotation
        yaw += mouseX * mouseSensitivity;
        pitch -= mouseY * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Smooth rotation
        currentYaw = Mathf.Lerp(
            currentYaw,
            yaw,
            rotationSmoothness * Time.deltaTime
        );

        currentPitch = Mathf.Lerp(
            currentPitch,
            pitch,
            rotationSmoothness * Time.deltaTime
        );

        Quaternion rotation = Quaternion.Euler(
            currentPitch,
            currentYaw,
            0f
        );

        bool isAiming = Input.GetMouseButton(1);

        Vector3 targetOffset = isAiming
            ? new Vector3(offset.x, offset.y, -aimDistance)
            : offset;

        currentOffset = Vector3.Lerp(
            currentOffset,
            targetOffset,
            zoomSmoothness * Time.deltaTime
        );

        // Camera position
        Vector3 desiredPosition =
            target.position + rotation * currentOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        transform.rotation = rotation;
    }
}