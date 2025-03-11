using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance; // Singleton instance
    private Checkpoint currentCheckpoint; // The last activated checkpoint

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Checkpoint newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint Set at: " + newCheckpoint.transform.position);
    }

    public Vector3 GetRespawnPosition()
    {
        if (currentCheckpoint != null)
        {
            return currentCheckpoint.GetRespawnPosition();
        }
        return Vector3.zero; // Default spawn point if no checkpoint is set
    }
}

