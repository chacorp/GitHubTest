using UnityEngine;

// ========================================================================
// 키패드 1, 2, 3, 4, 5 를 누르면 
// 1. 아이템을 바꾼다.---------------------------------------------------[ ]
// 2. 퀵 메뉴를 띄웠다가 다시 내린다. ------------------------------------[O]
// ========================================================================

public class CSH_QuickMenu : MonoBehaviour
{

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

    void Start()
    {
        // 퀵 메뉴 보여주기
        showQM = true;

        // 현재 위치 가져오기
        Ypos_D = QM_RT.anchoredPosition.y; // 540

        // 시작하면서 퀵 메뉴 올려놓기
        QM_RT.anchoredPosition = new Vector2(QM_RT.anchoredPosition.x, Ypos);
        showQM = true;
    }

    void Show_QuickMenu()
    {
        if (showQM)
        {
            QM_RT.gameObject.SetActive(true);
            // 천천히 올리기
            QM_RT.anchoredPosition += new Vector2(0, 1) * moveSpeed * Time.deltaTime;

            // 적당한 위치에서 멈추기
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
            QM_RT.anchoredPosition -= new Vector2(0, 1) * moveSpeed * Time.deltaTime;
            if (QM_RT.anchoredPosition.y <= Ypos_D)
            {
                QM_RT.anchoredPosition = new Vector2(QM_RT.anchoredPosition.x, Ypos_D);

                QM_RT.gameObject.SetActive(false);
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
    }
}
