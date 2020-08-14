using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_Flame : MonoBehaviour
{
    ParticleSystem PS;
    public float flameSize = 0;
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
    private void OnTriggerStay(Collider collision)
    {
        // 만약 트리거에 접촉한게 Enemy라면,
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Enemy의 컴포넌트 Enemy 스크립트의 OnDamageProcess를 호출한다
            Enemy em = collision.gameObject.GetComponent<Enemy>();
            if (em)
            {
                em.OnDamageProcess();
                Debug.Log("Enemy: " + collision.name + " is On Fire");
            }
        }
    }
}
