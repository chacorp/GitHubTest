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
        inventory_UICanvas.localPosition = new Vector3(0, 0, 10f);
    }
}
