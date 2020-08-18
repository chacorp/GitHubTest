using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_RayManager : MonoBehaviour
{
    public static CSH_RayManager Instance;

    CSH_RayManager()
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
    public Transform raycastHitObject;

    // 플레이어
    public GameObject player;

    // 레이 맞은 물체와 플레이어와의 거리
    float distance;

    // 레이 맞은 물체와 플레이어 와의 거리가 제한 거리보다 작을때 true / 아니면 false
    public bool isNear = false;

    // 왼손으로 잡아올 포인터 => colider 달려있음(trigger)
    public Transform crossHair;
    public float crossHairScale = 0.1f;
    Vector3 crossHairSize;
    Camera Cam;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        crossHairSize = crossHair.localScale * crossHairScale;
#if VR_MODE
        Cam = CSH_ModeChange.Instance.centerEyeAnchor.GetComponent<Camera>();
#elif EDITOR_MODE
        Cam = CSH_ModeChange.Instance.mainCamera.GetComponent<Camera>();
#endif
    }

    private void RayManager()
    {
        // 플레이어 카메라가 보는 방향으로 레이 쏘기
        Ray ray = new Ray(Cam.transform.position, Cam.transform.forward * rayLength);
        RaycastHit hit;

        // 레이어 마스크             crossHair 레이어                       플레이어의 레이어                  플레이어가 갖고 있는 무기의 레이어
        int layerMask = (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Weapon"));

        // 레이 쏘기
        if (Physics.Raycast(ray, out hit, rayLength, ~layerMask))
        {
            // 1. hit 지점에 crossHair 두기
            crossHair.position = hit.point;
            crossHair.forward = Cam.transform.forward;
            crossHair.localScale = crossHairSize * hit.distance;



            // 2. hit 오브젝트 담아두기
            raycastHitObject = hit.transform;



            // 3. 서로 거리재기
            distance = hit.distance;
            // 3-a. 상호작용 가능한 거리인지 여부 파악하기
            isNear = distance <= distanceLimit ? true : false;



            // 4. hit 오브젝트가 CSH_ItemSelect를 갖고 있는지 여부를 파악해서
            //    갖고 있다면,       CSH_ItemGrab.Instance.pointingItem 에     raycastHitObject.gameObject 넣어두기
            //    안 갖고 있다면,    CSH_ItemGrab.Instance.pointingItem 에     null 넣어두기
            CSH_ItemSelect select = raycastHitObject.GetComponent<CSH_ItemSelect>();

            CSH_ItemGrab.Instance.pointingItem = select ? raycastHitObject.gameObject : null;
        }
    }

    // 에디터에서 Ray 그리기
    private void OnDrawGizmos()
    {
#if VR_MODE
#elif EDITOR_MODE
        Gizmos.color = Color.magenta;
        //Ray ray = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)), Camera.main.transform.forward * rayLength);
        Gizmos.DrawRay(Cam.transform.position, Cam.transform.forward * rayLength);
#endif
    }

    void Update()
    {
        //Ray 관련 모든 것
        RayManager();
    }

}
