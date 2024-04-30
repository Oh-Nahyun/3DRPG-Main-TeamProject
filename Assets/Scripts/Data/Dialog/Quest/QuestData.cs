using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    public enum questType
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
    /// ����Ʈ ����
    /// </summary>
    public string questContents;

    /// <summary>
    /// ����Ʈ ��ǥ
    /// </summary>
    public string questObjectives;

    public int[] npcId;

    public QuestData(questType type, string name, string contents, string objectives)
    {
        questName = name;
        questContents = contents;
        questObjectives = objectives;
    }

    public void QuestInfo(questType type, string name, string contents, string objectives)
    {
        questName = name;
        questContents = contents;

        switch (type)
        {
            case questType.None:
                break;
            case questType.Hunt:
                HuntQuset(1,1);
                break;
            case questType.GiveItem:
                GiveItemQuset();
                break;
            case questType.ClearDungeon:
                ClearDungeonQuset();
                break;
            default:
                break;
        }
    }

    public void HuntQuset(int kill, int maxKill)
    {
        int killCount = maxKill;
    }

    public void GiveItemQuset()
    {

    }

    public void ClearDungeonQuset()
    {

    }

}
