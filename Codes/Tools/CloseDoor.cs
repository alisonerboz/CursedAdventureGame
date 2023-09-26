using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] private GameObject door1, door2;

    private void Start()
    {
        //AudioManager.instance.GameMusic.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            door1.transform.Rotate(transform.rotation.x,transform.rotation.y,90);
            door2.transform.Rotate(transform.rotation.x,transform.rotation.y,-90);
            Destroy(gameObject);
        }
        
    }
}
