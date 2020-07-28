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
            SwingClipboard();
        }


    }

    void SwingClipboard()
    {
        swingTimer += Time.deltaTime;

        if (swingTimer < (swingDuration / 2))
        {
            transform.localEulerAngles = Vector3.Lerp(startRot, new Vector3(260.0f, 2.0f, 165.0f), swingSpeed);
        }

        if (swingTimer > (swingDuration / 2))
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, startRot, swingSpeed);
        }

        if (swingTimer > swingDuration)
        {
            swingTimer = 0.0f;
            isSwinging = false;
        }
    }
}
