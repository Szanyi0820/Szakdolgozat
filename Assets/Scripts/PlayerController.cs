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
    Animator anim;
    Rigidbody rb;
    
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        // Move the player in world space
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        // Jump handling
        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
            rb.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
    {
//kulcs=true;
//Destroy(key);
    }
    if (other.CompareTag("Chest")){
       /* if (kulcs){
            //chest_closed.gameObject.SetActive(false);
//chest_open.gameObject.SetActive(true);
VictoryText.gameObject.SetActive(true);
//gameManager.StopAllCoroutines();*/
StopAllCoroutines();
        }
    }
    
}