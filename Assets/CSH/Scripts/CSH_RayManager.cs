using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_RayManager : MonoBehaviour
{
    public static CSH_RayManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // ==================================<< 주의 사항 >>==================================
    // 레이가 Trigger를 무시하도록 설정됨!!!
    // 다시 바꾸려면
    //        Edit > Project Settings > Physics > Uncheck "Queries Hit Triggers"
    // 이 경로로 들어가서 변경하기!
    // ==================================================================================

    // 레이 길이
    float rayLength = 200f;

    public float distanceLimit = 3f;

    // 레이 맞은 물체
    public Transform raycastHitObject_R;
    public Transform raycastHitObject_L;

    // 플레이어
    public GameObject player;

    // 레이 맞은 물체와 플레이어와의 거리
    float distance;

    // 레이 맞은 물체와 플레이어 와의 거리가 제한 거리보다 작을때 true / 아니면 false
    public bool isNear = false;

    // 왼손으로 잡아올 포인터 => colider 달려있음(trigger)
    public Transform crossHair_R;
    public Transform crossHair_L;
    public float crossHairScale = 0.1f;
    Vector3 crossHairSize;
    public Camera Cam;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        crossHairSize = new Vector3(0.1f, 0.1f, 0.1f);
        // 카메라 가져오기


#if VR_MODE
        Cam = CSH_ModeChange.Instance.centerEyeAnchor.GetComponent<Camera>();
#elif EDITOR_MODE
        if (!crossHair_L)
        {
            Debug.Log("크로스 헤어 L 연결 안됨  && 못 가져옴");
        }
        else 
        { 
            Debug.Log("크로스 헤어 L 연결 됨 ㅎㅎ");
            crossHair_L.gameObject.SetActive(false);
        }
        
        if (!crossHair_R)
        {
            Debug.Log("크로스 헤어 R 연결 안됨  && 못 가져옴");
        }
        else
        {
            Debug.Log("크로스 헤어 R 연결 됨 ㅎㅎ");

        }
#endif
    }

    // 왼손 트리거의 조준점
    private void RayManager_L()
    {

#if VR_MODE
        Ray ray = new Ray(CSH_ModeChange.Instance.leftControllerAnchor.position, CSH_ModeChange.Instance.leftControllerAnchor.forward * rayLength);
#elif EDITOR_MODE

        Ray ray = new Ray(Cam.transform.position, Cam.transform.forward * rayLength);
#endif
        RaycastHit hit_L;

        // 레이어 마스크             crossHair 레이어                       플레이어의 레이어                  플레이어가 갖고 있는 무기의 레이어
        int layerMask = (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Weapon"));

        if (Physics.Raycast(ray, out hit_L, rayLength, ~layerMask))
        {
            // 1. hit 지점에 crossHair 두기
            crossHair_L.position = hit_L.point;
            crossHair_L.forward = Cam.transform.forward;
            crossHair_L.localScale = crossHairSize * hit_L.distance;



            // 2. hit 오브젝트 담아두기
            raycastHitObject_L = hit_L.transform;



            // 3. 서로 거리재기
            distance = hit_L.distance;
            // 3-a. 상호작용 가능한 거리인지 여부 파악하기
            isNear = distance <= distanceLimit ? true : false;



            // 4. hit 오브젝트가 특수 아이템인지 파악하기

            // CSH_ItemSelect 가져오기
            CSH_ItemSelect select = raycastHitObject_L.GetComponent<CSH_ItemSelect>();
            // CSH_ItemSelect 가져올 수 있을때,
            if (select)
            {
                // isSpecialItem으로 [특수 템] 여부 확인하기 CSH_ItemGrab.Instance.pointingItem_L 에 보내기
                //    특수 맞다       null 
                //    특수 아니다     raycastHitObject_L.gameObject
                CSH_ItemGrab.Instance.pointingItem_L = select.isSpecialItem ? null : raycastHitObject_L.gameObject;
            }
            // 못 가져오면 암것도 안 보냄
            else
            {
                CSH_ItemGrab.Instance.pointingItem_L = null;
            }
        }
    }

    //오른손 트리거의 조준점
    private void RayManager_R()
    {
        // 플레이어 카메라가 보는 방향으로 레이 쏘기

#if VR_MODE
        Ray ray = new Ray(CSH_ModeChange.Instance.rightControllerAnchor.position, CSH_ModeChange.Instance.rightControllerAnchor.forward * rayLength);
#elif EDITOR_MODE

        //Ray ray = new Ray(Cam.transform.position, Cam.transform.forward * rayLength);
#endif
        RaycastHit hit;

        // 레이어 마스크             crossHair 레이어                       플레이어의 레이어                  플레이어가 갖고 있는 무기의 레이어
        int layerMask = (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Weapon"));

        // 레이 쏘기
        //if (Physics.Raycast(ray, out hit, rayLength, ~layerMask))
        if(Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, rayLength, ~layerMask))
        {
            // 1. hit 지점에 crossHair 두기
            crossHair_R.position = hit.point;
            crossHair_R.forward = Cam.transform.forward;
            crossHair_R.localScale = crossHairSize * hit.distance;



            // 2. hit 오브젝트 담아두기
            raycastHitObject_R = hit.transform;



            // 3. 서로 거리재기
            distance = hit.distance;
            // 3-a. 상호작용 가능한 거리인지 여부 파악하기
            isNear = distance <= distanceLimit ? true : false;



            // 4. hit 오브젝트가 특수 아이템인지 파악하기

            // CSH_ItemSelect 가져오기
            CSH_ItemSelect select = raycastHitObject_R.GetComponent<CSH_ItemSelect>();
            // CSH_ItemSelect 가져올 수 있을때,
            if (select)
            {
                // isSpecialItem으로 [특수 템] 여부 확인해서 CSH_ItemGrab.Instance.pointingItem_R 에 보내기
                //    특수 맞다       raycastHitObject_R.gameObject
                //    특수 아니다     null 
                CSH_ItemGrab.Instance.pointingItem_R = select.isSpecialItem ? raycastHitObject_R.gameObject : null;
            }
            // 못 가져오면 암것도 안 보냄
            else
            {
                CSH_ItemGrab.Instance.pointingItem_R = null;
            }
        }
    }

#if VR_MODE
#elif EDITOR_MODE
    // 에디터에서 Ray 그리기
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    //Ray ray = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)), Camera.main.transform.forward * rayLength);
    //    Gizmos.DrawRay(Cam.transform.position, Cam.transform.forward * rayLength);
    //}
#endif

    void Update()
    {
        //Ray 관련 모든 것
        RayManager_R();
        RayManager_L();
    }

}
