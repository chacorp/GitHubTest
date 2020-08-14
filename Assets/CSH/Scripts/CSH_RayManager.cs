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
    public GameObject player;

    // 왼손으로 잡아올 포인터 => colider 달려있음(trigger)
    public Transform crossHair;
    public float crossHairScale = 0.1f;
    Vector3 crossHairSize;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        crossHairSize = crossHair.localScale * crossHairScale;
    }

    private void RayManager()
    {
        // 플레이어 카메라가 보는 방향으로 레이 쏘기
        Ray ray = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)), Camera.main.transform.forward * rayLength);
        RaycastHit hit;

        // 레이어 마스크             crossHair 레이어                       플레이어의 레이어                  플레이어가 갖고 있는 무기의 레이어
        int layerMask = (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Weapon"));

        // 레이 쏘기
        if (Physics.Raycast(ray, out hit, rayLength, ~layerMask))
        {
            // hit 지점에 crossHair 두기
            crossHair.position = hit.point;
            crossHair.forward = Camera.main.transform.forward;
            crossHair.localScale = crossHairSize * hit.distance;

            // hit 오브젝트 담아두기
            raycastHitObject = hit.transform;

            // hit 오브젝트의 컴포넌트 CSH_ItemSelect 가져오기
            CSH_ItemSelect sel = raycastHitObject.GetComponent<CSH_ItemSelect>();

            // 만약 가져올 수 있다면
            if (sel) CSH_ItemGrab.Instance.pointingItem = raycastHitObject.gameObject;

            // 만약 못 가져온다면
            else CSH_ItemGrab.Instance.pointingItem = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.forward);
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayLength);
    }

    void Update()
    {
        RayManager();
    }

}
