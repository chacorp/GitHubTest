using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    [SerializeField] float StartTimeBtwSpawns;
    private float timeBtwSpawns;
    private float currentTime;

    private Enemy enemy;
    EchoPool echoManager;

    void OnEnable()
    {
        enemy = GetComponent<Enemy>();
        echoManager = GameObject.Find("ObjectManager").GetComponent<EchoPool>();
    }


    //private void ReturnEchoPool(GameObject obj, List<GameObject> objPool)
    //{
    //    obj.SetActive(false);
    //    obj.transform.position = Vector3.zero;
    //    objPool.Add(obj);
    //}

    void Update()
    {
        // 거미가 Damage 상태가 되면 isBleeding이 참이 되게 되어 본 업데이트 함수가 실행된다.
        if (enemy.isBleeding)
        {
            currentTime += Time.deltaTime;

            if (currentTime <= 1.2f)
            {
                if (timeBtwSpawns <= 0)
                {
                    // spawn echo gameobject
                    GameObject trailEffect = echoManager.GetEchoPool();
                    trailEffect.transform.position = transform.position;

                    StartCoroutine(echoManager.ReturnEchoPool(trailEffect, 0.8f));

                    timeBtwSpawns = StartTimeBtwSpawns;
                }
                else timeBtwSpawns -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                enemy.isBleeding = false;
                return;
            }
        }
    }
}
