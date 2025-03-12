using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreenUI; // UI panel containing "You Died" text and respawn button
    public Button respawnButton; // Button to trigger respawn
    private PlayerHealth playerHealth;

    void Start()
    {
        deathScreenUI.SetActive(false); // Hide death screen at start
        respawnButton.onClick.AddListener(RespawnPlayer); // Attach button event
        playerHealth = FindObjectOfType<PlayerHealth>(); // Get the PlayerHealth script
    }

    public void ShowDeathScreen()
    {
        deathScreenUI.SetActive(true);
        //Time.timeScale = 0f; // Pause the game
    }

    public void RespawnPlayer()
    {
        deathScreenUI.SetActive(false);
        Time.timeScale = 1f; // Resume game
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
                spawner.RespawnEnemies();

        if (playerHealth != null)
        {
            playerHealth.SendMessage("Respawn", SendMessageOptions.DontRequireReceiver); // Call Respawn() function in PlayerHealth
        }
        else
        {
            Debug.LogError("PlayerHealth script not found! Cannot respawn.");
        }
    }
}
