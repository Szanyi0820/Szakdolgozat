using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;        // Maximum health
    public float currentHealth;          // Current health
    public Slider healthSlider;           // Health slider (UI element)
    public Image healthFillImage;         // Health fill image (to change color)
    public Color healthyColor = Color.green;
    public Color hurtColor = Color.red;
    private PlayerController controller;
    private Fighter fight;
    private PlayerRoll roll;
    public DeathScreenManager deathScreen;
    public RectTransform healthSliderTransform; // Reference to the UI bar RectTransform
    public float healthIncreaseAmount = 20f; // How much max HP increases per level-up
    public float healthBarIncreaseSize = 20f;

    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        controller = GetComponent<PlayerController>();
        fight = GetComponent<Fighter>();
        roll=GetComponent<PlayerRoll>();
        healthFillImage.color = healthyColor;
        UpdateHealthBar();
        healthSlider.value = currentHealth;
        deathScreen = FindObjectOfType<DeathScreenManager>();
    }

    void Update()
    {
        // This is for testing purposes, we will simulate damage

    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        if (roll.isInvincible)
        {
            // If the player is invincible, they don't take damage
            Debug.Log("Player is invincible and did not take damage!");
            return;
        }

        currentHealth -= damage;
        healthSlider.value = currentHealth;
        currentHealth=Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }  


        // Optionally, you can also play a hurt animation or sound here
        Debug.Log("Took damage: " + damage);
    }
    public void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Normalize health (0 to 1)
        }
    }
    public void UpdateHealthBarSize()
    {
        if (healthSliderTransform != null)
    {
        float expansionAmount = 20f; // Adjust expansion amount as needed
        Vector2 newSize = healthSliderTransform.sizeDelta;
        newSize.x += expansionAmount; // Only grow width, not height
        healthSliderTransform.sizeDelta = newSize;
    }
    }

    public void Heal(float healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
        healthFillImage.enabled = false;
        fight.DisableHitbox();
        controller.anim.SetTrigger("IsDead"); 
        this.enabled = false;
        controller.enabled=false;
        roll.enabled=false;
        fight.enabled=false;
        deathScreen.ShowDeathScreen();
        //Respawn();
        //GetComponent<Collider>().enabled = false;
        // Handle death (Disable player controls, play death animation, etc.)
    }
    private void Respawn()
    {
        Vector3 respawnPos = CheckpointManager.instance.GetRespawnPosition();
        if (respawnPos != Vector3.zero)
        {
            controller.anim.SetTrigger("IsRespawned");
            transform.position = respawnPos;
            currentHealth = maxHealth; // Restore health
            healthSlider.value=maxHealth;
            Debug.Log("Player Respawned at Checkpoint");
            this.enabled = true;
            controller.enabled = true;
            roll.enabled = true;
            fight.enabled = true;
            healthFillImage.enabled = true;
            isDead = false;
            
        }
        else
        {
            Debug.Log("No checkpoint set, respawning at default position.");
        }
    }
}
