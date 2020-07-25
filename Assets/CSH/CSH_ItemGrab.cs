using UnityEngine;

// ===========================================================================
// 마우스 커서를 아이템에 갖다 대면,
// 1. [E] 키를 누르면 아이템 눈 앞으로 가져오기 ------------------------------[O]

// 아이템을 가져온 상태에서,
// 1. 마우스 우클릭을 하고 마우스를 움직이면 아이템 회전하기 ------------------[ ]
// 2. 마우스 좌클릭을 하면 아이템 던져버리기!! -------------------------------[O]
// ===========================================================================

public class CSH_ItemGrab : MonoBehaviour
{
    public static CSH_ItemGrab Instance;
    CSH_ItemGrab()
    {
        Instance = this;
    }

    // 현재 마우스 커서로 가리키고 있는 아이템
    public GameObject pointingItem;

    // 아이템의 컴포넌트를 담아둘 전역변수
    CSH_ItemSelect itemSelect;
    Rigidbody itemRB;

    // 아이템을 던져버릴 속도
    public float throwSpeed = 200f;

    // 현재 아이템을 잡고 있나?
    bool hasItem;

    private void Start()
    {
        hasItem = false;
    }

    private void Update()
    {
        if (pointingItem && !hasItem)
        {
            // -------------------------------------< [E] 키를 누르면 아이템 눈 앞으로 가져오기 >
            // 1. 커서로 가리키는 아이템이 있고,
            // 2. 현재 아이템을 잡고 있지 않다면,
            // 3. [E] 키를 눌러서 아이템 가져오기

            if (Input.GetKeyDown(KeyCode.E))
            {
                // [아이템]의 위치를 [아이템그랩]의 위치로 바꾸기
                pointingItem.transform.position = transform.position;

                // [아이템]의 부모를 [아이템그랩]으로 설정하기
                pointingItem.transform.SetParent(transform);

                // 아이템의 컴포넌트 전역변수에 넣어두기
                itemSelect = pointingItem.GetComponent<CSH_ItemSelect>();
                itemRB = pointingItem.GetComponent<Rigidbody>();

                // 아이템의 CSH_ItemSelect한테  <잡힌 상태> 라고 알려주기
                itemSelect.isGrabed = true;

                // 아이템의 Rigidbody 물리엔진 끄기
                itemRB.isKinematic = true;


                hasItem = true;
            }
        }


        // 현재 아이템을 잡고 있다!!
        if (hasItem)
        {
            // 현재 잡고 있는 아이템을 자기 자식으로 가져온 아이템으로 고정하기
            pointingItem = transform.GetChild(0).gameObject;
            // 이렇게 하는 이유 ()
            // {
            //   CSH_ItemSelect 때문에 아이템 위에 커서가 없으면,
            //   아이템을 잡고 있음에도, pointingItem이 계속 null이 떠버린다!!!
            // }


            // -------------------------------------< 마우스 우클릭을 한 채로 움직이면 아이템 회전하기 >
            // 1. 마우스 우클릭을 지속하는 중이고
            // 2. 마우스를 움직이면 아이템 회전하기
            if (Input.GetMouseButton(1))
            {
                float mx = Input.GetAxis("Mouse X");
                float my = Input.GetAxis("Mouse Y");

                pointingItem.transform.localEulerAngles += new Vector3(my, -mx, 0);
            }


            // -------------------------------------< 마우스 좌클릭을 하면 아이템 던져버리기!! >
            // 1. 마우스 좌클릭을 했다면,
            // 2. 보고 있는 방향으로 아이템 던지기


            if (Input.GetMouseButtonDown(0))
            {
                // ===============================================
                //   아이템의 컴포넌트 가져오기를 또 할 필요는 없다. 
                //       왜냐하면 이미 전역변수로 갖고 있으니까!
                // ===============================================
                // itemSelect = pointingItem.GetComponent<CSH_ItemSelect>();
                // itemRB = pointingItem.GetComponent<Rigidbody>();

                // 아이템의 Rigidbody 물리엔진 켜기
                itemRB.isKinematic = false;

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
        }
    }
}
