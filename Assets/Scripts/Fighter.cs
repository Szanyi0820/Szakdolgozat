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
    double maxComboDelay = 1.4;
    
 
    private void Start()
    {
        anim = GetComponent<Animator>();
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
            noOfClicks = 0;
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
}