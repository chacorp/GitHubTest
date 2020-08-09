﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardAttack : MonoBehaviour
{
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private float swingTimer = 0.0f;
    [SerializeField] private bool isSwinging = false;
    [SerializeField] private Vector3 startRot;
    [SerializeField] Vector3 targetAngle = new Vector3(50.543f, -21.848f, -44.478f);
    [SerializeField] private Vector3 startPos;
    [SerializeField] Vector3 targetPos1 = new Vector3(0.063f, 0.001f, 0.13f);
    [SerializeField] Vector3 targetPos2 = new Vector3(0.063f, -0.5f, 0.13f);
    [SerializeField] AudioClip swingSound;
    [SerializeField] AudioSource clipboardAudio;
    float motionRatio = 0;
    float motionRatio2 = 0;
    float motionRatio3 = 0;


    void Start()
    {
        startRot = transform.localEulerAngles;
        startPos = transform.transform.localPosition;
        clipboardAudio = GetComponentInChildren<AudioSource>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;
            clipboardAudio.PlayOneShot(swingSound);
        }

        if (isSwinging)
        {
            SwingClipboard();
        }
    }

    //   Vector3 targetAngle = new Vector3(13.5f, 205.0f, 10.0f);


    void SwingClipboard()
    {
        swingTimer += Time.deltaTime;


        if (swingTimer < swingDuration * 0.33f)
        {
            motionRatio += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos1, motionRatio * 3 * (1.0f / swingDuration));

        }

        else if (swingTimer > swingDuration * 0.33f && swingTimer < swingDuration * 0.66f)
        {
            motionRatio2 += Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startRot, targetAngle, motionRatio2 * 3 * (1.0f / swingDuration));
            transform.localPosition = Vector3.Lerp(targetPos1, targetPos2, motionRatio2 * 3 * (1.0f / swingDuration));

        }

        else if (swingTimer > swingDuration * 0.66f && swingTimer < swingDuration)
        {
            motionRatio -= Time.deltaTime;
            motionRatio3 += Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startRot, targetAngle, motionRatio * 3 * (1.0f / swingDuration));
            transform.localPosition = Vector3.Lerp(targetPos2, startPos, motionRatio3 * 3 * (1.0f / swingDuration));
        }

        else if (swingTimer >= swingDuration)
        {
            swingTimer = 0.0f;
            motionRatio = 0;
            motionRatio2 = 0;
            isSwinging = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collide");
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            Debug.Log($"{other.gameObject.name} is attacked!");
            enemy.OnDamageProcess();
        }
    }

}
