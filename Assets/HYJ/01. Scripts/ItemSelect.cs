using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    // 커서를 [아이템]에 올렸냐?
    public bool isGrabed;

    // 이 [아이템]이 [특수 템]인가?
    public bool isSpecialItem;

    new Renderer renderer;
    public Material border;
    public Material nonBorder;

    void Start()
    {
        isGrabed = false;
        renderer = GetComponent<MeshRenderer>();
        renderer.materials[0] = nonBorder;
    }


    void Update()
    {

    }

    private void OnMouseExit()
    {
        isGrabed = false;

    }
}
