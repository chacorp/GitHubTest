using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가구위로 커서를 가져가면 아웃라인을 생성하는 스크립트 만들기
public class FurnitureOutline : MonoBehaviour
{
    // * 에셋스토어에서 다운받은 'Outline에셋'으로 바꾸기 위해 기존에 만든 것은 모두 주석처리했습니다


    // 커서를 [가구]에 가져갔는지 판단하는 변수
    //[SerializeField] bool isPointing;

    //new Renderer renderer;

    // 아웃라인 온/오프 머티리얼
    [Header("Shader")]
    //public Material border;
    //public Material nonBorder;
    //Material borderMat;
    //Material nonBorderMat;

    // 새로 다운 받은 아웃라인 에셋 스크립트!
    Outline outliner;

    // 사용되지 않는 변수들이라서 주석처리 했습니다.
    // public float delayTime = 2.0f;
    // float currentTime = 0;

    void Start()
    {
        // 굳이 불리언을 추가 하지 않아도 되어서 주석처리했습니다.
        //isPointing = false;

        //renderer = GetComponent<MeshRenderer>();

        //borderMat = new Material(border);
        //nonBorderMat = new Material(nonBorder);

        //renderer.material = nonBorderMat;

        // ----------------------------------------------------------< 추가한 부분 - 1 > - 시작
        // Start()가 실행되는 순간,
        // 게임 오브젝트에 아웃라인 에셋에서 다운받은 스크립트 추가하기
        // ** 이렇게 하는 이유는 어떤 오브젝트에 FurnitureOutline이 붙어있는지 모두 찾아내기 어려워서 그렇습니다.
        gameObject.AddComponent<Outline>();

        // + 아웃라인 에셋 스크립트에 설정 넣어주기
        outliner = GetComponent<Outline>();
        outliner.OutlineColor = new Color(0.3f, 1f, 0.3f);
        outliner.OutlineWidth = 8f;

        // 아웃라인 에셋 기본으로 꺼놓기
        outliner.enabled = false;
        // ----------------------------------------------------------< 추가한 부분 - 1 > - 끝
    }

    // *** OnMouseOver() 는 마우스 커서가 사물을 지속적으로 가리키고 있을때마다 계속 호출!
    // **** OnMouseEnter() 는 마우스 커서가 사물을 가리킨 순간 한번 호출! 
    //        =>     void OnMouseOver()    >>>>    void OnMouseEnter() 로 바꿨습니다


    //private void OnMouseEnter()
    //{
    //    //isPointing = true;

    //    //if (Input.GetKeyUp(KeyCode.E) && isPointing)
    //    //{
    //    //    renderer.material = nonBoarder;
    //    //    currentTime += Time.deltaTime;

    //    //    if (currentTime >= delayTime)
    //    //    {
    //    //        renderer.material = border;
    //    //    }
    //    //}

    //    //if (isPointing)
    //    //{
    //    //    renderer.material = borderMat; 
    //    //}


    //    // ----------------------------------------------------------< 추가한 부분 - 2 > - 시작
    //    outliner.enabled = true;
    //    // ----------------------------------------------------------< 추가한 부분 - 2 > - 끝
    //}

    //private void OnMouseExit()
    //{
    //    //isPointing = false;
    //    //renderer.material = nonBorderMat;


    //    // ----------------------------------------------------------< 추가한 부분 - 3 > - 시작
    //    outliner.enabled = false;
    //    // ----------------------------------------------------------< 추가한 부분 - 3 > - 끝
    //}

    // ----------------------------------------------------------< 추가한 부분 - 4 > - 시작
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EditorOnly"))
        {
            // 플레이어와의 거리재기
            float distance = Vector3.Distance(CSH_RayManager.Instance.player.transform.position, transform.position);
            // 거리가 3m 이내라면
            if (distance <= CSH_RayManager.Instance.distanceLimit) outliner.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EditorOnly")) outliner.enabled = false;
    }
    // ----------------------------------------------------------< 추가한 부분 - 4 > - 끝
}
