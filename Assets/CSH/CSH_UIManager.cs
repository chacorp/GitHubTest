using UnityEngine;

// ========================================================================
// 키패드 'Q' 를 누르면
// 1. 인벤토리 메뉴를 연다. ---------------------------------------------[O]
// 2. 인벤토리 속 아이템 위에 마우스 커서를 올리고 1-2-3-4-5 중에 하나를 누르면
//    해당 번호의 퀵 메뉴 아이템 바꾸기 ----------------------------------[ ]
// ========================================================================

public class CSH_UIManager : MonoBehaviour
{
    // 인벤토리 메뉴판
    public GameObject inventoryMenuUI;
    // 퀵메뉴판
    public CSH_QuickMenu quickMenuUI;
    // 커서 조준점
    public GameObject CursorUI;

    // 인벤토리 온오프
    bool isInventoryOn;

    void Start()
    {
        // 시작할때 미리 꺼두기
        isInventoryOn = false;
        inventoryMenuUI.SetActive(false);
    }

    void Open_Inventory()
    {
        // 켜져있으면 끄고, 꺼져있으면 킨다
        inventoryMenuUI.SetActive(!inventoryMenuUI.activeSelf);
        isInventoryOn = !isInventoryOn;

        // 인벤토리에 따라 퀵메뉴 온/오프 콘트롤하기
        quickMenuUI.QM_Control = isInventoryOn;

        // 인벤토리가 켜져있으면, 퀵메뉴도 같이 키기
        if (isInventoryOn)
        {
            quickMenuUI.showQM = true;
        }
        else
        {
            quickMenuUI.SetTimer = 2f;
        }
    }

    void Update()
    {
        // Q를 누르면 인벤토리 띄우기
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Open_Inventory();
        }

        // 아이템을 갖고 있냐 없냐 여부에 따라서 커서 조준점 키고 끄기
        // 서로 반대 결과로 해야함
        CursorUI.SetActive(!CSH_ItemGrab.Instance.hasItem);
    }
}
