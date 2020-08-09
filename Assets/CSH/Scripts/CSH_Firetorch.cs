using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_Firetorch : MonoBehaviour
{
    public GameObject flame;
    public AudioSource torchSound;

    bool soundOn;

    void Start()
    {
        soundOn = false;
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
            // 소리 켜기 / 끄기
            soundOn = flame.activeSelf;
        }

        if (soundOn) torchSound.Play();
        else torchSound.Stop();

    }
}
