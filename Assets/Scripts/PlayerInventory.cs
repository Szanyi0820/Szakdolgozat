using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject sword;       // Reference to the Sword GameObject
    public GameObject katana;      // Reference to the Katana GameObject

    public float strength;         // Strength stat (affects sword damage)
    public float dexterity;        // Dexterity stat (affects katana damage)

    private GameObject activeWeapon;

    private void Start()
    {
        // Initially, Katana is off, Sword is on
        sword.SetActive(true);
        katana.SetActive(false);
        activeWeapon = sword;  // Start with Sword as the active weapon
    }

    private void Update()
    {
        // Toggle weapons when pressing the "T" key
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleWeapon();
        }

        // Calculate the active weapon's damage
        float damage = GetWeaponDamage();
        Debug.Log("Current damage: " + damage);  // You can replace this with the actual damage calculation for attacks
    }

    private void ToggleWeapon()
    {
        if (activeWeapon == sword)
        {
            sword.SetActive(false);
            katana.SetActive(true);
            activeWeapon = katana;  // Now Katana is active
        }
        else if (activeWeapon == katana)
        {
            katana.SetActive(false);
            sword.SetActive(true);
            activeWeapon = sword;  // Now Sword is active
        }
    }

    private float GetWeaponDamage()
    {
        // Damage calculation based on which weapon is active
        if (activeWeapon == katana)
        {
            return 15f + dexterity * 0.5f;  // Katana damage increases with Dexterity
        }
        else if (activeWeapon == sword)
        {
            return 15f + strength * 0.5f;  // Sword damage increases with Strength
        }

        return 0f;
    }
}
