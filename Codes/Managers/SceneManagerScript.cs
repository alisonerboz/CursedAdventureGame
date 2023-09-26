using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    private BossState bossState;
    public GameObject bossHealthBar;
    private void Awake()
    {
        bossState = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossState>();
    }

    private void Start()
    {
        AudioManager.instance.PlayGameMusic();
    }

    void Update()
    {
        if (bossState.state==BossState.State.SLEEP || bossState.state==BossState.State.DEATH)
        {
            if (bossHealthBar!=null)
            {
                bossHealthBar.SetActive(false);
            }
        }
        else
        {
            if (bossHealthBar!=null)
            {
                bossHealthBar.SetActive(true);
            }
        }

        if (Boss.bossDeath==true)
        {
            Invoke("RestartScene",5f);
        }
        else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().currentHealth<=0)
        {
            Invoke("RestartScene",5f);           
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
