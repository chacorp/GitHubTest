using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;

    public PlayerPrefs player;

    public Text titleText;
    public Text descriptionText;
    public Text amountText;
}
