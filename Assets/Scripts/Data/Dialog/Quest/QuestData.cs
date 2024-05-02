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
    /// ����Ʈ ��ǥ ����
    /// </summary>
    public string questObjectivesText;

    /// <summary>
    /// ����Ʈ ��ǥ ���� Ƚ��
    /// </summary>
    public int questObjectivesCount;

    /// <summary>
    /// ����Ʈ ��ǥ ���� ������Ʈ
    /// </summary>
    public GameObject questObject;

    public QuestData(QuestType type, string name, string contents, string objectives, int count, GameObject gameObject)
    {
        QuestInfo(type, name, contents, objectives, count, gameObject);
    }

    public void QuestInfo(QuestType type, string name, string contents, string objectives, int count, GameObject gameObject)
    {
        questName = name;
        questContents = contents;
        questObjectivesText = objectives;
        questType = type;
        questObjectivesCount = count;
        questObject = gameObject;
    }

  
}
