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
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>(); // Find health bar in child
        healthBar.SetMaxHealth(maxHealth);
        controller = GetComponent<AIController>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Enemy Health: " + currentHealth);
        anim.SetTrigger("IsHit");
        AIController ai = GetComponent<AIController>();
    if (ai != null)
    {
        ai.Aggro(GameObject.FindGameObjectWithTag("Player").transform.position);
    }
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
   public bool IsDead()
{
    return currentHealth <= 0;
}
    void Die()
    {
        Debug.Log("Enemy Died!");
        controller.DisableHitbox();
        anim.SetTrigger("IsDead");
        this.enabled = false;
        controller.enabled=false;
        GetComponent<Collider>().enabled = false;
        PlayerLevel player = FindObjectOfType<PlayerLevel>();
    if (player != null)
    {
        player.AddSouls(100); // Give 100 souls when enemy dies
    }
    AIController ai = GetComponent<AIController>();
    if (ai != null)
    {
        ai.m_IsPatrol = true;  // Reset patrol state so it's not aggroed
    }
    }
    public void Respawn()
{
    currentHealth = maxHealth;
    healthBar.SetHealth(currentHealth);

    anim.ResetTrigger("IsDead");
    anim.Play("Idle"); // Make sure the enemy goes back to idle animation

    AIController ai = GetComponent<AIController>();
    if (ai != null)
    {
        ai.m_IsPatrol = true; // Reset to patrol mode
        ai.enabled = true;  // Re-enable AI behavior
    }

    this.enabled = true;
    GetComponent<Collider>().enabled = true;
    gameObject.SetActive(true);
}
}
