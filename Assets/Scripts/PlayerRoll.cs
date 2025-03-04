using System.Collections;
using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
    public Animator anim;                    // Reference to the Animator
    public float rollDistance = 5f;           // How far the player will roll
    public float rollSpeed = 10f;             // Speed of the roll
    public float rollCooldown = 1f;           // Time after rolling before the player can roll again
    private bool isRolling = false;           // Whether the player is currently rolling
    private bool canRoll = true;              // Whether the player is allowed to roll
    private Collider playerCollider;          // Reference to the player's collider
    public bool isInvincible = false;  
    private Vector3 rollDirection;
    public Camera playerCamera;         // Whether the player is invincible during the roll

    void Start()
    {
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();  // Assuming the player has a collider
    }

    void Update()
    {
        // If the player presses the roll button and is allowed to roll, trigger the roll
        if (Input.GetKeyDown(KeyCode.Space) && canRoll && !isRolling)
        {
            StartCoroutine(Roll());
        }
    }
    public void SetInvincible(){
        if(isInvincible==true){
            isInvincible=false;
        }
        else isInvincible=true;
    }

    private IEnumerator Roll()
    {
        // Set rolling state to true
        isRolling = true;
        canRoll = false;

        // Make the player invincible during the roll
        //DetermineRollDirection();
        //TriggerRollAnimation();

        // Trigger the roll animation
        anim.SetTrigger("RollForward");

        

        // Perform the roll movement (move the player forward)
          // You can also use a direction vector if needed
          
        float startTime = Time.time;
        Vector3 rollDirection = transform.forward;
        
        // Move the player during the roll
        while (Time.time - startTime < 0.5f)  // Adjust this duration based on your roll animation length
        {
            transform.Translate(rollDirection * rollSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        // Set rolling state to false
        isRolling = false;

        // Wait for the cooldown before allowing the player to roll again
        yield return new WaitForSeconds(rollCooldown);

        // Allow the player to roll again
        canRoll = true;

        // Make the player not invincible anymore
        
    }
    /*private void DetermineRollDirection()
    {
        // Get the character's forward and right directions in world space
        Vector3 characterForward = transform.forward;
        Vector3 characterRight = transform.right;

        // Normalize the vectors to avoid scaling issues
        characterForward.y = 0;
        characterRight.y = 0;
        characterForward.Normalize();
        characterRight.Normalize();

        // Get the player's input direction (WASD or arrow keys)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down

        // Calculate the roll direction based on character's facing direction
        if (vertical > 0) // Forward (W key)
        {
            rollDirection = characterForward; // Roll forward in the direction the character is facing
        }
        else if (vertical < 0) // Backward (S key)
        {
            rollDirection = -characterForward; // Roll forward in the direction the character is facing, even if pressing S (as in walking or rolling toward the camera)
        }
        else if (horizontal > 0) // Right (D key)
        {
            rollDirection = characterRight; // Roll to the right relative to the character
        }
        else if (horizontal < 0) // Left (A key)
        {
            rollDirection = -characterRight; // Roll to the left relative to the character
        }
    }

    private void TriggerRollAnimation()
    {
        // Trigger roll animation based on the direction of the roll
        if (rollDirection == transform.forward)
        {
            anim.SetTrigger("RollForward");
        }
        else if (rollDirection == -transform.forward)
        {
            anim.SetTrigger("RollBackward");
        }
        else if (rollDirection == transform.right)
        {
            anim.SetTrigger("RollRight");
        }
        else if (rollDirection == -transform.right)
        {
            anim.SetTrigger("RollLeft");
        }
    }*/

    // Call this function to detect if the player is hit (you would typically call this in a OnCollisionEnter or OnTriggerEnter)
    
}
