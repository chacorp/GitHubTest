using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardAttack : MonoBehaviour
{
    // [SerializeField] private float swingDuration = 0.5f;
    // [SerializeField] private float swingSpeed = 0.22f;
    [SerializeField] private float swingDuration = 0.2f;

    [SerializeField] private float swingTimer = 0.0f;
    [SerializeField] private bool isSwinging = false;
    [SerializeField] private Vector3 startRot;
    [SerializeField] private Vector3 startPos;

    void Start()
    {
        startRot = transform.localEulerAngles;
        startPos = transform.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;

        }

        if (isSwinging)
        {
            SwingClipboard();
        }

    }

    //   Vector3 targetAngle = new Vector3(13.5f, 205.0f, 10.0f);
    [SerializeField] Vector3 targetAngle = new Vector3(-30.5f, 210.0f, 13.0f);
    [SerializeField] Vector3 targetPos1 = new Vector3(0.224f, -0.085f, 0.4f);
    [SerializeField] Vector3 targetPos2 = new Vector3(0.216f, -0.585f, 0.515f);
    float motionRatio = 0;
    float motionRatio2 = 0;
    float motionRatio3 = 0;

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

    //private void OnTrigger(Collision col)
    //{
    //    
    //}
}
