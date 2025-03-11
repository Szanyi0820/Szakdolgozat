using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public Camera playerCamera;
    public float rollStaminaCost = 25f;
    private PlayerController playerController;

void Start()
    {
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
        playerController = GetComponent<PlayerController>();// Assuming the player has a collider
    }

    void Update()
    {
        // If the player presses the roll button and is allowed to roll, trigger the roll
        if (Input.GetKeyDown(KeyCode.Space) && canRoll && !isRolling && playerController.currentStamina >= rollStaminaCost)
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
        playerController.DrainStamina(rollStaminaCost);

        
        // Trigger the roll animation
        //anim.SetTrigger("RollForward");



        // Perform the roll movement (move the player forward)
        // You can also use a direction vector if needed

        float startTime = Time.time;
        Vector3 rollDirection = DetermineRollDirection();
        
        TriggerRollAnimation();
        


        // Move the player during the roll
        while (Time.time - startTime < 0.5f)  // Adjust this duration based on your roll animation length
        {
            
            transform.Translate(rollDirection * rollSpeed * Time.deltaTime, Space.World);
            FaceCameraDirection();
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
    private void FaceCameraDirection()
    {
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f; // Ignore vertical rotation
        cameraForward.Normalize();

        // Rotate the character to match camera direction
        transform.forward = cameraForward;
    }
    /*private Vector3 GetRollDirection()
    {
        // Here, we detect the player's movement input (e.g., WASD or arrow keys)
        // You can use Input.GetAxis for movement or detect the direction based on input keys

        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float vertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

        // Get the direction vector based on input
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // If there is no input, default to the forward direction
        if (inputDirection.magnitude == 0)
        {
            inputDirection = transform.forward;
        }

        return inputDirection;
    }*/
    private Vector3 DetermineRollDirection()
    {
        // Get the character's forward and right directions in world space
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float vertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

        // Get the custom camera's forward and right directions
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        // Flatten the directions on the X-Z plane (ignore the Y axis)
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalize the directions
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the roll direction based on the input and camera orientation
        Vector3 inputDirection = cameraForward * vertical + cameraRight * horizontal;

        // Normalize the input direction to prevent faster diagonal movement
        if (inputDirection.magnitude > 0f)
        {
            inputDirection.Normalize();
        }
        else
        {
            inputDirection = cameraForward;  // Default to forward if no input
        }

        return inputDirection;
    }

    private void TriggerRollAnimation()
    {
        // Trigger roll animation based on the direction of the roll
        //if (rollDirection == playerCamera.transform.forward)
        if(Input.GetKey(KeyCode.W))
        {
            anim.SetTrigger("RollForward");
        }
        //else if (rollDirection == -playerCamera.transform.forward)
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetTrigger("RollBackward");
        }
        else if (Input.GetKey(KeyCode.D))
        //else if (rollDirection == playerCamera.transform.right)
        {
            anim.SetTrigger("RollRight");
        }
        //else if (rollDirection == -playerCamera.transform.right)
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetTrigger("RollLeft");
        }
    }

    // Call this function to detect if the player is hit (you would typically call this in a OnCollisionEnter or OnTriggerEnter)

}
