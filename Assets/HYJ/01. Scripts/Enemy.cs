using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 가장 가까운 목적지를 탐색하여 이동하는 스크립트
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    #region 에너미 상태변수
    enum EnemyState
    {
        Idle, Move, Damage, Die, ReMove
    }
    [SerializeField] EnemyState state;
    #endregion
    [Header("목적지 정보")]
    private GameObject[] destinations;
    private GameObject[] spawnPoints;
    private List<Vector3> targetVectors;
    private Vector3[] retargetVectors;
    [SerializeField] private Vector3 currrentDest;   // SetDestination에서 정해지는 목적지 변수

    private NavMeshAgent spiderAgent;
    public Animator animSpider;
    [SerializeField] AudioSource enemyAudio;
    [SerializeField] AudioClip[] idleClips;
    [SerializeField] AudioClip[] moveClips;
    [SerializeField] AudioClip[] dieClips;
    [SerializeField] AudioClip damageClip;
    private GameObject player;
    [SerializeField] private ObjectManager objMgr;

    [SerializeField] float speed = 1.2f;
    [SerializeField] float sprintSpeed = 2.0f;
    [SerializeField] float reactionRange = 4.0f;
    [SerializeField] float currentTime = 0.0f;
    [SerializeField] float damageDelayTIme = 1.0f;
    [SerializeField] float dieJumpPower = 1.5f;
    [SerializeField] GameObject enemyBlood;
    [SerializeField] GameObject enemyBloodSpatter;

    bool isReached = false;
    public bool isBleeding = false;
    private bool isSetdestination = false;
    Rigidbody rigidbody;

    //#region 퀘스트 관련 변수
    //QuestManager questManager;

    //#endregion

    #region 에너미 체력변수
    private int maxHp = 2;
    public int currentHp = 0;
    #endregion

    private void OnEnable()
    {
        currentHp = maxHp;
        state = EnemyState.Idle;
        if (spiderAgent) spiderAgent.enabled = false;
        enemyAudio = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        player = GameObject.Find("Player");
        spiderAgent = GetComponent<NavMeshAgent>();
        spiderAgent.enabled = false;
        if (animSpider == null) animSpider = GetComponentInChildren<Animator>();
        objMgr = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        // 목적지 변수 배열 초기화 & Destination 오브젝트 검색
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        destinations = GameObject.FindGameObjectsWithTag("Destination");
        isSetdestination = false;
    }

    void Start()
    {
        //player = GameObject.Find("Player");
        //spiderAgent = GetComponent<NavMeshAgent>();
        //spiderAgent.enabled = false;
        //if (animSpider == null) animSpider = GetComponentInChildren<Animator>();
        //objMgr = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        //// 목적지 변수 배열 초기화 & Destination 오브젝트 검색
        //spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        //destinations = GameObject.FindGameObjectsWithTag("Destination");
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
            case EnemyState.ReMove:
                ReMove();
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
        spiderAgent.speed = Random.Range(speed - 0.5f, speed);
        animSpider.SetBool("IsMoving", false);

        if (distance.magnitude < reactionRange)
        {
            state = EnemyState.Move;
            PlayRandomSound(isReached);
            isReached = true;
        }
        else if (distance.magnitude > reactionRange) isReached = false;
    }

    // 목적지를 설정하고 이동하게 되는 상태
    private void Move()
    {
        // 네브매쉬 에이전트 활성화
        if (spiderAgent.enabled == false)
        {
            spiderAgent.enabled = true;
        }

        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        targetVectors = new List<Vector3>();
        SetDestination();

        Vector3 distance = player.transform.position - transform.position;
        if (distance.magnitude < reactionRange)
        {
            PlayRandomSound(isReached);
            isReached = true;
        }
        else if (distance.magnitude > reactionRange) isReached = false;

        // 에너미의 위치가 목적지에 도달하게 되면 상태를 ReMove 변경한다.
        if (Vector3.Distance(transform.position, currrentDest) <= spiderAgent.stoppingDistance)
        {
            state = EnemyState.ReMove;
            ReSettingDestination();
            //isBleeding = false;
        };
    }

    // 목적지에 도달하게 되었을 때 플레이어가 반응 범위 안으로 들어오게 되면 다시 목적지를 설정하게 되고 이동을 하게 된다.
    // 목적지에 도달 했을 때, 플레이어가 반응 범위에 없으면 상태를 Idle 상태로 바꾼다. 
    private void ReMove()
    {
        Vector3 distance = player.transform.position - transform.position;

        if (distance.magnitude < reactionRange)
        {
            //ReSettingDestination();
            PlayRandomSound(isReached);
            isReached = true;
        }
        else if (distance.magnitude > reactionRange) isReached = false;

        if (Vector3.Distance(transform.position, currrentDest) <= spiderAgent.stoppingDistance)
        {
            state = EnemyState.Idle;
            isSetdestination = false;
        }
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
            isSetdestination = false;
            isBleeding = true;
            animSpider.SetBool("Flinched", false);
            spiderAgent.speed = Random.Range(sprintSpeed - 0.2f, sprintSpeed);
            currentTime = 0;
        }
    }

    private void SetDestination()
    {
        if (destinations != null && !isSetdestination)
        {
            Debug.Log("SetDestination");
            // 목적지와의 거리 값을 나타내는 배열 생성
            for (int i = 0; i < destinations.Length; i++)
            {
                targetVectors.Add(destinations[i].transform.position - transform.position);
                if (targetVectors[i] == null) Debug.Log("Cannot find destination " + destinations[i].name);
            }

            foreach (Vector3 temp in targetVectors)
            {
                if (Vector3.Distance(transform.position, temp) < spiderAgent.stoppingDistance + 1.2f)
                {
                    targetVectors.Remove(temp);
                    break;
                }
            }

            // 목적지와의 거리를 가까운 기준으로 정렬하는 함수 (선택 정렬)
            for (int i = 0; i < targetVectors.Count - 1; i++)
            {
                for (int j = i + 1; j < targetVectors.Count; j++)
                {
                    if (targetVectors[i].magnitude > targetVectors[j].magnitude)
                    {
                        Vector3 temp = targetVectors[i];
                        targetVectors[i] = targetVectors[j];
                        targetVectors[j] = temp;
                    }
                }
            }
            currrentDest = (targetVectors[0] + transform.position);
            Debug.Log($" 현재 목적지는 {currrentDest} 입니다!");
            animSpider.SetBool("IsMoving", true);
            spiderAgent.SetDestination(currrentDest);
            spiderAgent.autoBraking = false;
            spiderAgent.stoppingDistance = 0.3f;
            isSetdestination = true;
        }
    }



    void ReSettingDestination()
    {
        Debug.Log("ResetDestination");
        // 이동 가능한 지점을 담은 List 변수 생성
        List<GameObject> otherDestinations = new List<GameObject>();

        int randNum = Random.Range(0, 2);
        if (randNum == 0)
        {
            for (int i = 0; i < destinations.Length; i++)
            {
                otherDestinations.Add(destinations[i]);
            }
            Debug.Log("Find Destination");
        }
        else if (randNum == 1)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                otherDestinations.Add(spawnPoints[i]);
            }
            Debug.Log("Find spawnPoints");
        }

        foreach (GameObject temp in otherDestinations)
        {
            if ((temp.transform.position - transform.position).magnitude < spiderAgent.stoppingDistance + 1.2f)
            {
                otherDestinations.Remove(temp);
                break;
            }
        }

        // 목적지와 나의 위치 차이값을 가지는 배열 변수 만들기
        retargetVectors = new Vector3[otherDestinations.Count];
        for (int i = 0; i < otherDestinations.Count; i++)
        {
            retargetVectors[i] = otherDestinations[i].transform.position - transform.position;
            if (retargetVectors[i] == null) Debug.Log("Cannot find destination " + destinations[i].name);
        }

        // 목적지와의 거리를 가까운 기준으로 정렬하는 함수 (선택 정렬)
        for (int i = 0; i < retargetVectors.Length - 1; i++)
        {
            for (int j = i + 1; j < retargetVectors.Length; j++)
            {
                if (retargetVectors[i].magnitude > retargetVectors[j].magnitude)
                {
                    Vector3 temp = retargetVectors[i];
                    retargetVectors[i] = retargetVectors[j];
                    retargetVectors[j] = temp;
                }
            }
        }

        int rand = Random.Range(0, retargetVectors.Length);

        currrentDest = (retargetVectors[rand] + transform.position);
        animSpider.SetBool("IsMoving", true);
        spiderAgent.SetDestination(currrentDest);
        spiderAgent.autoBraking = false;
        spiderAgent.stoppingDistance = 0.3f;
    }

    // 에너미가 피격되면 호출되는 함수, 다른 클래스에서 적용시키기 위해 public으로 수식
    public void OnDamageProcess()
    {
        if (state == EnemyState.Die)
        {
            return;
        }

        currentHp--;
        StartCoroutine(DamageVFX());

        if (currentHp > 0)
        {
            state = EnemyState.Damage;
            animSpider.SetBool("IsMoving", false);
            animSpider.SetBool("Flinched", true);
            PlayRandomSound(isReached);
            spiderAgent.enabled = false;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.AddForce(-Vector3.forward * 2 * dieJumpPower, ForceMode.Force);
        }
        else
        {
            state = EnemyState.Die;
            PlayRandomSound(isReached);
            spiderAgent.enabled = false;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.AddForce(Vector3.up * dieJumpPower, ForceMode.Impulse);
            rigidbody.AddTorque(Vector3.right * 100, ForceMode.Impulse);
            animSpider.SetTrigger("IsDying");
            StartCoroutine(Die());
        }
    }

    public float squeezeSpeed = 0.3f;

    private IEnumerator DamageVFX()
    {
        GameObject enemyDamageVFX = Instantiate(enemyBlood);
        GameObject enemyDamageVFX2 = Instantiate(enemyBloodSpatter);
        enemyDamageVFX.transform.position = transform.position;
        enemyDamageVFX2.transform.position = transform.position;
        enemyDamageVFX2.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.3f);
        enemyDamageVFX2.GetComponent<ParticleSystem>().Pause();
    }

    private IEnumerator Die()
    {
        // 현재 진행중인 퀘스트에 맞게 퀘스트 변수를 증가시켜준다.
        if (QuestManager.Instance.quests[0].isActive || QuestManager.Instance.quests[3].isActive || QuestManager.Instance.quests[2].isActive)
        {
            QuestManager.Instance.quests[0].goal.EnemyKilled();
            QuestManager.Instance.quests[3].goal.EnemyKilled();
        }

        // 게임 오브젝트의 Y 스케일 값을 작게 만들고 싶다.
        float t = 0;
        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(1, 0.1f, 1);
        while (t <= 1)
        {
            transform.localScale = Vector3.Lerp(start, end, t);
            t += Time.deltaTime;
            yield return 0;
        }
        yield return new WaitForSeconds(2.0f);
        DetectName(gameObject);
    }

    private void DetectName(GameObject gameObject)
    {
        Debug.Log($"DetectName() is called");
        if (gameObject.name.Contains("SpiderS2")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[1]);
        else if (gameObject.name.Contains("SpiderS")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[0]);
        else if (gameObject.name.Contains("SpiderL")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[2]);
        else if (gameObject.name.Contains("Beetle")) objMgr.ReturnObject(gameObject, objMgr.enemyObjectPools[3]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon") || other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Debug.Log($"{gameObject.name} is attacked!");
            OnDamageProcess();
        }
    }

    void PlayRandomSound(bool isReaced)
    {
        if (state == EnemyState.Idle && !isReaced)
        {
            int randNum = Random.Range(0, idleClips.Length);
            enemyAudio.PlayOneShot(idleClips[randNum]);

        }
        else if (state == EnemyState.Move && !isReaced)
        {
            int randNum = Random.Range(0, moveClips.Length);
            enemyAudio.PlayOneShot(moveClips[randNum]);

        }
        else if (state == EnemyState.ReMove && !isReached)
        {
            int randNum = Random.Range(0, moveClips.Length);
            enemyAudio.PlayOneShot(moveClips[randNum]);
        }
        else if (state == EnemyState.Damage)
        {
            enemyAudio.PlayOneShot(damageClip);
        }
        else if (state == EnemyState.Die)
        {
            int randNum = Random.Range(0, dieClips.Length);
            enemyAudio.PlayOneShot(dieClips[randNum]);
        }
    }

}
