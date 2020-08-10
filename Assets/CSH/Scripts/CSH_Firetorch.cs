using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_Firetorch : MonoBehaviour
{
    public GameObject flame;
    public AudioSource torchSound;
    public AudioClip torchClip;


    //bool soundOn;

    void Start()
    {
        //soundOn = false;
        flame.SetActive(false);

        torchSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 마우스 좌클릭을 하면
        if (Input.GetMouseButtonDown(0))
        {
            // 불 뿜기 / 안 뿜기
            flame.SetActive(!flame.activeSelf);
            if (flame.activeSelf) torchSound.PlayOneShot(torchClip);
            else torchSound.Stop();
            // 소리 켜기 / 끄기
            //soundOn = flame.activeSelf;
        }

    }
}
