using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardAttack : MonoBehaviour
{
    [SerializeField] private float swingDuration = 0.5f;
    [SerializeField] private float swingSpeed = 0.22f;

    [SerializeField] private float swingTimer = 0.0f;
    [SerializeField] private bool isSwinging = false;
    [SerializeField] private Vector3 startRot;

    void Start()
    {
        startRot = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;
            
        }

        if(isSwinging)
        {
            SwingClipboard();
        }

    }

    Vector3 targetAngle = new Vector3(13.5f, 205.0f, 10.0f);
    float motionRatio = 0;

    void SwingClipboard()
    {
        swingTimer += Time.deltaTime;
        

        if (swingTimer < swingDuration * 0.5f)
        {
            motionRatio += Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startRot, targetAngle, motionRatio * 2 * (1.0f/ swingDuration));
        }

        else if (swingTimer > swingDuration * 0.5f && swingTimer < swingDuration)
        {
            motionRatio -= Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startRot, targetAngle, motionRatio * 2 * (1.0f / swingDuration));
        }

        else if (swingTimer >= swingDuration)
        {
            swingTimer = 0.0f;
            motionRatio = 0;
            isSwinging = false;
        }
    }
}
