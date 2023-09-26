using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private int currentExp;
    private int level;
    private int expToNextLevel;

    public Image ExpBar;
    public Text LevelText;
    public GameObject LevelUpVFX;
    private Transform player;
    public int GetLevel
    {
        get { return level+1; } set{}
    }

    public static LevelManager instance;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        level = 0;
        currentExp = 0;
        expToNextLevel = 100;
        ExpBar.fillAmount = 0f;
        UpdateLevelText();
        player = GameObject.Find("Player").gameObject.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddExp(25);
            print(level);
        }
        //print(currentExp);
    }

    public void AddExp(int amount)
    {
        ExpBar.fillAmount += (float)currentExp / expToNextLevel;
        currentExp += amount;
        if (currentExp>=expToNextLevel)
        {
            level++;
            GameObject LevelUpVFXClone = Instantiate(LevelUpVFX, player.position, Quaternion.identity);
            LevelUpVFXClone.transform.SetParent(player);
            UpdateLevelText();
            currentExp -= expToNextLevel;
            ExpBar.fillAmount = 0f;
        }
    }

    void UpdateLevelText()
    {
        LevelText.text = GetLevel.ToString();
    }
    private void OnEnable()
    {
        EnemyHealth.onDeath += AddExp;
    }

    private void OnDisable()
    {
        EnemyHealth.onDeath -= AddExp;
    }
}
