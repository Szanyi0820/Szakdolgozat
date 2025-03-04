using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to UI Slider
    public Vector3 offset = new Vector3(0, 2f, 0); // Adjust position above enemy

    void Update()
    {
        // Make the health bar always face the camera
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}

