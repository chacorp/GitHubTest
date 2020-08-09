using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_Firetorch : MonoBehaviour
{
    public GameObject flame;
    void Start()
    {
        flame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            flame.SetActive(!flame.activeSelf);
        }
    }
}
