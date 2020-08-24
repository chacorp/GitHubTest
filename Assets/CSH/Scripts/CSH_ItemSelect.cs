using UnityEngine;

//===============================================
// 마우스 커서를 아이템에 갖다 대면
// 1. 아이템에 아웃라인 만들기 -----------------[O]
//===============================================
[RequireComponent(typeof(Rigidbody))]
public class CSH_ItemSelect : MonoBehaviour
{
    // [아이템]을 잡고 있나?
    public bool isGrabed;

    // 이 [아이템]이 [특수 템]인가?
    public bool isSpecialItem;

    // 아웃라인 온/오프 머티리얼
    [Header("Shader")]
    Outline outliner;

    GameObject player;

    [Header("Sparkling VFX")]

    // 파티클 이펙트 오브젝트 변수
    public GameObject glowVFXFactory;
    public float reachRange = 2.5f;

    // 반짝이는지 여부
    bool isGlowed;

    // RaycastHit로 가리켜지고 있는지 여부
    public bool outlineOn;

    // 자신의 레이어를 저장할 변수
    int itemLayerMask = 0;

    private void Start()
    {
        isGrabed = false;

        // 자신의 레이어 저장
        itemLayerMask = gameObject.layer;

        // 게임 오브젝트에 아웃라인 에셋에서 다운받은 스크립트 추가하기
        // + 에셋 스크립트에 설정 넣어주기
        gameObject.AddComponent<Outline>();
        outliner = GetComponent<Outline>();

        if (isSpecialItem)
        {
            // 특수 아이템 외곽선 = 초록색
            outliner.OutlineColor = new Color(0.3f, 1f, 0.3f);
            outliner.OutlineWidth = 8f;
        }
        else
        {
#if VR_MODE
            // 일반 아이템 외곽선 = 노란색
            outliner.OutlineColor = new Color(1f, 1f, 0f);
            outliner.OutlineWidth = 8f;
#elif EDITOR_MODE
            // 특수 아이템 외곽선 = 초록색
            outliner.OutlineColor = new Color(0.3f, 1f, 0.3f);
            outliner.OutlineWidth = 8f;
#endif
        }

        // 아웃라인은 기본으로 꺼놓기
        outliner.enabled = false;

        // 플레이어 찾기
        player = CSH_RayManager.Instance.player;
        isGlowed = false;


        // ---------------------------------------------------------------------- < 반짝이 이펙트 >
        // 1. 플레이어가 잡을 수 있는 [특수 아이템(= 도구)]에 반짝이 이펙트 추가하기
        // 2. Start() 시작할때 미리 만들어서 자식으로 넣어두기
        // 3. 플레이어가 일정 거리 안으로 들어오면 반짝이 보여주기
        // 4. 처음 딱 한번만 보여주기
        if (isSpecialItem)
        {
            GameObject glowVFX = Instantiate(glowVFXFactory);
            // 위치 맞추기
            glowVFX.transform.position = transform.position;
            // 자식으로 넣기
            glowVFX.transform.SetParent(transform);
            // 반짝이 꺼놓기
            glowVFX.SetActive(false);
        }
    }

    // 반짝이 효과
    void GlowVFX_On()
    {
        // 플레이어가 일정 범위 안으로 들어오게 되면 아이템에서 반짝이는 효과를 연출하고 싶다.
        //Vector3 reachDistace = transform.position - player.transform.position;

        // 플레이어와 아이템과의 거리 => distance
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // 만약 [특수아이템]이고
        // 한 번도 안 켜졌고
        // distance의 크기가 설정된 거리보다 작다면
        if (isSpecialItem && !isGlowed && distance < reachRange)
        {
            // 자식 오브젝트로 들어있는 반짝이 이펙트 가져오기
            GameObject glowVFX = transform.GetChild(transform.childCount - 1).gameObject;
            // * transform.childCount-1 하는 이유
            //      => 각자 자식의 갯수가 다를 수 있고, 어쨌든 이펙트가 가장 마지막에 추가된 자식이라서
            //                가장 마지막 자식이 곧 반짝이 이펙트기 때문이다!

            // 반짝이 켜주기
            glowVFX.SetActive(true);
            isGlowed = true;
        }
    }

    // 레이어 변경!
    void LayerChange()
    {
        // 아이템 레이어 Weapon으로 바꾸기 
        // => 이렇게 하는 이유는 다른 물체에 겹쳐서 안보이는 걸 방지하기 위함
        if (isGrabed)
        {
            if (CSH_UIManager.Instance.CSH_QM.showQM)
            {
                gameObject.layer = itemLayerMask;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Weapon");
            }
        }
        else
        {
            gameObject.layer = itemLayerMask;
        }
    }

    // 아웃라인 그려주기!
    void ShowOutline()
    {
        // 플레이어와 충분히 가까이 있지 않다면
        if (!CSH_RayManager.Instance.isNear)
        {
            // 아웃라인 끄기
            outliner.enabled = false;
            return;
        }

        // 왼손 ray 또는 오른손 ray에 맞았다면,
        if(CSH_RayManager.Instance.raycastHitObject_L == gameObject.transform || CSH_RayManager.Instance.raycastHitObject_R == gameObject.transform)
        {
            // 아웃라인 켜기
            outlineOn = true;
        }
        // 왼손 ray 또는 오른손 ray에 안 맞았다면,
        else
        {
            // 아웃라인 끄기
            outlineOn = false;
        }

        if (outlineOn)
        {
            // 플레이어가 [아이템]을 잡고 있는 상태라면, 아웃라인 만들지 않기
            if (isGrabed)
            {
                // 아웃라인 제거하기
                outliner.enabled = false;

                // 빠져나가기
                return;
            }
            outliner.enabled = true;
        }
        else
        {
            // 아웃라인 제거하기
            outliner.enabled = false;
        }
    }

    private void Update()
    {
        GlowVFX_On();
        ShowOutline();
        LayerChange();
    }


    // [아이템] 위에 커서를 올리면, 아웃라인 만들기
    //private void OnMouseEnter()
    //{
    //    // 플레이어가 [아이템]을 잡고 있는 상태라면, 아웃라인 만들지 않기
    //    if (isGrabed)
    //    {
    //        // 아웃라인 제거하기
    //        outliner.enabled = false;

    //        // 빠져나가기
    //        return;
    //    }
    //    outliner.enabled = true;

    //    // 현재 가리키는 [아이템]으로 두기
    //    CSH_ItemGrab.Instance.pointingItem = gameObject;
    //}



    // [아이템] 위에 커서를 치우면, 아웃라인 없애기
    //private void OnMouseExit()
    //{
    //    //// 아웃라인 제거하기
    //    outliner.enabled = false;

    //    // 현재 가리키는 [아이템]에서 빼기
    //    CSH_ItemGrab.Instance.pointingItem = null;
    //}



    // 적에 부딪히면 데미지 프로세스 호출

    //private void OnTriggerEnter (Collider other)
    //{
    //    // 부딪힌 트리거가 Enemy 스크립트를 갖고 있다면
    //    Enemy enemy = other.gameObject.GetComponent<Enemy>();
    //    if (enemy)
    //    {
    //        // Enemy 스크립트에서 OnDamageProcess() 실행하기
    //        Debug.Log($"{other.gameObject.name} is attacked!");
    //        enemy.OnDamageProcess();
    //    }
    //}

    private void OnCollisionEnter(Collision other)
    {
        //부딪힌 트리거가 Enemy 스크립트를 갖고 있다면
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            // Enemy 스크립트에서 OnDamageProcess() 실행하기
            Debug.Log($"{other.gameObject.name} is attacked!");
            enemy.OnDamageProcess();

            if (enemy.currentHp == 0)
            {
                if (QuestManager.Instance.quests[2].isActive) QuestManager.Instance.quests[2].goal.EnemyKilled();
                Debug.Log("박스를 이용해 적을 처치했습니다");
            }
        }
    }
}
