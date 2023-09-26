using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    public float fallowHeight = 5f;
    public float fallowDistance = 5f;
    public float fallowHeightSpeed = 0.9f;

    private Transform Player;

    private float targetHeight;
    private float currentHeight;
    private float currentRotation;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        targetHeight = Player.position.y + fallowHeight;
        currentRotation = transform.eulerAngles.y;
        currentHeight = Mathf.Lerp(transform.position.y, targetHeight, fallowHeightSpeed * Time.deltaTime);
        Quaternion euler=Quaternion.Euler(0f,currentRotation,0f);
        Vector3 targetPosition = Player.position - (euler * Vector3.forward) * fallowDistance;
        targetPosition.y = currentHeight;
        transform.position = targetPosition;
        transform.LookAt(Player);
    }
}
