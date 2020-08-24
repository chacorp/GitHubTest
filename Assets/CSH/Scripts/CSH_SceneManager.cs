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

    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject exitButton;
    // FADE OUT 여부
    bool FADEOUT = false;

    // 알파 값
    float alphaC = 1;

    // FADE OUT 속도
    public float fadeSpeed = 10f;

    public GameObject enndingText;


    float timer = 0;
    bool start = false;
    bool IsGameFinished = false;

    public void OnButtonClicked()
    {
        // 버튼 누르면 FADE OUT 하기
        FADEOUT = true;
    }

    private void Start()
    {
        Time.timeScale = 1;
        fadeOutImage.gameObject.SetActive(true);
        restartButton.SetActive(false);
        exitButton.SetActive(false);
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

        if (start && !QuestManager.Instance.quests[3].goal.IsReached())
        {
            FadeOutStart();
        }

        else if (QuestManager.Instance.quests[3].goal.IsReached())
        {
            StartCoroutine(FinishGame());

        }
    }

    void FadeOutStart()
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

    // 게임 엔딩씬을 만들고 싶다
    IEnumerator FinishGame()
    {
        if (!IsGameFinished)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            // 약간의 슬로우 모션 효과를 부여
            Time.timeScale = 1.0f;
            // 3초간의 지연시간을 통해 슬로우 모션을 플레이어가 체감할 수 있도록 함.
            yield return new WaitForSecondsRealtime(1.8f);

            fadeOutImage.gameObject.SetActive(true);
            fadeOutImage.color = new Color(0, 0, 0, alphaC);
            enndingText.SetActive(true);
            restartButton.SetActive(true);
            exitButton.SetActive(true);
            iTween.ScaleTo(restartButton, iTween.Hash("scale", Vector3.one, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
            iTween.ScaleTo(exitButton, iTween.Hash("scale", Vector3.one, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));


            alphaC += Time.deltaTime * fadeSpeed;
            if (alphaC >= 0.5f)
            {
                alphaC = 0.5f;
                Time.timeScale = 1.0f;
            }

            IsGameFinished = true;

            yield return null;
        }
    }


    void ButtonAnim()
    {



        //iTween.ScaleTo(
        //   exitButton,
        //   iTween.Hash(
        //       "scale", Vector3.one,
        //       "time", 0.5f,
        //       "easetype", iTween.EaseType.easeInOutBack
        //       )
        //   );
    }


}
