using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;

    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerteData();
    }

    private void GenerteData()
    {
        //questList.Add(10, new QuestData("����Ʈ �̸�", new int[] {1000, 2000}));  //����Ʈ ID / ����Ʈ �̸� / ���� NPC ID �Է�
    }

    public int GetQusetTalkIndex(int id)
    {
        return questList.Count;
    }
}
