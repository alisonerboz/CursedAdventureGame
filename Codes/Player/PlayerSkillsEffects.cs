using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillsEffects : MonoBehaviour
{
    [Header("Skill Effects")] public GameObject[] Effect;
    [Header("Skill Transforms")] public Transform[] EffectLocation;

    //private Transform EffectRotation;

    public void FlameSkill_Q()
    {
        print("Q BASTI");
        Instantiate(Effect[0], EffectLocation[0].position,transform.rotation);
    }
    public void HealSkill_W()
    {
        GameObject HealClone = Instantiate(Effect[1], transform.position, transform.rotation);
        HealClone.transform.SetParent(transform);
    }
    public void IceArrowSkill_E()
    {
        Instantiate(Effect[2], EffectLocation[1].position, transform.rotation);
    }
    public void PlasmaSkill_R()
    {
        Instantiate(Effect[3], transform.position, transform.rotation);
    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FlameSkill_Q();
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            HealSkill_W();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            IceArrowSkill_E();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            PlasmaSkill_R();
        }
        */
    }
}
