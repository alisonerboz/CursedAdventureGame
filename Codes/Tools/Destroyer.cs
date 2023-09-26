using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float timer;
    void Start()
    {
        Destroy(this.gameObject,timer);      
    }
}
