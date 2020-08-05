using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가구위로 커서를 가져가면 아웃라인을 생성하는 스크립트 만들기
public class FurnitureOutline : MonoBehaviour
{
    // 커서를 [가구]에 가져갔는지 판단하는 변수
    [SerializeField] bool isPointing;

    new Renderer renderer;

    // 아웃라인 온/오프 머티리얼
    [Header("Shader")]
    public Material border;
    public Material nonBorder;
    Material borderMat;
    Material nonBorderMat;

    public float delayTime = 2.0f;
    float currentTime = 0;

    void Start()
    {
        isPointing = false;
        renderer = GetComponent<MeshRenderer>();

        borderMat = new Material(border);
        nonBorderMat = new Material(nonBorder);

        renderer.material = nonBorderMat;
    }

    private void OnMouseOver()
    {
        isPointing = true;

        //if (Input.GetKeyUp(KeyCode.E) && isPointing)
        //{
        //    renderer.material = nonBoarder;
        //    currentTime += Time.deltaTime;

        //    if (currentTime >= delayTime)
        //    {
        //        renderer.material = border;
        //    }
        //}

        if (isPointing)
        {
            renderer.material = borderMat; 
        }

    }

    private void OnMouseExit()
    {
        isPointing = false;
        renderer.material = nonBorderMat;
    }
}
