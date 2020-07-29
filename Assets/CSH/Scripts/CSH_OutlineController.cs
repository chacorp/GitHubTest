using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_OutlineController : MonoBehaviour
{
    MeshRenderer rdrr;
    public float maxOutlineWidth;
    public Color OutlineColor;

    void Start()
    {
    rdrr = GetComponent<MeshRenderer>();    
    }

    public void ShowOutline()
    {
        rdrr.material.SetFloat("_Outline", maxOutlineWidth);
        rdrr.material.SetColor("_OutlineColor", OutlineColor);
    }

    public void HideOutline()
    {
        rdrr.material.SetFloat("_Outline", 0f);
    }

}
