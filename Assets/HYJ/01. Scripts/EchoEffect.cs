using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    [SerializeField] float StartTimeBtwSpawns;
    private float timeBtwSpawns;
    private float currentTime;

    [SerializeField] GameObject echo;
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();

    }


    void Update()
    {
        // 거미가 Damage 상태가 되면 isBleeding이 참이 되게 되어 본 업데이트 함수가 실행된다.
        if (enemy.isBleeding)
        {
            if (timeBtwSpawns <= 0)
            {
                // spawn echo gameobject
                GameObject echos = Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(echos, 4.0f);
                timeBtwSpawns = StartTimeBtwSpawns;
            }
            else timeBtwSpawns -= Time.deltaTime;

            currentTime += Time.deltaTime;
            if (currentTime > 1.0f) return;
        }
    }
}
