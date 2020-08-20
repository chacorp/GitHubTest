using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 1. 버튼을 누르면 
// 2. FADE OUT 하고
// 3. Scene 바꾸기

public class CSH_SceneManager : MonoBehaviour
{
    // 화면을 검게 만들 암막 이미지
    public Image fadeOutImage;

    // FADE OUT 여부
    bool FADEOUT = false;

    // 알파 값
    float alphaC = 1;

    // FADE OUT 속도
    public float fadeSpeed = 10f;

    public GameObject enndingText;


    float timer = 0;
    bool start = false;
    public void OnButtonClicked()
    {
        // 버튼 누르면 FADE OUT 하기
        FADEOUT = true;
    }

    private void Start()
    {
        fadeOutImage.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!start)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                start = true;
                timer = 0;
            }
        }

        if (start)
        {
            // FADE OUT일때 화면 검게하기
            if (FADEOUT)
            {
                fadeOutImage.gameObject.SetActive(true);
                alphaC += Time.deltaTime * fadeSpeed;
                if (alphaC >= 1f)
                {
                    alphaC = 1;

                    // 완전히 까매지면 
                    // Scene 바꾸기
                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                alphaC -= Time.deltaTime * fadeSpeed;
                if (alphaC <= 0)
                {
                    alphaC = 0;
                    fadeOutImage.gameObject.SetActive(false);
                }
            }

            // 화면 암막 색상
            fadeOutImage.color = new Color(0, 0, 0, alphaC);
        }

        if (QuestManager.Instance.quests[3].goal.IsReached())
        {
            fadeOutImage.gameObject.SetActive(true);
            alphaC += Time.deltaTime * fadeSpeed;
            if (alphaC >= 0.5f) alphaC = 0.5f;
            enndingText.SetActive(true);
        }
    }
}
