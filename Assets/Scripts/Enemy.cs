using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    public int maxHealth = 100;
    private int currentHealth;
    private HealthBar healthBar;
    public float stunDuration = 60f;  // Time the enemy is unable to attack
    public bool isStunned = false;
    private AIController controller;
    private void Start()
    {
        // Get the Animator component when the script starts
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>(); // Find health bar in child
        healthBar.SetMaxHealth(maxHealth);
        controller = GetComponent<AIController>();
    }
    // Start is called before the first frame update
    
    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Enemy Health: " + currentHealth);
        anim.SetTrigger("IsHit");
        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(StunEnemy());
        
    }
    private IEnumerator StunEnemy()
{
    isStunned = true;
    anim.speed = 0.5f; 
    Debug.Log("Enemy is stunned!");

    // Optional: Play a stun animation
    anim.SetTrigger("Stunned");

    yield return new WaitForSeconds(stunDuration);

    isStunned = false;
    anim.speed = 1f; 
    Debug.Log("Enemy can attack again.");
}

    void Die()
    {
        Debug.Log("Enemy Died!");
        controller.DisableHitbox();
        anim.SetTrigger("IsDead");
        this.enabled = false;
        controller.enabled=false;
        GetComponent<Collider>().enabled = false;
    }
}
