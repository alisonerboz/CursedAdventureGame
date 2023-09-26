using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public enum State
    {
        NONE,
        SLEEP,
        PATROL,
        CHASE,
        ATTACK,
        SHOOT,
        DEATH
    }

    private Transform playerTarget;
    private State bosState = State.SLEEP;
    public State state
    {
        get { return bosState;  }
    }

    private float distanceToTarget;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        bosState = State.SLEEP;
    }

    private void Update()
    {
        SetState();
    }

    void SetState()
    {
        distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);
        if (bosState==State.SLEEP)
        {
            int enemyCount = FindObjectsOfType<EnemyWayPoint>().Length;
            if (enemyHealth.currentHealth<enemyHealth.maxHealth)
            {
                bosState = State.NONE;
            }
            else if(distanceToTarget<=7f)
            {
                bosState = State.NONE;
            }
            else if (enemyCount<=0)
            {
                bosState = State.NONE;
            }
            else
            {
                bosState = State.SLEEP;
            }
        }
        else if (bosState!=State.DEATH||bosState!=State.SLEEP)
        {
            if (distanceToTarget>4f&&distanceToTarget<=8f)
            {
                bosState = State.CHASE;
                
            }
            else if (distanceToTarget>5f&& distanceToTarget<=8f)
            {
                bosState = State.SHOOT;
            }
            else if (distanceToTarget>8f)
            {
                bosState = State.PATROL;
            }
            else if (distanceToTarget<=4f)
            {
                bosState = State.ATTACK;
            }
            else
            {
                bosState = State.NONE;
            }
        }

        if (enemyHealth.currentHealth<=0)
        {
            bosState = State.DEATH;
        }
    }
}
