using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearPlatform : MonoBehaviour
{
    public float Duration;

    float timer;
    bool isCollision;

    [SerializeField] Transform playerCheckPoint;
    [SerializeField] Vector2 playerCheckSize;

    [SerializeField] LayerMask PlayerLayer;

    GameObject child;
    private void Start()
    {
        isCollision = false;
        timer = Duration;
        child = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if(Physics2D.OverlapBox(playerCheckPoint.position,playerCheckSize,0,PlayerLayer))
            isCollision = true;
        if (isCollision)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                child.gameObject.SetActive(false);
                isCollision = false;
                timer = Duration;
                //isNoneActive = true;
            }
            
        }
        if(!child.gameObject.activeSelf)
        {
            timer -= Time.deltaTime/2;
            if (timer <= 0)
            {
                child.gameObject.SetActive(true);
                timer = Duration;
                //isNoneActive = false;
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(playerCheckPoint.position,playerCheckSize);
    }
}
