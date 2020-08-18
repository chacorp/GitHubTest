﻿using FPSControllerLPFP;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

// ===========================================================================
// 마우스 커서를 아이템에 갖다 대면,
// 1. 화면에 [E] 메시지 띄우기 ----------------------------------------------[O]
//
// 2. [E] 키를 눌렀을때,   *만약 당겨오는 경로에 물체가 있다면 낑기기* --------[ ] <<<<<<<<<<<<<<   ?
//       
//    - A. 그냥 [아이템]이면, 눈 앞으로 가져오기 -----------------------------[O]
//      +
//      A-1. 마우스 우클릭을 하고 마우스를 움직이면 아이템 회전하기 -----------[O]
//      A-2. 마우스 좌클릭을 하면 아이템 던져버리기!! ------------------------[O]


//    - B. 만약 [특수 템]이면, 바로 인벤토리에 넣기 --------------------------[O]
//                               퀵 메뉴가 비어있으면 퀵 메뉴에도 넣기 -------[ ] <<<<<<<<<<<<<<    CSH_UIManager.cs 

// 3.  1,2,3,4,5 번으로 활성화 무기 바꾸기 ----------------------------------[O] <<<<<<<<<<<<<<    CSH_ItemSwitch.cs 
// ===========================================================================

public class CSH_ItemGrab : MonoBehaviour
{
    public static CSH_ItemGrab Instance;
    CSH_ItemGrab()
    {
        Instance = this;
    }


    [Header("Interacting Object")]
    // 현재 마우스 커서로 가리키고 있는 아이템
    public GameObject pointingItem;

    // 선택한 아이템
    GameObject selectedItem;

    // 텍스트 UI
    public GameObject pressE;
    public GameObject Looking;

    // [특수 템]을 위한 인벤토리
    // 걍 무기 담아두는 곳
    public Transform inventory;
    List<GameObject> invntry = new List<GameObject>();

    // 무기 잡고 있는 곳
    public Transform Holder;

    // 현재 쓸 수 있는 무기의 리스트
    public List<GameObject> activeItems = new List<GameObject>();

    // 갖고 있는 아이템 갯수를 보내주기 위해 
    // 스크립트 가져옴
    public CSH_ItemSwitch itemSwitch;

    // 아이템의 컴포넌트를 담아둘 전역변수
    CSH_ItemSelect itemSelect;
    Rigidbody itemRB;


    //public FirstPersonController fpcController;
    public HandgunScriptLPFP handGun;

    [Header("Properties")]
    // 던져버릴 속도
    public float throwSpeed = 15f;
    // 가져올 속도
    public float grabSpeed = 100f;

    // 현재 [아이템]을 잡고 있나?
    public bool hasItem;
    bool grabing;


    private void Start()
    {
        grabing = false;
        hasItem = false;

        pressE.SetActive(false);
        Looking.SetActive(false);

        // 홀더에 있는 아이템들 모두 꺼놓기
        // 판자 제외
        for (int i = 1; i < Holder.childCount; i++)
        {
            Holder.GetChild(i).gameObject.SetActive(false);
        }

        //activeItems.Add(Holder.GetChild(0).gameObject); // ------------------------------클립보드 야매로 리스트에 넣음
    }

    void Grab_item()
    {
        // 선택한 [아이템]의 컴포넌트를 전역변수에 넣어두기
        if (selectedItem != null)
        {
            itemSelect = selectedItem.GetComponent<CSH_ItemSelect>();
            itemRB = selectedItem.GetComponent<Rigidbody>();
        }

        // 잡기 시작
        if (grabing)
        {
            // [E] 키 안내문 끄기
            pressE.SetActive(false);

            // 살펴보기 안내문 켜기
            Looking.SetActive(true);

            // [특수 템]이라면, --------------------------------------------------< 이렇게 하지 말고, 플레이어가 다 갖고 있다가 활성화하는 방식으로 하자!>
            if (itemSelect.isSpecialItem)
            {
                // 이름 가져오기
                string itemName = selectedItem.name;

                // 인벤토리 리스트에 추가하기
                invntry.Add(pointingItem);
                //selectedItem.transform.SetParent(inventory);

                // 아이템의 rigidbody 물리엔진 끄기
                itemRB.isKinematic = true;
                //itemRB.constraints = RigidbodyConstraints.FreezePosition;

                // 아이템의 Collider 끄기 => 안끄면 플레이어와 충돌나서 뒤로 밀린다!
                Collider itemCol = selectedItem.GetComponent<Collider>();
                //itemCol.enabled = false;


                // 아이콘의 위치 옮기기
                // => 자식 컴포넌트 가져오리가서 부모 옮기기보다 먼저 해야함
                // CSH_UIManager의 아이템 박스 리스트 < I_Box > 속에서
                //                          [ iBoxCount ] 번째 위치를 가져온다.
                Vector2 iconBoxRT = CSH_UIManager.Instance.I_Box[CSH_UIManager.Instance.iBoxCount];

                // 아이콘의 위치를 [ iBoxCount ] 번째 위치로 바꾼다
                RectTransform selectedIcon = selectedItem.transform.GetComponentInChildren<RectTransform>();
                if (selectedIcon != null)
                {
                    selectedIcon.position = iconBoxRT;
                    selectedIcon.rotation = Quaternion.Euler(0, 0, 0);
                }



                // 부모 옮기기
                if (selectedItem.transform.childCount >= 1)
                {
                    Transform iconT = selectedItem.transform.GetChild(0);
                    if (iconT != null)
                        iconT.SetParent(CSH_UIManager.Instance.item_icons);
                }

                // 아이템 카운터 ++
                CSH_UIManager.Instance.iBoxCount++;


                // Holder에서 이름이 같은 오브젝트 켜기
                for (int i = 0; i < Holder.childCount; i++)
                {
                    if (Holder.GetChild(i).name.Contains(itemName))
                    {
                        Holder.GetChild(i).gameObject.SetActive(true);
                        // 활성화된 리스트에도 넣기--------------------------------- 최초 1번이라서 중복 안될듯
                        activeItems.Add(Holder.GetChild(i).gameObject);
                        // 갖고 있는 아이템 갯수 +1
                        itemSwitch.HolderCount++;
                    }
                    else
                    {
                        Holder.GetChild(i).gameObject.SetActive(false);
                    }
                }

                // 비활성화하기
                selectedItem.SetActive(false);

                // 잡기 탈출
                grabing = false;

                // 퀘스트 Gathering 변수 값 올리기
                if (QuestManager.Instance.quests[1].isActive) QuestManager.Instance.quests[1].goal.ItemCollected();
            }

            // 그냥 [아이템]이라면,
            else
            {
                // 아이템의 csh_itemselect한테  <잡힌 상태> 라고 알려주기
                // => 아웃라인 끄기
                itemSelect.isGrabed = true;

                // 아이템의 rigidbody 물리엔진 끄기
                itemRB.isKinematic = true;


                //itemRB.constraints = RigidbodyConstraints.FreezePosition;

                // [아이템]의 위치를 (this)의 위치로 바꾸기
                Vector3 dir = transform.position - selectedItem.transform.position;
                //selectedItem.transform.position = Vector3.Lerp(selectedItem.transform.position, transform.position, 20 * Time.deltaTime);
                selectedItem.transform.position += dir.normalized * grabSpeed * Time.deltaTime;

                if (dir.magnitude <= 1f)
                {
                    selectedItem.transform.position = transform.position;
                    // [아이템]의 부모를 (this)로 설정하기
                    // = [아이템]을 (this)의 자식으로 가져오기
                    selectedItem.transform.SetParent(transform);

                    // 현재 [아이템]을 갖고 있다!
                    hasItem = true;

                    // 잡기 탈출
                    grabing = false;
                }
            }
        }
    }

    void Spin_item()
    {
#if EDITOR_MODE
            // 마우스 인풋 가져오기
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

#elif VR_MODE
        // 오른 손 조이스틱의 좌표 가져오기
        Vector2 control = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        float mx = control.x;
        float my = control.y;
#endif

        // 마우스 인풋값으로 [아이템] 돌리기
        pointingItem.transform.localEulerAngles += new Vector3(my, -mx);
    }

    void Throw_item()
    {
        // 던지기가 가능한 경우는 모두 [아이템] 이라서 특수인지 아닌지 구분할 필요가 없다
        // ===============================================
        //   아이템의 컴포넌트 가져오기를 또 할 필요는 없다. 
        //       왜냐하면 이미 전역변수로 갖고 있으니까!
        // ===============================================
        // itemSelect = pointingItem.GetComponent<CSH_ItemSelect>();
        // itemRB = pointingItem.GetComponent<Rigidbody>();

        // 아이템의 Rigidbody 물리엔진 켜기
        itemRB.isKinematic = false;
        //itemRB.constraints = RigidbodyConstraints.None;

        // 아이템의 CSH_ItemSelect한테  <안 잡힌 상태>  라고 알려주기
        itemSelect.isGrabed = false;

        // [아이템]의 부모를 [null]로 설정하기
        // => 현재 갖고 있던 자식 비우기
        pointingItem.transform.SetParent(null);

        // 보고 있는 방향의 정면으로 던져버리기!!!!!!
        itemRB.AddForce(Camera.main.transform.forward * throwSpeed, ForceMode.Impulse);

        // 위에서 고정했던 pointingItem을 다시 비워두기!!!
        pointingItem = null;

        // 현재 잡고 있는 [아이템] 없음
        hasItem = false;
    }

    void Show_PressE()
    {
        // 현재 [아이템]을 잡고 있다면, 
        if (hasItem)
        {
            // [E] 안내문 끄기
            pressE.SetActive(false);

            // 살펴보기 안내문 켜기
            Looking.SetActive(true);
        }

        // 현재 [아이템]이 없다면, 
        else
        {
            // 가리키는 [아이템]이 있다면 + 충분히 가깝다면, 텍스트 보여주기
            if (pointingItem && CSH_RayManager.Instance.isNear)
            {
                // [E] 안내문 켜기
                pressE.SetActive(true);

                // 살펴보기 안내문 끄기
                Looking.SetActive(false);
            }

            // 가리키는 [아이템]이 없다면, 텍스트 가리기
            else
            {
                // [E] 안내문 끄기
                pressE.SetActive(false);

                // 살펴보기 안내문 끄기
                Looking.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // " Press E " 띄우기
        Show_PressE();

        // -------------------------------------------------------------------------------< [E] 키를 눌러 아이템 눈 앞으로 가져오기 >
        // 1. 현재 가리키는 아이템이 있고    현재 아이템을 잡고 있지 않고   
        if (!hasItem)
        {
            // Handgun 스크립트 활성화
            if (handGun != null)
                handGun.enabled = true;

            if (pointingItem != null && CSH_RayManager.Instance.isNear)
            {
                // 2. [E] 키를 눌러서 아이템 가져오기
                // VR. 오른손 VR 중지 버튼
#if EDITOR_MODE
                if (Input.GetKeyDown(KeyCode.E))
#elif VR_MODE
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
#endif
                {
                    // 커서로 가리킨 [아이템]을 선택한 [아이템]으로 설정한다
                    selectedItem = pointingItem;
                    pointingItem = null;
                    grabing = true;
                }
            }

            // 잡기 함수 실행!
            Grab_item();
        }

        // 현재 [아이템]을 잡고 있다면
        if (hasItem)
        {
            // Handgun 스크립트 비활성화
            if (handGun != null)
                handGun.enabled = false;

            // 현재 잡고 있는 아이템을 자기 자식으로 가져온 아이템으로 고정하기
            pointingItem = transform.GetChild(0).gameObject;

            //                      :: 이렇게 하는 이유 ::
            //
            //          CSH_ItemSelect 때문에 아이템 위에 커서가 없으면,
            //       아이템을 잡고 있어도, pointingItem이 계속 null이 된다!!!



            // -------------------------------------< 마우스 우클릭을 한 채로 움직이면 아이템 회전하기 >
            // 1. 마우스 우클릭을 지속하는 중이고
            // VR. 오른손 VR 조이스틱
#if EDITOR_MODE
                if (Input.GetMouseButton(1))
#elif VR_MODE
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
#endif
            {
                // 2. 마우스를 움직이면 아이템 회전하기
                Spin_item();

                // 3. 마우스 우클릭 중엔 카메라 회전 안하기
               // if (fpcController != null)
                   // fpcController.hasGrabed = true;
            }
            else
            {
                // 3. 마우스 우클릭 중엔 카메라 회전 안하기
               // if (fpcController != null)
                  //  fpcController.hasGrabed = false;
            }

            // -------------------------------------< 마우스 좌클릭을 하면 아이템 던져버리기!! >
            // 1. 마우스 좌클릭을 했다면,
            // VR. 오른손 VR 검지 버튼
#if EDITOR_MODE
            if (Input.GetMouseButtonDown(0))
#elif VR_MODE
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
#endif
            {
                // 2. 보고 있는 방향으로 아이템 던지기
                Throw_item();
            }
        }
    }
}
