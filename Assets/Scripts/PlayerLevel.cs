using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevel : MonoBehaviour
{
    public int currentSouls = 0; // Player's current souls
    public int level = 1;
    public int strength = 10;
    public int dexterity = 10;
    public int endurance = 10;
    public int vitality = 10;
    public int soulCost = 200; // Initial cost for first level up

    public TMP_Text soulsText; // UI Text to display souls

    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;
    private Fighter playerFighter;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerStamina = GetComponent<PlayerStamina>();
        playerFighter = GetComponent<Fighter>();
        playerInventory=GetComponent<PlayerInventory>();
        UpdateUI();
    }

    public void AddSouls(int amount)
    {
        currentSouls += amount;
        UpdateUI();
    }

    public void LevelUp(string stat)
    {
        if (currentSouls >= soulCost)
        {
            currentSouls -= soulCost;
            level++;
            soulCost = Mathf.RoundToInt(soulCost * 1.5f); // Increase cost exponentially

            switch (stat)
            {
                case "Strength":
                    strength += 1;
                    playerInventory.strength+=5; // Increase base damage
                    break;
                case "Dexterity":
                    dexterity += 1;
                    playerInventory.dexterity+=5;
                    // In the future, boost dex-based weapons here
                    break;
                case "Endurance":
                endurance += 1;
                playerStamina.maxStamina += 10;  // Increase max stamina
                playerStamina.stamina += 10;     // Increase current stamina
                /*playerStamina.staminaBar.maxValue = playerStamina.maxStamina; // Update slider max value
                playerStamina.staminaBar.value = playerStamina.maxStamina;*/
                playerStamina.UpdateStaminaUI(); 
                playerStamina.UpdateStaminaBarSize();
                 // Update UI to reflect new stamina value
                  // Adjust the stamina bar to reflect max and current stamina
                break;
                case "Vitality":
                    vitality += 1;
                    playerHealth.maxHealth += 20; // Increase health
                    playerHealth.currentHealth += 20;
                    playerHealth.healthSlider.maxValue = playerHealth.maxHealth; // Update slider max value
                    playerHealth.healthSlider.value = playerHealth.currentHealth;
                    playerHealth.UpdateHealthBar();
                    playerHealth.UpdateHealthBarSize();
                    break;
            }

            UpdateUI();
            Debug.Log("Leveled up " + stat + "!");
        }
        else
        {
            Debug.Log("Not enough souls!");
        }
    }

    private void UpdateUI()
    {
        if (soulsText != null)
            soulsText.text = "Souls: " + currentSouls;
    }
}

