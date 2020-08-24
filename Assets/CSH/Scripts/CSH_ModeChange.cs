using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_ModeChange : MonoBehaviour
{
    public static CSH_ModeChange Instance;
    CSH_ModeChange()
    {
        Instance = this;
    }

    [Header("OVR")]    // ----------------------------< VR 사용시 활성화 >
    public GameObject OVRCamera;
    // OVR 위치들
    public Transform centerEyeAnchor;
    public Transform leftControllerAnchor;
    public Transform rightControllerAnchor;

    public Transform crossHair_R;

    [Header("Editor")] // ----------------------------< VR 사용시 비활성화 >
    public GameObject mainCamera;

    [Header("Objects")]
    public Transform WeaponCamera;
    public Transform itemGrab;
    public Transform Holder;
    public Transform playerInventroy;

    public Transform inventory_UICanvas;

    private void Awake()
    {

#if VR_MODE
        // OVRCamera 활성화
        OVRCamera.SetActive(true);

        // Editor의 MainCamera 비활성화
        mainCamera.SetActive(false);


        // 위치 이동
        // 카메라
        WeaponCamera.position = centerEyeAnchor.position;
        WeaponCamera.SetParent(centerEyeAnchor);
        // VR에선 카메라 두개 운용이 안되는 듯!
        WeaponCamera.gameObject.SetActive(false);

        playerInventroy.position = centerEyeAnchor.position;
        playerInventroy.SetParent(centerEyeAnchor);
        
        // 왼손
        itemGrab.position = leftControllerAnchor.position;
        itemGrab.SetParent(leftControllerAnchor);

        // 오른손
        Holder.position = rightControllerAnchor.position;
        Holder.SetParent(rightControllerAnchor);

        inventory_UICanvas.SetParent(crossHair_R);

#elif EDITOR_MODE

        // OVRCamera 비활성화
        OVRCamera.SetActive(false);
        // EditorCamera 활성화
        mainCamera.SetActive(true);
        // 위치 이동
        WeaponCamera.SetParent(mainCamera.transform);
        itemGrab.SetParent(mainCamera.transform);
        Holder.SetParent(mainCamera.transform);
        playerInventroy.SetParent(mainCamera.transform);


        inventory_UICanvas.SetParent(mainCamera.transform);
        inventory_UICanvas.transform.localPosition = new Vector3(0, 0, 10f);
#endif
    }
}
