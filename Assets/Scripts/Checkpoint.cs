using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint; // The location where the player will respawn
    private bool isActive = false;
    public GameObject restTextUI;// If this checkpoint is the current respawn point

    private void Start()
    {
        if (restTextUI != null)
            restTextUI.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            CheckpointManager.instance.SetCheckpoint(this);
            Debug.Log("Checkpoint Activated!");
            if (restTextUI != null)
                restTextUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (restTextUI != null)
                restTextUI.SetActive(false); // Hide the text
        }
    }

    public Vector3 GetRespawnPosition()
    {
        return respawnPoint.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) // Press 'E' to rest
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.currentHealth = player.maxHealth;
                player.healthSlider.value=player.maxHealth;
                Debug.Log("Player rested at checkpoint. Health restored.");
            }
        }
    }

}
