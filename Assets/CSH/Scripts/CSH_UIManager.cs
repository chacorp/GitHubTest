using System.Collections.Generic;
using UnityEngine;

// ========================================================================
// 키패드 'Q' 를 누르면
// 1. 인벤토리 메뉴를 연다. ---------------------------------------------[O]
// 2. 인벤토리 속 아이템 위에 마우스 커서를 올리고 1-2-3-4-5 중에 하나를 누르면
//    해당 번호의 퀵 메뉴 아이템 바꾸기 ----------------------------------[ ]

// 마우스 커서를 아이템에 갖다 대고,
// 3. [E] 키를 누르고,
//           - B. 만약 [특수 템]이면, 바로 인벤토리에 넣기 ---------------[O] <<<<<<<<<<<<<<    CSH_ItemGrab.cs
//                               퀵 메뉴가 비어있으면 퀵 메뉴에도 넣기 ---[ ]   
// ========================================================================

public class CSH_UIManager : MonoBehaviour
{
    // 싱글톤으로 만들기
    public static CSH_UIManager Instance;
    CSH_UIManager()
    {
        Instance = this;
    }

    // 인벤토리 메뉴판
    public GameObject inventoryMenuUI;
    public GameObject quickMenuUI;
    public Transform item_icons;

    // 퀵메뉴판
    public CSH_QuickMenu CSH_QM;

    // 커서 조준점
    public GameObject CursorUI;

    // 인벤토리 온오프
    bool isInventoryOn;


    // 인벤토리 아이템 상자
    public List<GameObject> I_Box = new List<GameObject>();

    // 퀵 메뉴 아이템 상자
    List<Vector2> QM_Box = new List<Vector2>();

    public int iBoxCount = 0;

    void Start()
    {
        // 퀵 메뉴 스크립트 가져오기
        CSH_QM = GetComponent<CSH_QuickMenu>();

        // 리스트에 인벤토리 네모들의 위치 값 넣어두기
        for (int i = 0; i < inventoryMenuUI.transform.childCount; i++)
        {
            I_Box.Add(inventoryMenuUI.transform.GetChild(i).gameObject);
        }
        // 리스트에 퀵 메뉴 네모들의 위치 값 넣어두기
        for (int i = 0; i < quickMenuUI.transform.childCount; i++)
        {
            QM_Box.Add(quickMenuUI.transform.GetChild(i).GetComponent<RectTransform>().position);
        }

        // 시작할때 미리 꺼두기
        isInventoryOn = false;
        // 인벤토리 메뉴
        inventoryMenuUI.SetActive(false);
        // 아이콘 담아두는 곳
        item_icons.gameObject.SetActive(false);
    }

    void Open_Inventory()
    {
        // 켜져있으면 끄고, 꺼져있으면 킨다
        inventoryMenuUI.SetActive(!inventoryMenuUI.activeSelf);
        isInventoryOn = !isInventoryOn;

        // 켜고 끄기를, 인벤토리 메뉴랑 똑같이 적용한다!
        item_icons.gameObject.SetActive(inventoryMenuUI.activeSelf);
                

        // 인벤토리에 따라 퀵메뉴 온/오프 콘트롤하기
        CSH_QM.stayQM = isInventoryOn;

        // 인벤토리가 켜져있으면, 퀵메뉴도 같이 키기
        if (isInventoryOn)
        {
            CSH_QM.showQM = true;
        }
        else
        {
            CSH_QM.SetTimer = 2f;
        }
    }


    void Update()
    {
        // Q를 누르면 인벤토리 띄우기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Open_Inventory();
        }

        // 아이템을 갖고 있냐 없냐 여부에 따라서 커서 조준점 키고 끄기
        // 서로 반대 결과로 해야함
        CursorUI.SetActive(!CSH_ItemGrab.Instance.hasItem);

        // 갖고 있는 아이템이 5개 이하일때 
        // 퀵 메뉴에도 아이콘 띄우기 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 흠....
        if(item_icons.childCount <= 5)
        {

        }
    }
}
