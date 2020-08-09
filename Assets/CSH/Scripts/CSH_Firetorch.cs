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
        if (Input.GetMouseButtonDown(0))
        {
            flame.SetActive(!flame.activeSelf);
            soundOn = flame.activeSelf;
        }
        if(soundOn)
        {
            torchSound.Play();
        }else
        {
            torchSound.Stop();
        }
    }
}
