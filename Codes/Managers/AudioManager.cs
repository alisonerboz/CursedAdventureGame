using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource MenuMusic, GameMusic;
    public AudioSource[] SFX;
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
    }

    public void PlayMenuMusic()
    {
        if (GameMusic.isPlaying)
        {
            GameMusic.Stop();
        }
        MenuMusic.Play();
    }
    public void PlayGameMusic()
    {
        if (MenuMusic.isPlaying)
        {
            MenuMusic.Stop();
        }
        GameMusic.Play();
    }

    public void PlaySFX(int SXF_To_Play)
    {
        SFX[SXF_To_Play].Stop();
        SFX[SXF_To_Play].Play();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
