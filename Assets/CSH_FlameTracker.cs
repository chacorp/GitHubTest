using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_FlameTracker : MonoBehaviour
{
    public bool trackOn = false;
    float defaultSize;
    float damageTimer = 2f;
    // 파티클 시스템
    ParticleSystem PS;

    // 플레임 리스트 
    CSH_EnemyFlameManager EFM;

    // 따라갈 적
    public GameObject enemy;

    private void Awake()
    {
        PS = GetComponent<ParticleSystem>();
        EFM = GameObject.Find("EnemyFlameManager").GetComponent<CSH_EnemyFlameManager>();
    }

    void Update()
    {
        var flameParticle = PS.main;
        flameParticle.startSize = defaultSize;

        // 따라갈때
        if (trackOn)
        {
            transform.SetParent(null);

            transform.position = enemy.transform.position;

            if(enemy.activeSelf == false)
            {
                // 불의 크기 줄이기
                defaultSize -= Time.deltaTime;
                if(defaultSize <= 0)
                {
                    defaultSize = 0;
                    trackOn = false;
                }
            }
            else
            {
                // 불타라!
                defaultSize = 0.25f;

                damageTimer += Time.deltaTime;
                if(damageTimer >= 2f)
                {
                    Enemy em = enemy.GetComponent<Enemy>();
                    if (em)
                    {
                        em.OnDamageProcess();
                        Debug.Log("Enemy: " + em.name + " is burning!!!");
                    }
                    damageTimer = 0;
                }
            }
        }
        // 안 따라갈때
        else
        {
            // 자신을 리스트에 추가하고
            EFM.enemyFlame.Add(gameObject);
            transform.SetParent(EFM.gameObject.transform);
            // 추적할 에너미 비우기
            enemy = null;

            // 비활성화하기
            gameObject.SetActive(false);
        }
        
    }
}
