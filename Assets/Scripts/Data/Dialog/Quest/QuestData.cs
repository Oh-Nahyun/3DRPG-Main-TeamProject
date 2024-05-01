using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    public enum QuestType
    {
        None = 0,
        Hunt,
        GiveItem,
        ClearDungeon
    }

    public QuestType questType;

    /// <summary>
    /// ����Ʈ �̸�
    /// </summary>
    public string questName;
    /// <summary>
    /// ����Ʈ ����/����
    /// </summary>
    public string questContents;
    /// <summary>
    /// ����Ʈ ��ǥ
    /// </summary>
    public string questObjectives;
    public int[] npcId;

    public QuestData(QuestType type, string name, string contents, string objectives)
    {
        QuestInfo(type, name, contents, objectives);
    }

    public void QuestInfo(QuestType type, string name, string contents, string objectives)
    {
        questName = name;
        questContents = contents;
        questObjectives = objectives;

        questType = type;
    }

  
}
