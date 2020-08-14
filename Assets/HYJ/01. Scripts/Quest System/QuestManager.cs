using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;

    public Text[] titles;
    public Text[] descriptionText;
    public Text[] amountText;

    private void Start()
    {
        ShowingQuest(quests[0], 0);
        ShowingQuest(quests[1], 1);
        ShowingQuest(quests[2], 2);
    }

    private void Update()
    {

    }

    void ShowingQuest(Quest quest, int index)
    {
        titles[index].text = quest.title;
        descriptionText[index].text = quest.description;
        Debug.Log($"{quest}[{index}] is on Clipboard ");

    }
}
