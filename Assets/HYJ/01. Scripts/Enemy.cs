using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

// 가장 가까운 목적지를 탐색하여 이동하는 스크립트
public class Enemy : MonoBehaviour
{
    #region 에너미 상태변수
    enum EnemyState
    {
        Idle, Move, Damage, Die
    }
    [SerializeField] EnemyState state;
    #endregion
    [Header("목적지 정보")]
    [SerializeField] int destinationLength = 2;
    private GameObject[] destinations;
    private Vector3[] targetVectors;
    [SerializeField] private Vector3 dest;

    private NavMeshAgent spiderAgent;
    public Animator animSpider;
    private GameObject player;
    [SerializeField] private ObjectManager objMgr;

    [SerializeField] float speed = 3.0f;
    [SerializeField] float sprintSpeed = 5.0f;
    [SerializeField] float reactionRange = 4.0f;
    [SerializeField] float currentTime = 0.0f;
    [SerializeField] float damageDelayTIme = 1.0f;

    #region 에너미 체력변수
    private int maxHp = 2;
    private int currentHp = 0;
    #endregion

    private void OnEnable()
    {
        currentHp = maxHp;
        state = EnemyState.Idle;
        if (spiderAgent) spiderAgent.enabled = false;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        spiderAgent = GetComponent<NavMeshAgent>();
        spiderAgent.speed = speed;
        spiderAgent.enabled = false;
        //spiderAgent.enabled = true;
        //spiderAgent.SetDestination(GameObject.Find("Dest2").transform.position);
        if (animSpider == null) animSpider = GetComponentInChildren<Animator>();
        objMgr = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Damage:
                Damage();
                break;
        }
    }

    // 플레이어가 일정 범위 안으로 들어오면 상태를 Move로 변환하고 싶다.
    private void Idle()
    {
        Vector3 distance = player.transform.position - transform.position;
        spiderAgent.speed = speed;

        if (distance.magnitude < reactionRange)
        {
            state = EnemyState.Move;
        }
    }

    private void Move()
    {
        // 네브매쉬 에이전트 활성화
        if (spiderAgent.enabled == false)
        {
            spiderAgent.Warp(transform.position);
            spiderAgent.enabled = true;
        }

        // 목적지 변수 배열 초기화 & Destination 오브젝트 검색
        targetVectors = new Vector3[destinationLength];
        destinations = GameObject.FindGameObjectsWithTag("Destination");

        
        SetDestination();

        // 에너미의 위치가 목적지에 도달하게 되면 상태를 IDle로 변경한다.
        if (Vector3.Distance(transform.position , dest) <= spiderAgent.stoppingDistance) state = EnemyState.Idle;
    }

    private void Damage()
    {
        //데미지 받을 시 움직임이 잠시 Delay 되었다가 더 빠른 속도로 이동하게 하고 싶다.
        currentTime += Time.deltaTime;
        if (currentTime < damageDelayTIme)
        {
            spiderAgent.enabled = false;
        }
        else if (damageDelayTIme <= currentTime)
        {
            state = EnemyState.Move;
            spiderAgent.speed = sprintSpeed;
            currentTime = 0;
        }
    }

    private void SetDestination()
    {
        if (destinations != null)
        {
            // 목적지와의 거리 값을 나타내는 배열 생성
            for (int i = 0; i < destinations.Length; i++)
            {
                targetVectors[i] = destinations[i].transform.position - transform.position;
                if (targetVectors[i] == null) Debug.Log("Cannot find destination " + destinations[i].name);
            }

            // 목적지와의 거리를 가까운 기준으로 정렬하는 함수 (선택 정렬)
            for (int i = 0; i < targetVectors.Length - 1; i++)
            {
                for (int j = i + 1; j < targetVectors.Length; j++)
                {
                    if (targetVectors[i].magnitude > targetVectors[j].magnitude)
                    {
                        Vector3 temp = targetVectors[i];
                        targetVectors[i] = targetVectors[j];
                        targetVectors[j] = temp;
                    }
                }
            }
            dest = (targetVectors[0] + transform.position);
            animSpider.SetTrigger("IsMoving");
            spiderAgent.SetDestination(dest);
            spiderAgent.autoBraking = false;
            spiderAgent.stoppingDistance = 1.0f;
        }
    }

    // 에너미가 피격되면 호출되는 함수, 다른 클래스에서 적용시키기 위해 public으로 수식
    public void OnDamageProcess()
    {
        currentHp--;
        if (currentHp > 0)
        {
            state = EnemyState.Damage;
        }
        else
        {
            state = EnemyState.Die;
            StartCoroutine(Die());
        }
    }

    public float squeezeSpeed = 0.3f;

    private IEnumerator Die()
    {
        // 게임 오브젝트의 Y 스케일 값을 작게 만들고 싶다.
        spiderAgent.enabled = false;
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float z = transform.localScale.z;


        while (transform.localScale.y > transform.localScale.y * 0.3f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(x, y * 0.3f, z), squeezeSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        DetectName();
    }

    private void DetectName()
    {
        if (gameObject.name.Contains("SpiderS")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[0]);
        if (gameObject.name.Contains("SpiderS2")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[1]);
        if (gameObject.name.Contains("SpiderL")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[2]);
        if (gameObject.name.Contains("Beetle")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[3]);
    }
}
