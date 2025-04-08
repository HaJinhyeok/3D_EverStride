using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    public Text QuestText;
    public static Action OnQuestPanelUpdate;

    private void Awake()
    {
        OnQuestPanelUpdate += QuestPanelUpdate;
        QuestPanelUpdate();
    }

    private void OnEnable()
    {
        QuestPanelUpdate();
    }

    public void QuestPanelUpdate()
    {
        QuestText.text = "";
        if (GameManager.Instance.QuestDictionary.Count == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        foreach (KeyValuePair<Define.QuestName, Quest> quest in GameManager.Instance.QuestDictionary)
        {
            QuestText.text += $"{quest.Value.context} - ( {quest.Value.currentNum} / {quest.Value.requireNum} )\n";
        }
    }

    private void OnDestroy()
    {
        OnQuestPanelUpdate -= QuestPanelUpdate;
    }
}
