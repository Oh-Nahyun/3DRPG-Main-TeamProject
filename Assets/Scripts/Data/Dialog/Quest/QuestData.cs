using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
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

        switch (type)
        {
            case QuestType.None:
                break;
            case QuestType.Hunt:
                HuntQuest(1, 10); // ���÷� �ּ� 1 �������� �ִ� 10 ������ ����ؾ� �ϴ� ����Ʈ�� ����
                break;
            case QuestType.GiveItem:
                GiveItemQuest();
                break;
            case QuestType.ClearDungeon:
                ClearDungeonQuest();
                break;
            default:
                break;
        }
    }

    public void HuntQuest(int currentKill, int maxKill)
    {
        int killCount = maxKill - currentKill;
        Debug.Log("Remaining enemies to kill: " + killCount);
    }

    public void GiveItemQuest()
    {
        // Give item quest logic
    }

    public void ClearDungeonQuest()
    {
        // Clear dungeon quest logic
    }
}
