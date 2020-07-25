using UnityEngine;

//==============================================
// 마우스 커서를 아이템에 갖다 대면
// 1. 아이템에 아웃라인 만들기
//==============================================

public class CSH_ItemSelect : MonoBehaviour
{
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
        CSH_ItemGrab.Instance.pointingItem = gameObject;
    }

    // 아이템 위에 커서를 치우면, 아웃라인 없애기
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = nonBorder;
        CSH_ItemGrab.Instance.pointingItem = null;
    }
}
