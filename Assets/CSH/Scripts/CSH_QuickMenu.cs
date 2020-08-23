using System;
using UnityEngine;

// ========================================================================
// 키패드 1, 2, 3, 4, 5 를 누르면 
// 1. 아이템을 바꾼다.---------------------------------------------------[ ]
// 2. 퀵 메뉴를 띄웠다가 다시 내린다. ------------------------------------[O]
// ========================================================================

public class CSH_QuickMenu : MonoBehaviour
{
    [Header("QuickMenu Properties")]
    // 퀵 메뉴 보여줄 속도
    public float moveSpeed = 1000f;

    // UI 트랜스폼
    public RectTransform QM_RT;

    // UI 올라올 위치
    public float Ypos = 540f;

    // UI 보여주는 시간
    public float visibleTime = 2f;

    // UI 현재 위치
    float Ypos_D;


    // 타이머
    float timer;
    public float SetTimer
    {
        set
        {
            timer = value;
        }
    }

    // 퀵 메뉴 온/오프 여부
    public bool stayQM;
    public bool showQM;

    [Header("Showing icons on QuickMenu")]
    public Transform QM_icons;
    public Transform grabedItem_icons;

    public int grabeditem_Count = 0;

    void Start()
    {
        // 퀵 메뉴 보여주기
        showQM = true;

        // 현재 위치 가져오기
        Ypos_D = QM_RT.anchoredPosition.y; // 540

        // 시작하면서 퀵 메뉴 올려놓기
        QM_RT.anchoredPosition = new Vector2(QM_RT.anchoredPosition.x, Ypos);
        showQM = true;

        QM_icons.gameObject.SetActive(false);
    }

    void Show_QuickMenu()
    {
        if (showQM)
        {
            // 1. 활성화기
            QM_RT.gameObject.SetActive(true);

            // 2. 천천히 올리기
            QM_RT.anchoredPosition += new Vector2(0, 1) * moveSpeed * Time.deltaTime;

            // 3. 적당한 위치에서 멈추기
            if (QM_RT.anchoredPosition.y >= Ypos)
            {
                QM_RT.anchoredPosition = new Vector2(QM_RT.anchoredPosition.x, Ypos);

                // 타이머 무시하고 멈춰있기
                if (stayQM) return;

                // 2초정도 기다리기
                timer += Time.deltaTime;
                if (timer > visibleTime)
                {
                    timer = 0;
                    showQM = false;
                }
            }
        }
        else
        {
            // 1. 천천히 내리기
            QM_RT.anchoredPosition -= new Vector2(0, 1) * moveSpeed * Time.deltaTime;

            // 2. 적당한 위치에서 멈추기
            if (QM_RT.anchoredPosition.y <= Ypos_D)
            {
                QM_RT.anchoredPosition = new Vector2(QM_RT.anchoredPosition.x, Ypos_D);

                // 3. 비활성화하기
                QM_RT.gameObject.SetActive(false);
            }
        }
    }


    private void Show_Icon()
    {
        // QM이 알고 있는 아이템의 갯수와    실제 플레이어가 획득한 아이콘의 갯수가 다르다면,
        //             = 플레이어가 아이템을 획득했다!
        //                                 +
        //                      퀵메뉴 칸의 갯수보다 작다면
        if (grabeditem_Count < grabedItem_icons.childCount && grabeditem_Count < 5)
        {
            // 플레이어가 획득한 아이콘 의 트랜스폼을 가져온다.
            Transform item = grabedItem_icons.GetChild(grabeditem_Count);

            // QM의 자식들 중에서 가져온 이름과 같은 것을 찾아서 활성화한다
            for (int i = 0; i < QM_icons.childCount; i++)
            {
                if (QM_icons.GetChild(i).name == item.name)
                {
                    // 해당하는 오브젝트를 가져오고
                    GameObject QMicon = QM_icons.GetChild(i).gameObject;

                    // 아이콘을 옮겨놓을 위치의 좌표
                    RectTransform BTNpos = QM_RT.GetChild(grabeditem_Count).GetComponent<RectTransform>();

                    // 옮길 아이콘의 좌표
                    RectTransform QM_Ipos = QMicon.GetComponent<RectTransform>();

                    Debug.Log(BTNpos.name + " :: " );

                    // 둘다 가져올 수 있다면
                    if (BTNpos && QM_Ipos)
                    {
                        QM_Ipos.SetParent(BTNpos);
                        QM_Ipos.sizeDelta     = new Vector2(100f, 100f);
                        QM_Ipos.localPosition = new Vector2(0, 50);
                        QM_Ipos.localRotation = BTNpos.localRotation;
                        QM_Ipos.localScale    = BTNpos.localScale;
                    }

                    grabeditem_Count++;

                    QMicon.SetActive(true);
                }
            }
        }
    }
    void Update()
    {
        // 1,2,3,4,5 중에 하나라도 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            showQM = true;
            timer = 0;
        }

        // 퀵 메뉴 보여주기
        Show_QuickMenu();

        Show_Icon();
    }
}
