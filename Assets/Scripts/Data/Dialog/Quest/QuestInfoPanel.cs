using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static QuestData;

public class QuestInfoPanel : MonoBehaviour, IPointerClickHandler
{
    TextMeshProUGUI textQuestName;
    QuestInfoData questInfoData;

    public int questId;
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

    private void Awake()
    {
        textQuestName = GetComponentInChildren<TextMeshProUGUI>();
        questInfoData = FindAnyObjectByType<QuestInfoData>();
    }

    private void Update()
    {
        textQuestName.text = questName;
    }

    public void Initialize(QuestType type, int id, string name, string contents, string objectives)
    {
        questId = id;
        questName = name;
        questContents = contents;
        questObjectives = objectives;

        switch (type)
        {
            case QuestType.None:
                break;
            case QuestType.Hunt:
                HuntQuest(1, 10);
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
        // �� �޼���� QuestManager���� ȣ��� ������ ȣ��˴ϴ�.
        // ���⿡ �ʿ��� UI ������Ʈ ���� ������ �߰��� �� �ֽ��ϴ�.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        questInfoData.OnQuestInfo(this.gameObject);
        questInfoData.setQuestList(questName, questContents, questObjectives);
    }

    public void HuntQuest(int currentKill, int maxKill)
    {
        int killCount = maxKill - currentKill;
        Debug.Log(killCount);
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
