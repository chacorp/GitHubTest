using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가구위로 커서를 가져가면 아웃라인을 생성하는 스크립트 만들기
public class FurnitureOutline : MonoBehaviour
{
    // * 에셋스토어에서 다운받은 'Outline에셋'으로 바꾸기 위해 기존에 만든 것은 모두 주석처리했습니다

    // 아웃라인 온/오프 머티리얼
    [Header("Shader")]
    // 새로 다운 받은 아웃라인 에셋 스크립트!
    Outline outliner;

    void Start()
    {
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

    void ShowOutline()
    {
        // CSH_RayManager.Instance.raycastHitObject가 가리키는 오브젝트와   이 오브젝트를 비교해서
        // 서로 갖다면, outline 켜기  
        // 서로 다르다면, outline 끄기
        outliner.enabled = CSH_RayManager.Instance.raycastHitObject == gameObject.transform ? true : false;        
    }

    private void Update()
    {
        ShowOutline();
    }
}
