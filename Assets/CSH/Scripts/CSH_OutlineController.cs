using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_OutlineController : MonoBehaviour
{
    MeshRenderer renderer;
    public float maxOutlineWidth;
    public Color OutlineColor;

    void Start()
    {
    renderer = GetComponent<MeshRenderer>();    
    }

    public void ShowOutline()
    {
        renderer.material.SetFloat("_Outline", maxOutlineWidth);
        renderer.material.SetColor("_OutlineColor", OutlineColor);
    }

    public void HideOutline()
    {
        renderer.material.SetFloat("_Outline", 0f);
    }

}
