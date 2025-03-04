using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public Transform cameraTransform; // Camera to focus on the target
    public LayerMask enemyLayer; // Layer for enemies
    public float lockOnRadius = 15f; // Radius for detecting enemies
    public KeyCode lockOnKey = KeyCode.Q; // Key to toggle lock-on
    public KeyCode switchTargetKey = KeyCode.Tab; // Key to switch targets

    private Transform currentTarget; // Current locked-on target
    private List<Transform> targets = new List<Transform>(); // List of potential targets
    private bool isLockedOn = false; // Lock-on state

    void Update()
    {
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

        if (isLockedOn && Input.GetKeyDown(switchTargetKey))
        {
            SwitchTarget();
        }

        if (isLockedOn && currentTarget != null)
        {
            KeepFocusOnTarget();
        }
    }

    // Find all enemies within the lock-on radius
    private void FindTargets()
    {
        targets.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, lockOnRadius, enemyLayer);

        foreach (Collider collider in colliders)
        {
            targets.Add(collider.transform);
        }
    }

    // Lock onto the nearest target
    private void LockOnToNearestTarget()
    {
        if (targets.Count == 0) 
        {
        Debug.Log("Sikertelen");
        return;}

        Transform nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null)
        {
            currentTarget = nearestTarget;
            isLockedOn = true;
            Debug.Log("Sikerult");
        }
    }

    // Unlock the current target
    private void UnlockTarget()
    {
        currentTarget = null;
        isLockedOn = false;
    }

    // Switch to the next target in the list
    private void SwitchTarget()
    {
        if (targets.Count <= 1) return;

        int currentIndex = targets.IndexOf(currentTarget);
        int nextIndex = (currentIndex + 1) % targets.Count;

        currentTarget = targets[nextIndex];
    }

    // Rotate player and camera to face the target
    private void KeepFocusOnTarget()
    {
        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;

        // Rotate player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Rotate camera
        if (cameraTransform != null)
        {
            cameraTransform.LookAt(currentTarget.position + Vector3.up * 1.5f); // Adjust height to focus on the target's upper body
        }
    }

    // Optional: Debugging the lock-on radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lockOnRadius);
    }
}
