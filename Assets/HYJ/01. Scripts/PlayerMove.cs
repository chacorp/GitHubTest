﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VR_MODE
public class PlayerMove : MonoBehaviour
{
    [SerializeField] OVRInput.Axis2D iThumbstick_L;
    [SerializeField] float moveSpeed = 2.5f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        DirectMove();
    }

    void DirectMove()
    {

        Vector2 contPos = OVRInput.Get(iThumbstick_L);

        float h = contPos.x;
        float v = contPos.y;

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);

        transform.position += dir * moveSpeed * Time.deltaTime;

    }
}
#endif
