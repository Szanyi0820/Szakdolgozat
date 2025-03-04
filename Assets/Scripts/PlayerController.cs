using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
         //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //key.gameObject.SetActive(false);
        //VictoryText.gameObject.SetActive(false);
        //gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        ControllPlayer();
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
        if (Input.GetKeyDown(KeyCode.F) && Time.time > canJump)
        {
            rb.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isRunning",true);
            movementSpeed=3;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        else {
            anim.SetBool("isRunning",false);
            movementSpeed=2;
        }
    }

    
}