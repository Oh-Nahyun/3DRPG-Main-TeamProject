using System;
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

    TestNPC test;

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
    /// <summary>
    /// ����Ʈ ��ǥ
    /// </summary>
    public int questObjectID;

    private int questCount = 0;
    public int QuestCount 
    {
        get => questCount;
        set 
        {
            QuestCount = Mathf.Clamp(value, 0, questMaxCount);
        } 
    }
    
    private int questMaxCount = 0;

    public Action<int> QuestClearId;



    private void Awake()
    {
        textQuestName = GetComponentInChildren<TextMeshProUGUI>();
        questInfoData = FindAnyObjectByType<QuestInfoData>();
        test = FindAnyObjectByType<TestNPC>();
    }

    private void Start()
    {
        test.EnemyQuestData[0] += (id) => 
        {
            if (id == questObjectID)
            {
                GetEnemyID();
            }
        };
    }

    private void Update()
    {
        textQuestName.text = questName;
        
    }

    /// <summary>
    /// ����Ʈ �г��� �����Ǿ��� �� ����Ǿ��� �Լ�
    /// </summary>
    /// <param name="type">����Ʈ�� ����</param>
    /// <param name="id">����Ʈ�� ID</param>
    /// <param name="name">����Ʈ�� ����</param>
    /// <param name="contents">����Ʈ�� ����</param>
    /// <param name="objectives">����Ʈ �������</param>
    /// <param name="count">����Ʈ�� ��ǥġ</param>
    /// <param name="objectID">����Ʈ�� ��ǥ ID</param>
    public void Initialize(QuestType type, int id, string name, string contents, string objectives, int count, int objectID)
    {
        questId = id;
        questName = name;
        questContents = contents;
        questObjectives = objectives;
        questObjectID = objectID;

        switch (type)
        {
            case QuestType.None:
                break;
            case QuestType.Hunt:
                HuntQuest(count);
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

    /// <summary>
    /// ����Ʈ �г��� Ŭ������ �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        questInfoData.OnQuestInfo(this.gameObject);
        questInfoData.setQuestList(questName, questContents, questObjectives);
    }

    // ��� ����Ʈ ���� -------------------------------
    public void HuntQuest(int maxKill)
    {
        questMaxCount = maxKill;
        questObjectives = $"óġ {QuestCount}/{questMaxCount} ";
        Debug.Log($"{QuestCount} ����");
        // ���� ����Ʈ ���� ��Ȳ�� �������� Ŭ���� ���θ� Ȯ���մϴ�.
        if (QuestCount >= maxKill)
        {
            QuestClear();
        }
    }

    /// <summary>
    /// ����Ʈ �г�UI�� ������Ʈ �ϴ� �Լ�
    /// </summary>
    void UpdateQuestProgress()
    {
        QuestCount++;
        questObjectives = $"óġ {QuestCount}/{questMaxCount} ";
        if (QuestCount == questMaxCount)
        {
            QuestClear();
        }
    }

    private void GetEnemyID()
    {
        test.EnemyQuestData[1] += (count) =>
        { 
                UpdateQuestProgress();
        };
    }

    // ������ ��� ����Ʈ ���� -------------------------------

    public void GiveItemQuest()
    {
        // Give item quest logic
    }

    // ���� Ŭ���� ����Ʈ ���� -------------------------------

    public void ClearDungeonQuest()
    {
        // Clear dungeon quest logic
    }

    /// <summary>
    /// ����Ʈ Ŭ����� ����� �Լ�
    /// </summary>
    private void QuestClear()
    {
        GameManager.Instance.QuestManager.clearQuestID.Add(questId);
        QuestClearId?.Invoke(questId);
        Debug.Log("Ŭ����");
    }

}
