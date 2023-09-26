using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{
    [HideInInspector] public float currentHealth;
    private Animator anim;
    public float maxHealth = 100f;

    [SerializeField] private Image EnemyHealthBar;
    [SerializeField] private SphereCollider targetCollider;

    public int ExpAmount = 10;
    public static event Action<int> onDeath; 
    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        EnemyHealthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth>0)
        {
            if (this.gameObject.tag=="Boss")
            {
                AudioManager.instance.PlaySFX(6);
            }
            else if (this.gameObject.tag=="Enemy")
            {
                AudioManager.instance.PlaySFX(3);
                maxHealth = Random.Range(80, 100);
            }
            anim.SetTrigger("Hit");
        }

        if (currentHealth<=0)
        {
            if (this.gameObject.tag=="Enemy")
            {
                onDeath(ExpAmount);
                EnemyHealthBar.gameObject.GetComponentInParent<Canvas>().gameObject.SetActive(false);
                targetCollider.gameObject.SetActive(false);
                if (this.gameObject.name=="Idle_Witch")
                {
                    
                }
            }
        }
    }
}
