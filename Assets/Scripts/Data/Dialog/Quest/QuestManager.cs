using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questList = new Dictionary<int, QuestData>();

    private QuestMessage questMessage;

    private void Awake()
    {
        questMessage = FindObjectOfType<QuestMessage>();
        GenerateData();
    }

    private void GenerateData()
    {
        questList.Add(0, new QuestData(QuestData.QuestType.None, "����Ʈ �̸�", "����Ʈ ����", "����Ʈ ��ǥ"));
        questList.Add(10, new QuestData(QuestData.QuestType.Hunt, "����Ʈ ���", "����Ʈ ���� ���", "����Ʈ ��ǥ 10����"));
    }

    public void GetQuestTalkIndex(int id, bool complete)
    {
        if (questList.ContainsKey(id))
        {
            string questName = questList[id].questName;
            questMessage.OnQuestMessage(questName, complete);
        }
    }
}
