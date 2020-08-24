using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnEffect_1 : MonoBehaviour
{
    public AudioSource btnFx;
    public AudioClip hoverFX;
    public AudioClip clickFX;

    public GameObject restartButton;

    float currentTIme;


    public void HoverSound()
    {
        btnFx.PlayOneShot(hoverFX);
    }

    public void ClickSound()
    {
        btnFx.PlayOneShot(clickFX);
    }

    public void OnMouseEnter()
    {

        iTween.ScaleTo(restartButton, iTween.Hash("scale", Vector3.one * 1.2f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));

    }

    public void OnMouseDown()
    {
        iTween.ScaleTo(restartButton, iTween.Hash("scale", Vector3.one * 1.3f, "time", 0.01f, "easetype", iTween.EaseType.easeInOutBack));
        StartCoroutine(LoadScene());

    }

    public void OnMouseExit()
    {

        iTween.ScaleTo(restartButton, iTween.Hash("scale", Vector3.one, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
