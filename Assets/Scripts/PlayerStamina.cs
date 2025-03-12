using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For stamina UI (Optional)

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;  // Maximum stamina
    public float stamina;            // Current stamina
    public float staminaRegenRate = 10f;  // How fast stamina regenerates per second
    public float regenDelay = 0.5f;  // Delay before stamina starts regenerating
    private float lastActionTime;    // Tracks when the player last used stamina

    [Header("Action Costs")]
    public float sprintCostPerSecond = 15f;
    public float jumpCost = 20f;
    public float attackCost = 10f;
    public float rollCost = 25f;

    [Header("Stamina UI (Optional)")]
    public Slider staminaBar;
    public RectTransform staminaSliderTransform;  // UI bar to show stamina (optional)

    private bool isSprinting = false;
    private bool isRegenerating = false;

    private void Start()
    {
        stamina = maxStamina;
        staminaBar.value=stamina; // Start with full stamina
    }

    private void Update()
    {
        HandleSprinting();
        HandleStaminaRegen();
        UpdateStaminaUI(); // Optional UI update
    }

    // Handles sprinting and stamina drain
    private void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            isSprinting = true;
            DrainStamina(sprintCostPerSecond * Time.deltaTime);
        }
        else
        {
            isSprinting = false;
        }
    }

    // Jump function that uses stamina
    public bool TryJump()
    {
        if (stamina >= jumpCost)
        {
            DrainStamina(jumpCost);
            return true; // Allow jump
        }
        return false; // Not enough stamina
    }

    // Attack function that uses stamina
    public bool TryAttack()
    {
        if (stamina >= attackCost)
        {
            DrainStamina(attackCost);
            return true; // Allow attack
        }
        return false; // Not enough stamina
    }

    // Roll function that uses stamina
    /*public bool TryRoll()
    {
        if (stamina >= rollCost)
        {
            DrainStamina(rollCost);
            StartCoroutine(Roll()); // Call roll coroutine
            return true; // Allow roll
        }
        return false; // Not enough stamina
    }*/

    // Drain stamina and reset regen timer
    private void DrainStamina(float amount)
    {
        stamina -= amount;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        lastActionTime = Time.time;
    }

    // Handles stamina regeneration
    private void HandleStaminaRegen()
    {
        if (Time.time - lastActionTime >= regenDelay && stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }
    }

    // Optional UI Update
    public void UpdateStaminaUI()
    {
        if (staminaBar != null) // Ensure the staminaBar is assigned
        {
            staminaBar.value = maxStamina; // Set the slider value
        }
    }
    public void UpdateStaminaBarSize()
    {
        if (staminaSliderTransform != null)
    {
        float expansionAmount = 20f; // Adjust expansion amount as needed
        Vector2 newSize = staminaSliderTransform.sizeDelta;
        newSize.x += expansionAmount; // Only grow width, not height
        staminaSliderTransform.sizeDelta = newSize;
    }
    }

    // Example Roll Coroutine (Updated to Use Stamina System)
    /* private IEnumerator Roll()
     {
         isRolling = true;
         canRoll = false;

         FaceCameraDirection();

         Vector3 rollDirection = Vector3.zero;
         string rollAnimation = GetRollDirectionAndAnimation(out rollDirection);

         if (!string.IsNullOrEmpty(rollAnimation))
         {
             anim.SetTrigger(rollAnimation);
         }

         float rollDuration = 0.5f;
         float targetDistance = rollSpeed * rollDuration;

         float elapsedTime = 0f;
         while (elapsedTime < rollDuration)
         {
             float moveAmount = Mathf.Lerp(0, targetDistance, elapsedTime / rollDuration);
             transform.Translate(rollDirection * moveAmount * Time.deltaTime, Space.World);

             elapsedTime += Time.deltaTime;
             yield return null;
         }

         isRolling = false;
         yield return new WaitForSeconds(rollCooldown);
         canRoll = true;
}*/
}
