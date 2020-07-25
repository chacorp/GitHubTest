using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_ItemSelect : MonoBehaviour
{
    //==============================================
    // 1. 마우스 커서를 아이템에 갖다 대면
    // 2. "[E] 아이템 들기" 라는 문구를 띄우고
    // 3. 아이템에 아웃라인 만들기
    //==============================================


    // 커서를 아이템에 올렸냐?
    public bool isGrabed;

    // 아웃라인 온/오프 머티리얼
    [Header("Shader")]
    public Material border;
    public Material nonBorder;


    private void Start()
    {
        isGrabed = false; 
        GetComponent<Renderer>().material = nonBorder;
    }

    // 아이템 위에 커서를 올리면, 아웃라인 만들기
    private void OnMouseOver()
    {
        // 플레이어가 아이템을 잡고 있는 상태라면, 아웃라인 만들지 않기
        if (isGrabed) { return; }
        GetComponent<Renderer>().material = border;
    }


    // 아이템 위에 커서를 치우면, 아웃라인 없애기
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = nonBorder;
    }

}
