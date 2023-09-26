using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyWayPoint : Witch
{
    public float saldiriSuresi = 2f;
    private float zamanSayaci = 0f;
    private Witch witch;
    [Header("WayPoints")] public Transform[] walkPoint;
    [Header("MovementSettings")] public float turnSpeed = 5f, patrolTime = 10f, walkDistance = 8f;
    [Header("AttackSettings")] public float attackDistance = 1.4f, attackRate = 1f;

    private Transform playerTarget;
    private Animator anim;

    private NavMeshAgent agent;

    private float currentAttackTime;

    private Vector3 nextDestination;
    private int index;
    
    //Health
    private EnemyHealth enemyHealth;
    private void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        index = UnityEngine.Random.Range(0, walkPoint.Length);
        if (walkPoint.Length>0)
        {
            InvokeRepeating("Patrol",UnityEngine.Random.Range(0,patrolTime),patrolTime);
        }
    }

    void Start()
    {
        agent.avoidancePriority = UnityEngine.Random.Range(1, 51);
    }

    
    void Update()
    {
        if (enemyHealth.currentHealth>0)
        {
            MoveAndAttack();
        }
        else
        {
            anim.ResetTrigger("Hit");
            anim.SetBool("Death",true);
            AudioManager.instance.PlaySFX(5);
            agent.enabled = false;
            if (!anim.IsInTransition(0)
                &&anim.GetCurrentAnimatorStateInfo(0).IsName("Death")
                &&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.95f)
            {
                Destroy(this.gameObject,5f);   
            }
        }
    }

    void MoveAndAttack()
    {
        float distance = Vector3.Distance(transform.position, playerTarget.position);
        if (distance > walkDistance)
        {
            if (agent.remainingDistance >= agent.stoppingDistance)
            {
                agent.isStopped = false;
                agent.speed = 2f;
                anim.SetBool("Walk", true);
                nextDestination = walkPoint[index].position;
                agent.SetDestination(nextDestination);
            }

            else
            {
                agent.isStopped = true;
                agent.speed = 0;
                anim.SetBool("Walk", false);

                nextDestination = walkPoint[index].position;
                agent.SetDestination(nextDestination);
            }
        }
        else
        {
            if (distance>attackDistance+0.15f && playerTarget.GetComponent<PlayerHealth>().currentHealth>0)
            {
                if (!anim.IsInTransition(0)&& !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    anim.ResetTrigger("Attack");
                    agent.isStopped = false;
                    agent.speed = 3f;
                    anim.SetBool("Walk",true);
                    agent.SetDestination(playerTarget.position);
                    if (this.gameObject.name=="Idle_Witch")
                    {
                        buyu.SetActive(true);
                        zamanSayaci += Time.deltaTime;

                        if (zamanSayaci >= saldiriSuresi)
                        {
                            Instantiate(iskelet, iskeletKonum1.position, Quaternion.identity);
                            zamanSayaci = 0f;
                        }
                    }
                }
            }
            else if (distance<=attackDistance && playerTarget.GetComponent<PlayerHealth>().currentHealth>0)
            {
                agent.isStopped = true;
                anim.SetBool("Walk",false);
                agent.speed = 0;
                Vector3 targetPosition =
                    new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(targetPosition - transform.position),
                    turnSpeed * Time.deltaTime);
                if (currentAttackTime>=attackRate)
                {
                    anim.SetTrigger("Attack");
                    currentAttackTime = 0f;
                    //AudioManager.instance.PlaySFX(2);
                }
                else
                {
                    currentAttackTime += Time.deltaTime;
                }
            }
        }
    }

    void Patrol()
    {
        index = index == walkPoint.Length - 1 ? 0 : index + 1;
    }

}
