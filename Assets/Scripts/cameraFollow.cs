using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform playerTarget; // The player to follow
    public Transform lockOnTarget; // The current lock-on target
    public LayerMask enemyLayer; // Layer for enemies

    [Header("Camera Settings")]
    public float distance = 4.0f;      // Distance behind the player
    public float height = 2.0f;        // Height above the player
    public float rotationSpeed = 150f; // Speed of camera rotation
    public float smoothness = 5.0f;    // Smoothness of position changes

    [Header("Lock-On Settings")]
    public float lockOnRadius = 15f; // Radius for detecting enemies
    public KeyCode lockOnKey = KeyCode.Q; // Key to toggle lock-on
    public KeyCode switchTargetKey = KeyCode.Tab; // Key to switch targets

    private float currentX = 0f;       // Current X rotation (for orbit)
    private float currentY = 15f;      // Current Y rotation (slightly downward angle)
    private float minY = -35f;         // Lower clamp for vertical angle
    private float maxY = 70f;          // Upper clamp for vertical angle
    private bool isLockedOn = false;   // Whether the camera is locked onto a target
    private List<Transform> targets = new List<Transform>(); // List of potential targets

    void Update()
    {
        // Input for rotating the camera (when not locked on)
        if (!isLockedOn)
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            currentY = Mathf.Clamp(currentY, minY, maxY);
        }

        // Lock-on toggle
        if (Input.GetKeyDown(lockOnKey))
        {
            if (isLockedOn)
            {
                UnlockTarget();
            }
            else
            {
                FindTargets();
                LockOnToNearestTarget();
            }
        }

        // Switch lock-on target
        if (isLockedOn && Input.GetKeyDown(switchTargetKey))
        {
            SwitchTarget();
        }
    }

    void LateUpdate()
    {
        if (playerTarget == null)
            return;

        if (isLockedOn && lockOnTarget != null)
        {
            // Lock-on behavior: Camera focuses on the locked-on target
            Vector3 targetPosition = lockOnTarget.position + Vector3.up * -2f; // Adjust for height
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, smoothness * Time.deltaTime);

            // Keep the camera a fixed distance from the player
            Vector3 offset = new Vector3(0, height, -distance);
            transform.position = playerTarget.position + transform.rotation * offset;
        }
        else
        {
            // Default behavior: Camera follows the player
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 offset = new Vector3(0, height, -distance);
            Vector3 desiredPosition = playerTarget.position + rotation * offset;

            // Smoothly move the camera to the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothness * Time.deltaTime);

            // Always look at the player
            transform.LookAt(playerTarget.position + Vector3.up * height * 0.5f);
        }
    }

    private void FindTargets()
    {
        targets.Clear();
        Collider[] colliders = Physics.OverlapSphere(playerTarget.position, lockOnRadius, enemyLayer);

        foreach (Collider collider in colliders)
        {
            targets.Add(collider.transform);
        }
    }

    private void LockOnToNearestTarget()
    {
        if (targets.Count == 0)
        {
            Debug.Log("No targets found");
            return;
        }

        Transform nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(playerTarget.position, target.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null)
        {
            lockOnTarget = nearestTarget;
            isLockedOn = true;
            Debug.Log("Locked onto " + nearestTarget.name);
        }
    }

    private void UnlockTarget()
    {
        lockOnTarget = null;
        isLockedOn = false;
        Debug.Log("Unlocked target");
    }

    private void SwitchTarget()
    {
        if (targets.Count <= 1) return;

        int currentIndex = targets.IndexOf(lockOnTarget);
        int nextIndex = (currentIndex + 1) % targets.Count;

        lockOnTarget = targets[nextIndex];
        Debug.Log("Switched target to " + lockOnTarget.name);
    }

    private void OnDrawGizmosSelected()
    {
        if (playerTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTarget.position, lockOnRadius);
        }
    }
}
