using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    private Transform playerTarget;
    private BossState bossStateChecker;
    private NavMeshAgent agent;
    private Animator anim;
    
    private bool finishedAttack = true;

    public float turnSpeed;
    public float attackRate;
    public float currentAttackTime;
    private SphereCollider targetCollider;
    public static bool bossDeath = false;

    private List<GameObject> allWayPointsList = new List<GameObject>();
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform firePosition;
    private void Awake()
    {
        bossDeath = false;
        targetCollider = GetComponentInChildren<SphereCollider>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        bossStateChecker = GetComponent<BossState>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        allWayPointsList.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
    }

    private void Update()
    {
        if (!anim.IsInTransition(0)&& anim.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
        {
            if (!AudioManager.instance.SFX[10].isPlaying)
            {
                AudioManager.instance.PlaySFX(10);
            }
        }
        if (finishedAttack)
        {
            GetControl();    
        }
        else
        {
            anim.SetInteger("Attack",0);
            if (!anim.IsInTransition(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                finishedAttack = true;
            }
        }
    }

    void GetControl()
    {
        if (bossStateChecker.state==BossState.State.DEATH)
        {
            agent.isStopped=true;
            anim.ResetTrigger("Hit");
            anim.ResetTrigger("Shoot");
            anim.SetBool("WakeUp",false);
            anim.SetBool("Run",false);
            anim.SetBool("Walk",false);
            anim.SetBool("Death",true);
            targetCollider.enabled = false;
            AudioManager.instance.PlaySFX(7);
            Destroy(gameObject);
            //bossDeath = true;
        }
        else
        {
            if (bossStateChecker.state==BossState.State.CHASE)
            {
                agent.isStopped = false;
                anim.SetBool("WakeUp",true);
                anim.SetBool("Run",true);
                anim.SetBool("Walk",false);
                agent.speed = 3f;
                agent.SetDestination(playerTarget.position);
            }
            else if (bossStateChecker.state==BossState.State.PATROL)
            {
                agent.isStopped = false;
                anim.ResetTrigger("Shoot");
                anim.SetBool("WakeUp",true);
                anim.SetBool("Run",false);
                anim.SetBool("Walk",true);
                if (agent.remainingDistance<4f||!agent.hasPath)
                {
                    agent.speed = 2f;
                    PickRandomLocation();            
                }
            }
            else if (bossStateChecker.state==BossState.State.SHOOT)
            {
                anim.SetBool("WakeUp",true);
                anim.SetBool("Run",false);
                anim.SetBool("Walk",false);
                LookPlayer();
                if (currentAttackTime>=attackRate)
                {
                    anim.SetTrigger("Shoot");
                    AudioManager.instance.PlaySFX(0);
                    Instantiate(fireBall, firePosition.position, Quaternion.identity);
                    currentAttackTime = 0;
                    finishedAttack = false;
                }
                else
                {
                    currentAttackTime += Time.deltaTime;
                    
                }
            }
            else if (bossStateChecker.state==BossState.State.ATTACK)
            {
                anim.SetBool("WakeUp",true);
                anim.SetBool("Run",false);
                anim.SetBool("Walk",false);
                LookPlayer();
                if (currentAttackTime>=attackRate)
                {
                    int index = Random.Range(1, 3);
                    anim.SetInteger("Attack",index);
                    AudioManager.instance.PlaySFX(9);
                    currentAttackTime = 0;
                    finishedAttack = false;
                }
                else
                {
                    currentAttackTime += Time.deltaTime;
                    anim.SetInteger("Attack",0);
                }
            }
            else
            {
                anim.SetBool("WakeUp",false);
                anim.SetBool("Walk",false);
                anim.SetBool("Run",false);
                agent.isStopped = true;
            }
            {
                
            }
        }
    }

    void LookPlayer()
    {
        Vector3 targetPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
        transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetPosition-transform.position),turnSpeed*Time.deltaTime);
        
    }
    void PickRandomLocation()
    {
        GameObject pos = GetRandomPoint();
        agent.SetDestination(pos.transform.position);
    }

    private GameObject GetRandomPoint()
    {
        int index = Random.Range(0, allWayPointsList.Count);
        return allWayPointsList[index];
    }
}
