using UnityEngine;

public class CSH_Firetorch : MonoBehaviour
{
    public GameObject flame;
    public AudioSource torchSound;
    public AudioClip torchClip;

    void Start()
    {
        //soundOn = false;
        flame.SetActive(false);

        torchSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 마우스 좌클릭을 하면 && 아이템이 없을때
        if (Input.GetMouseButtonDown(0) && !CSH_ItemGrab.Instance.hasItem)
        {
            // 불 뿜기 / 안 뿜기
            flame.SetActive(!flame.activeSelf);

            // 불이 나오는 상태라면 소리켜기
            if (flame.activeSelf) torchSound.PlayOneShot(torchClip);

            // 불이 안나올땐 소리 끄기
            else torchSound.Stop();
        }

    }
}
