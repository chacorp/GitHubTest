using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CSH_Firetorch : MonoBehaviour
{
    [Header("Object Properties")]
    // 불꽃 오브젝트
    public GameObject flame;

    // 불꽃 사운드
    public AudioSource torchSound;
    public AudioClip torchClip;
    float torchVolume = 0;

    // 불꽃 크기조절용 스크립트
    public CSH_Flame flameControl;

    // 불꽃 사이즈 조절 속도
    public float flameSpeed = 20f;
    public float maxFlameSize = 1f;

    // 불꽃 발사!!!!!
    public bool flameOn = false;
    float timer;

    // 토치 UI
    [System.Serializable]
    public class torch_UI
    {
        [Header("UI Properties")]
        public Text totalAmmo;
        public Text currentAmmo;
    }
    public torch_UI torchUI;

    float ammoTime;
    float ammo = 100f;


    void Start()
    {
        flame.SetActive(false);

        torchSound = GetComponent<AudioSource>();
    }


    void Update()
    {
        // 토치 소리 크기
        torchSound.volume = torchVolume;

        // 마우스 좌클릭을 하면 && 아이템이 없을때
#if VR_MODE
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && !CSH_ItemGrab.Instance.hasItem)
#elif EDITOR_MODE
        if (Input.GetMouseButtonDown(0) && !CSH_ItemGrab.Instance.hasItem && ammo > 0)
#endif
        {
            // 불 뿜기 / 안 뿜기
            flameOn = !flameOn;

            // 불꼴 사운드 켜기 / 끄기
            if (flameOn)
                torchSound.PlayOneShot(torchClip);
        }
        // 불이 나오는 상태라면 소리켜기
        if (flameOn)
        {
            // A. 불꽃 켜기
            flame.SetActive(true);

            // B. 소리 점점 키우기
            torchVolume += Time.deltaTime;
            if (torchVolume >= 0.5f)
            {
                torchVolume = 0.5f;
            }

            // 연료가 없다면 불 X
            if (ammo <= 0)
            {
                flameOn = false;
                return;
            }

            // C. 불꽃 키우기
            flameControl.flameSize += flameSpeed * Time.deltaTime;
            if (flameControl.flameSize >= maxFlameSize)
            {
                flameControl.flameSize = maxFlameSize;
            }



            // D. 연료 소비하기
            ammoTime += Time.deltaTime;
            if (ammoTime > 0.25f)
            {
                ammo--;
                torchUI.currentAmmo.text = ammo.ToString();
                ammoTime = 0;
            }
        }

        // 불이 안나올땐 소리 끄기
        else
        {
            // B. 소리 점점 줄이기
            torchVolume -= Time.deltaTime;
            if (torchVolume <= 0)
            {
                torchVolume = 0;
            }


            // C. 불꽃 줄이기
            flameControl.flameSize -= Time.deltaTime;
            if (flameControl.flameSize <= 0)
            {
                flameControl.flameSize = 0;

                // 불꽃이 켜져 있다면,
                if (flame.activeSelf)
                {
                    timer += Time.deltaTime;
                    if (timer >= 0.75f)
                    {
                        // A. 불꽃 끄기
                        flame.SetActive(false);

                        torchSound.Stop();
                        timer = 0;
                    }
                }
            }
        }
    }
}
