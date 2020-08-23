using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가구위로 커서를 가져가면 아웃라인을 생성하는 스크립트 만들기
public class FurnitureOutlineSe : MonoBehaviour
{
    // 커서를 [가구]에 가져갔는지 판단하는 변수
    public bool isPointing;

    new Renderer renderer;

    // 아웃라인 온/오프 머티리얼
    [Header("Shader")]
    public Material border;
    public Material nonBoarder;

    public float delayTime = 2.0f;
    float currentTime = 0;

    void Start()
    {
        isPointing = false;
        renderer = GetComponentInChildren<Renderer>();
        renderer.material = nonBoarder;
    }

    private void OnMouseOver()
    {
        isPointing = true;
         
        if (Input.GetKeyUp(KeyCode.E) && isPointing)
        {
            renderer.material = nonBoarder;
            currentTime += Time.deltaTime;

            if (currentTime >= delayTime)
            {
                renderer.material = border;
            }
        }
        renderer.material = border;

    }

    private void OnMouseExit()
    {
        renderer.material = nonBoarder;
    }
}
