using UnityEngine;

public class CSH_QuickMenu : MonoBehaviour
{        
    public float moveSpeed = 200f;
    RectTransform RT;
    float Ypos = 540f;
    float Ypos_D;
    float timer;
    bool showQM;
    void Start()
    {
        // 퀵 메뉴 보여주기
        showQM = true;
        RT = GetComponent<RectTransform>();

        // 현재 위치 가져오기
        Ypos_D = RT.anchoredPosition.y; // 540

        // 시작하면서 퀵 메뉴 올려놓기
        RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, Ypos);
        showQM = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            showQM = true;
            timer = 0;
        }

        if (showQM)
        {
            // 천천히 올리기
            RT.anchoredPosition += new Vector2(0, 1) * moveSpeed * Time.deltaTime;

            // 적당한 위치에서 멈추기
            if (RT.anchoredPosition.y >= Ypos)
            {
                RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, Ypos);
                timer += Time.deltaTime;
                if (timer > 2)
                {
                    timer = 0;
                    showQM = false;
                }
            }
        }
        else
        {
            RT.anchoredPosition -= new Vector2(0, 1) * moveSpeed * Time.deltaTime;
            if (RT.anchoredPosition.y <= Ypos_D)
            {
                RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, Ypos_D);
            }
        }
    }
}
