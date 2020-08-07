using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;

    public bool isActive; // 현재 퀘스트가 진행중인지 판별하는 변수

    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
        Debug.Log(title + " was completed!!");
    }
}
