using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;
    private Collider swordCollider;
    public Transform swordTransform;
    private Enemy enemyScript;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    Animator anim;

    Vector3 m_PlayerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;
    bool alreadyHit;


    public float attackRange = 2.5f;  // Distance at which the enemy will try to attack
    public float attackCooldown = 2f; // Time between attacks
    private float lastAttackTime = 0f;
    

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        anim = GetComponent<Animator>();
        swordCollider = swordTransform.GetComponent<BoxCollider>();
        swordCollider.enabled = false;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyScript = GetComponent<Enemy>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        
        
    }
    void Update()
    {
        EnviromentView();
        if(!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }
    private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation
    }
     
    public void EnableHitbox()
    {
        swordCollider.enabled = true;
        anim.speed = 0.5f; 
    }
    public void DisableHitbox()
    {
        anim.speed = 1f; 
        alreadyHit=false;
        swordCollider.enabled = false;
        Debug.Log("sword off");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your enemy has the tag "Enemy"
        {
            Debug.Log("Hit Enemy!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null&& alreadyHit==false)
            {
                alreadyHit=true;
                playerHealth.TakeDamage(20f); // Adjust damage value as needed
            }
            //other.GetComponent<Enemy>().TakeDamage(30); // Call damage function
        }
    }
    private void Chasing()
{
    m_PlayerNear = false;
    m_PlayerLastPosition = Vector3.zero;
    FaceTarget(m_PlayerPosition);

    float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
    if(distanceToPlayer>1){
    navMeshAgent.SetDestination(m_PlayerPosition);
    }
    if (!m_CaughtPlayer)
    {
        // Continue moving towards the player if not in attack range
        Move(speedWalk);
        
    }
    
    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
    {
        
        // Player is close enough, stop moving and attack if in range
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            m_CaughtPlayer=true;
            Stop(); // Stop and attack
            Attack();
        }
        else
        {
            // Stop moving, player is close enough
            Stop();
            anim.SetBool("IsWalking", false); // Set walking animation to false
            m_WaitTime -= Time.deltaTime;
        }
    }
    else
    {
        // Player is outside of stopping distance, continue moving towards the player
        anim.SetBool("IsWalking", true);
        Debug.Log("Called");
        m_CaughtPlayer=false; // Set walking animation to true
        navMeshAgent.isStopped = false; // Resume movement
        navMeshAgent.SetDestination(m_PlayerPosition);
    }
}

        
    
    private void Attack()
        {
            if (enemyScript != null && enemyScript.isStunned) 
    {
        Debug.Log("Enemy is stunned and can't attack!");
        return; // Stop the attack
    }
            // Trigger attack animation
            anim.SetTrigger("Attack");

            // Update the last attack time
            lastAttackTime = Time.time;
        }
    private void Patroling()
    {
        if(m_PlayerNear)
        {
            if(m_TimeToRotate<=0)
            {
                Move(speedWalk);
                LookingPlayer(m_PlayerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate-=Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear=false;
            m_PlayerLastPosition=Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if(navMeshAgent.remainingDistance<=navMeshAgent.stoppingDistance)
            {
                if(m_WaitTime<=0)
                {
                    
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime=startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime-=Time.deltaTime;
                }
            }
        }
    }
    void Move(float speed)
    {
        anim.SetBool("IsWalking",true);
        navMeshAgent.isStopped=false;
        navMeshAgent.speed=speed;
    }
    void Stop()
    {
        anim.SetBool("IsWalking",false);
        navMeshAgent.isStopped=true;
        navMeshAgent.speed=0;
    }
    public void NextPoint()
    {
        m_CurrentWaypointIndex=(m_CurrentWaypointIndex+1)%waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
    void CaughtPlayer()
    {
        m_CaughtPlayer=true;
    }
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position,player)<=0.3)
        {
            if(m_WaitTime<=0)
            {
                m_PlayerNear=false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime=startWaitTime;
                m_TimeToRotate=timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime-=Time.deltaTime;
            }
        }
    }
    void EnviromentView()
    {
        Collider[] PlayerInRange=Physics.OverlapSphere(transform.position,viewRadius,playerMask);
        for(int i=0;i<PlayerInRange.Length;i++)
        {
            Transform player= PlayerInRange[i].transform;
            Vector3 dirToPlayer=(player.position-transform.position).normalized;
            if(Vector3.Angle(transform.forward,dirToPlayer)<viewAngle/2)
            {
                float dsToPlayer=Vector3.Distance(transform.position,player.position);
                if(!Physics.Raycast(transform.position,dirToPlayer,dsToPlayer,obstacleMask))
                {
                    m_PlayerInRange=true;
                    m_IsPatrol=false;
                }
                else
                {
                    m_PlayerInRange=false;
                }
            }
            if(Vector3.Distance(transform.position,player.position)>viewRadius)
            {
                m_PlayerInRange=false;
            }
            if(m_PlayerInRange)
            {
                m_PlayerPosition=player.transform.position;
            }
        }
    }
}
