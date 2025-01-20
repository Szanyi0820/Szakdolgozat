using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The player to follow

    [Header("Camera Settings")]
    public float distance = 4.0f;      // Distance behind the player
    public float height = 2.0f;        // Height above the player
    public float rotationSpeed = 150f; // Speed of camera rotation
    public float smoothness = 5.0f;    // Smoothness of position changes

    private float currentX = 0f;       // Current X rotation (for orbit)
    private float currentY = 15f;      // Current Y rotation (slightly downward angle)
    private float minY = -35f;         // Lower clamp for vertical angle
    private float maxY = 70f;          // Upper clamp for vertical angle

    void Update()
    {
        // Input for rotating the camera (right joystick or mouse)
        currentX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Clamp the vertical rotation angle
        currentY = Mathf.Clamp(currentY, minY, maxY);
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 offset = new Vector3(0, height, -distance);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothness * Time.deltaTime);

        // Always look at the target (player)
        transform.LookAt(target.position + Vector3.up * height * 0.5f); // Adjust focus slightly upward
    }
}
