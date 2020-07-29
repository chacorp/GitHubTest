using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CSH_SceneManager : MonoBehaviour
{
    public Image fadeOut;
    private void Start()
    {
        fadeOut.gameObject.SetActive(false);
    }

    public void OnButtonClicked()
    {
        fadeOut.gameObject.SetActive(true);
        for (int i = 0; i < 256; i++)
        {
            fadeOut.color = new Color(0, 0, 0, i * Time.deltaTime);
        }
        SceneManager.LoadScene(1);
    }
}
