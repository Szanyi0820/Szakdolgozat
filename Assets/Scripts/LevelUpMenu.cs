using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpMenu : MonoBehaviour
{
    public GameObject menuUI; // The level-up menu panel
    private bool isOpen = false;
    private PlayerLevel playerLevel;

    public TMP_Text strengthText;
    public TMP_Text dexterityText;
    public TMP_Text enduranceText;
    public TMP_Text vitalityText;
    public TMP_Text soulsText;

    void Start()
    {
        playerLevel = FindObjectOfType<PlayerLevel>();
        menuUI.SetActive(false);
        UpdateUI();
    }

    public void OpenMenu()
    {
        isOpen = true;
        menuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        UpdateUI();
    }

    public void CloseMenu()
    {
        isOpen = false;
        menuUI.SetActive(false);
        Time.timeScale = 1f; // Unpause the game
    }
    public void LevelUpStrength() 
    { 
        playerLevel.LevelUp("Strength"); 
        UpdateUI();
    }
    public void LevelUpDexterity() 
    { 
        playerLevel.LevelUp("Dexterity"); 
        UpdateUI();
    }
    public void LevelUpEndurance() 
    { 
        playerLevel.LevelUp("Endurance"); 
        UpdateUI();
    }
    public void LevelUpVitality() 
    { 
        playerLevel.LevelUp("Vitality"); 
        UpdateUI();
    }

    private void UpdateUI()
    {
        strengthText.text = "Strength: " + playerLevel.strength;
        dexterityText.text = "Dexterity: " + playerLevel.dexterity;
        enduranceText.text = "Endurance: " + playerLevel.endurance;
        vitalityText.text = "Vitality: " + playerLevel.vitality;
        soulsText.text = "Souls: " + playerLevel.currentSouls;
    }

    /*public void LevelUpStrength() { playerLevel.LevelUp("Strength"); }
    public void LevelUpDexterity() { playerLevel.LevelUp("Dexterity"); }
    public void LevelUpEndurance() { playerLevel.LevelUp("Endurance"); }
    public void LevelUpVitality() { playerLevel.LevelUp("Vitality"); }*/
}
