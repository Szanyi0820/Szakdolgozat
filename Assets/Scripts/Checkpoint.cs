using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint; // The location where the player will respawn
    private bool isActive = false;
    public GameObject restTextUI;// If this checkpoint is the current respawn point
    private Enemy[] allEnemies;

    private void Start()
    {
        if (restTextUI != null)
            restTextUI.SetActive(false);

        //allEnemies = FindObjectsOfType<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            
            Debug.Log("Checkpoint Activated!");
            if (restTextUI != null)
                restTextUI.SetActive(true);
                allEnemies = FindObjectsOfType<Enemy>();
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
            if (IsEnemyAggroed())
            {
                Debug.Log("Cannot rest! Enemies are nearby and aggressive.");
                return; // Prevent resting if an enemy is aggroed
            }

            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                LevelUpMenu levelUpMenu = FindObjectOfType<LevelUpMenu>();
            if (levelUpMenu != null)
            {
                levelUpMenu.OpenMenu();
            }
                CheckpointManager.instance.SetCheckpoint(this);
                player.currentHealth = player.maxHealth;
                player.healthSlider.value=player.maxHealth;
                Debug.Log("Player rested at checkpoint. Health restored.");
                EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
                spawner.RespawnEnemies();
            }
        }
    }
    private bool IsEnemyAggroed()
    {
        foreach (Enemy enemy in allEnemies)
        {
            AIController enemyAI = enemy.GetComponent<AIController>();

            if (enemyAI != null && enemyAI.IsAggroed())
            {
                return true; // An enemy is aggroed, prevent resting
            }
        }
        return false; // No enemies are aggroed, player can rest
    }

}
