using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float baseDamage;    // Base damage of the weapon
    public string weaponName;   // Name of the weapon (Katana or Sword)

    public void Equip()
    {
        gameObject.SetActive(true);  // Turn the weapon on (make it visible, usable, etc.)
    }

    public void Unequip()
    {
        gameObject.SetActive(false); // Turn the weapon off
    }

    public float GetDamage(float strength, float dexterity)
    {
        if (weaponName == "Katana")
        {
            return baseDamage + dexterity * 0.5f;  // Dexterity increases Katana damage
        }
        else if (weaponName == "Sword")
        {
            return baseDamage + strength * 0.5f;  // Strength increases Sword damage
        }

        return baseDamage;
    }
}

