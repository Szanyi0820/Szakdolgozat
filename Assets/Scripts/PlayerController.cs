using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    public Transform cameraTransform; 
    private float canJump = 0f;
    //[SerializeField] GameObject key,chest_open,chest_closed;
    //[SerializeField] TextMeshProUGUI VictoryText;
    //private GameManager gameManager;
    public Animator anim;
    Rigidbody rb;
    public float maxStamina = 100;
    public float currentStamina;
    public float staminaDrainRate = 15f; // Per second
    public float staminaRegenRate = 10f; // Per second
    public float regenDelay = 1f;
    private float lastStaminaUseTime;
    public Slider staminaBar;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentStamina = maxStamina;
        if (staminaBar)
            staminaBar.maxValue = maxStamina;
            staminaBar.value = maxStamina;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //key.gameObject.SetActive(false);
        //VictoryText.gameObject.SetActive(false);
        //gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        ControllPlayer();
        RegenerateStamina();
    }
    

    IEnumerator Varakozas2(){
        //key.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
                //key.gameObject.SetActive(false);
}
private bool IsCurrentAnimation(string animationName)
{
    AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // 0 is the base layer index
    return stateInfo.IsName(animationName);
}
private bool CanMove()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool isWalking = stateInfo.IsName("Walk")||stateInfo.IsName("Idle")||stateInfo.IsName("Run");
        
        
        
        return isWalking ;
    }

    void ControllPlayer()
    {
        // Input for movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Calculate movement relative to the camera's orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Flatten the vectors to ignore vertical camera tilt
        forward.y = 0f;
        right.y = 0f;

        // Normalize the vectors
        forward.Normalize();
        right.Normalize();

        // Combine input directions with camera's orientation
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        // Movement and animation handling
        if (movement != Vector3.zero&& (anim.GetBool("hit1")==false&&anim.GetBool("hit2")==false&&anim.GetBool("hit3")==false)&&CanMove())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetBool("isWalking", true);
            
        }
        else
        {
            anim.SetBool("isWalking", false);
            movementSpeed=0;
        }
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);   
        // Move the player in world space
        

        // Jump handling
        if (Input.GetKeyDown(KeyCode.F) && Time.time > canJump && currentStamina >= 20)
        {
            rb.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
            DrainStamina(20);
        }
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            anim.SetBool("isRunning",true);
            movementSpeed=3;
            DrainStamina(staminaDrainRate * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        else {
            anim.SetBool("isRunning",false);
            movementSpeed=2;
        }
    }
    public void DrainStamina(float amount)
    {
        currentStamina = Mathf.Max(0, currentStamina - amount);
        lastStaminaUseTime = Time.time;

        if (staminaBar)
            staminaBar.value = currentStamina;
    }
    public void RegenerateStamina()
    {
        if (Time.time > lastStaminaUseTime + regenDelay && currentStamina < maxStamina)
        {
            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.deltaTime);

            if (staminaBar)
                staminaBar.value = currentStamina;
        }
    }
    


}