using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionScene : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene_Betatype0811");
    }
}
