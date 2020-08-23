using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_Flame : MonoBehaviour
{
    ParticleSystem PS;
    public float flameSize = 0;

    [Header("EnemyFlameManager")]
    public CSH_EnemyFlameManager EFM;


    private void Start()
    {
        PS = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // 불의 크기 적용하기
        var flameParticle = PS.main;
        flameParticle.startSize = flameSize;
    }
    // 트리거에 접촉하는 중이라면
    private void OnTriggerEnter(Collider collision)
    {
        // 만약 트리거에 접촉한게 Enemy라면,
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Enemy의 컴포넌트 Enemy 스크립트의 OnDamageProcess를 호출한다
            Enemy em = collision.gameObject.GetComponent<Enemy>();
            if (em)
            {

                // 데미지는 여기서 직접 하지 않고, CSH_FlameTracker에서 조정한다!
                //em.OnDamageProcess();
                Debug.Log("Enemy: " + collision.name + " is On Fire");

                // 불꽃 가져오기 = 리스트의 마지막 녀석
                GameObject eFlame = EFM.enemyFlame[EFM.enemyFlame.Count - 1];

                // 리스트에서 지우기
                EFM.enemyFlame.Remove(eFlame);

                // 가져온 불꽃에서 CSH_FlameTracker를 접근할 수 있다면
                CSH_FlameTracker FT = eFlame.GetComponent<CSH_FlameTracker>();
                if (FT)
                {
                    // Enemy를 따라가라고 명령하기
                    eFlame.SetActive(true);
                    FT.enemy = em.gameObject;
                    FT.trackOn = true;
                }

            }
        }
    }
}
