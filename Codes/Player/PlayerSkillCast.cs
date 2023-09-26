using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillCast : MonoBehaviour
{
    [Header("CoolDownIcons")] public Image[] CooldownIcon;
    [Header("OutOfManaIcons")] public Image[] OutOfManaIcon;
    [Header("CoolDownTimes")] public float[] CoolDownTimes;
    [Header("ManaAmounts")] public float[] ManaAmounts;
    [Header("ManaSettings")] public float TotalMana=100f,ManaRegenSpeed=2f;

    [Header("Required Level")] public int[] Skill;
    //SkillQ = 2, SkillW = 3, SkillE = 4, SkillR = 6;
    //private List<int> levelList = new List<int>();
    public Image ManaBar;

    private bool faded = false;
    private int[] fadeImages = new int[] { 0, 0, 0, 0 };
    private Animator anim;
    private bool canAttack = true;
    private PlayerOnClick playerOnClick;
    
    public  PlayerSkillsEffects playerSkillsEffects;
    private LevelManager levelManager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerOnClick = GetComponent<PlayerOnClick>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        playerSkillsEffects = GetComponent<PlayerSkillsEffects>();
    }

    
    void Update()
    {
        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        if (anim.IsInTransition(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            TurnThePlayer();
        }
        if (TotalMana < 100f)
        {
            TotalMana += ManaRegenSpeed*Time.deltaTime;
            ManaBar.fillAmount = TotalMana/100f;
        }
        CheckLevel();
        CheckMana();
        CheckToFade();
        CheckInput();
    }

    void CheckInput()
    {
        if (anim.GetInteger("Attack") == 0)
        {
            playerOnClick.FinishedMovement = false;
            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                playerOnClick.FinishedMovement = true;
            }
        }
        //Skill
        if (Input.GetKeyDown(KeyCode.Q) && TotalMana>=ManaAmounts[0]&&levelManager.GetLevel>=Skill[0])
        {
            if (playerOnClick.FinishedMovement && fadeImages[0] != 1 && canAttack)
            {
                TurnThePlayer();
                playerOnClick.TargetPosition = transform.position;
                anim.SetInteger("Attack",1);
                TotalMana -= ManaAmounts[0];
                fadeImages[0] = 1;
                playerSkillsEffects.FlameSkill_Q();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) && TotalMana>=ManaAmounts[1]&&levelManager.GetLevel>=Skill[1])
        {
            if (playerOnClick.FinishedMovement && fadeImages[1] != 1 && canAttack)
            {
                
                TotalMana -= ManaAmounts[1];
                fadeImages[1] = 1;
                anim.SetInteger("Attack",2);
                playerSkillsEffects.HealSkill_W();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && TotalMana>=ManaAmounts[2]&&levelManager.GetLevel>=Skill[2])
        {
            if (playerOnClick.FinishedMovement && fadeImages[2] != 1 && canAttack)
            {
                TurnThePlayer();
                playerOnClick.TargetPosition = transform.position;
                anim.SetInteger("Attack",3);
                TotalMana -= ManaAmounts[2];
                fadeImages[2] = 1;
                playerSkillsEffects.IceArrowSkill_E();

            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && TotalMana>=ManaAmounts[3]&&levelManager.GetLevel>=Skill[3])
        {
            if (playerOnClick.FinishedMovement && fadeImages[3] != 1 && canAttack)
            {
                TotalMana -= ManaAmounts[3];
                fadeImages[3] = 1;
                anim.SetInteger("Attack",4);
                playerSkillsEffects.PlasmaSkill_R();
            }
        }
        else
        {
            anim.SetInteger("Attack",0);
        }
    }
    
    void CheckToFade()
    {
        for (int i = 0; i < CooldownIcon.Length; i++)
        {
            if (fadeImages[i] == 1)
            {
                if (FadeAndWait(CooldownIcon[i], CoolDownTimes[i]))
                {
                    fadeImages[i] = 0;
                }
            }
        }
    }
    void CheckMana()
    {
        for (int i = 0; i < OutOfManaIcon.Length; i++)
        {
            if (levelManager.GetLevel>=Skill[i])
            {
                if (TotalMana<ManaAmounts[i])
                {
                    OutOfManaIcon[i].gameObject.SetActive(true);
                }
                else
                {
                    OutOfManaIcon[i].gameObject.SetActive(false);
                }  
            }
              
        }
    }

    void CheckLevel()
    {
        for (int i = 0; i < OutOfManaIcon.Length; i++)
        {
            if (levelManager.GetLevel <Skill[i])
            {
                OutOfManaIcon[i].gameObject.SetActive(true);
            }
        }
    }
    bool FadeAndWait(Image fadeImage, float fadeTime)
    {
        faded = false;
        if (fadeImage == null)
        {
            return faded;
        }

        if (!fadeImage.gameObject.activeInHierarchy)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.fillAmount = 1f;
        }
        
        fadeImage.fillAmount -= fadeTime * Time.deltaTime;
        if (fadeImage.fillAmount <= 0f)
        {
            fadeImage.gameObject.SetActive(false);
            faded = true;
        }

        return faded;
    }

    void TurnThePlayer()
    {
        Vector3 targetPos=Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(targetPos - transform.position), playerOnClick.turnSpeed* 100 * Time.deltaTime);
    }
}
