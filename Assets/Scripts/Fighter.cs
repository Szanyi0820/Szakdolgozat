using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Fighter : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    double maxComboDelay = 1.55;
    private Collider swordCollider;
    private PlayerController playerController;
    public int damage=30;

    private void Start()
    {
        anim = GetComponent<Animator>();
        swordCollider = GetComponentInChildren<MeshCollider>();
        playerController = GetComponent<PlayerController>();
        if (swordCollider != null)
{
    swordCollider.enabled = false;
}
else
{
    Debug.LogError("No MeshCollider found on the sword!");
}
        
    }
    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
 
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.65f&&noOfClicks<1 )
        {
            anim.SetBool("hit1", false);
            
        }
        
 
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            ResetCombo();
        }
        if (noOfClicks == 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f )
        {
            ResetCombo();
        }
 
        //cooldown time
        if (Time.time > nextFireTime)
        {
            // Check for mouse input
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
 
            }
        }
    }
    
 void ResetCombo()
    {
        noOfClicks = 0; // Reset click count
        anim.SetBool("hit1", false);
        anim.SetBool("hit2", false);
        anim.SetBool("hit3", false);
        
        
    }
    void OnClick()
    {
        //so it looks at how many clicks have been made and if one animation has finished playing starts another one.
        if (playerController.currentStamina < 15) return;
        lastClickedTime = Time.time;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        Debug.Log("" + noOfClicks);
        
        if (noOfClicks == 1)
        {
            anim.SetBool("hit1", true);
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", false);
            
            
        }
        
 
        if (noOfClicks >= 2  )
        {
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", true);
           
            
        }
        if (noOfClicks >= 3  )
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            
            anim.SetBool("hit3", true);
            if(!anim.IsInTransition(0)&&stateInfo.IsName("hit3")){
                noOfClicks=0;
            }
            
        }
    }
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")&& (anim.GetBool("hit1")==true||anim.GetBool("hit2")==true||anim.GetBool("hit3")==true)&& !hitEnemies.Contains(other.gameObject)) // Make sure your enemy has the tag "Enemy"
        {
            hitEnemies.Add(other.gameObject);
            Debug.Log("Hit Enemy!");
            other.GetComponent<Enemy>().TakeDamage(damage); // Call damage function
        }
    }
    public void EnableHitbox()
    {
        playerController.DrainStamina(15);

        swordCollider.enabled = true;
        hitEnemies.Clear(); // Reset hit list when attack starts
    }
    public void DisableHitbox()
    {
        swordCollider.enabled = false;
        Debug.Log("sword off");
    }
}