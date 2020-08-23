using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> quests;

    public Text[] titles;
    public Text[] descriptionText;
    public Text amountText;
    public GameObject[] checkmarks;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        ShowingQuest(quests[0], 0);
        quests[0].isActive = true;
        quests[1].isActive = false;
        quests[2].isActive = false;
        quests[3].isActive = false;
    }

    private void Update()
    {
        if (quests[0].goal.IsReached())
        {
            checkmarks[0].SetActive(true);
            ShowingQuest(quests[1], 1);
            quests[1].isActive = true;
        }

        if (quests[1].goal.IsReached())
        {
            checkmarks[1].SetActive(true);
            ShowingQuest(quests[2], 2);
            quests[2].isActive = true;
        }

        if (quests[2].goal.IsReached())
        {
            checkmarks[2].SetActive(true);
            ShowingQuest(quests[3], 3);
            quests[3].isActive = true;
        }

        if (quests[3].goal.IsReached())
        {
            checkmarks[3].SetActive(true);

        }
        amountText.text = quests[3].goal.currentAmount.ToString("0" + "마리 처치");

    }

    void ShowingQuest(Quest quest, int index)
    {   // 메소드가 업데이트에서 반복 실행하는 것을 방지하기 위해서 퀘스트가 활성화되기 전에 한 번 실행될 수 있게 함.    
        if (!quest.isActive)
        {
            Debug.Log($"{quest}[{index}] is on Clipboard ");
            titles[index].text = quest.title;
            descriptionText[index].text = quest.description;
            descriptionText[index].gameObject.SetActive(true);
            checkmarks[index].SetActive(false);

            if (index == 3) amountText.gameObject.SetActive(true);
        }
        else if (quest.isActive) return;
    }
}
