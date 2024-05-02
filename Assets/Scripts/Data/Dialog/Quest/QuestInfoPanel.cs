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

    private int questCount = 0;
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
        test.EnemyId += (id) => getEnemyID(id);
        test.KillEnemy += (count) => 
        {
            UpdateQuestProgress(count);
            UpdateQuestUI();
        };
    }

    private void Update()
    {
        textQuestName.text = questName;
        
    }

    public void Initialize(QuestType type, int id, string name, string contents, string objectives, int count)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        questInfoData.OnQuestInfo(this.gameObject);
        questInfoData.setQuestList(questName, questContents, questObjectives);
    }

    public void HuntQuest(int maxKill)
    {
        questMaxCount = maxKill;
        UpdateQuestUI();
        Debug.Log($"{questCount} ����");
        // ���� ����Ʈ ���� ��Ȳ�� �������� Ŭ���� ���θ� Ȯ���մϴ�.
        if (questCount >= maxKill)
        {
            QuestClear();
        }
    }

    public void GiveItemQuest()
    {
        // Give item quest logic
    }

    public void ClearDungeonQuest()
    {
        // Clear dungeon quest logic
    }

    public void UpdateQuestProgress(int currentKill)
    {
        this.questCount = currentKill;
    }
    void UpdateQuestUI()
    {
        questObjectives = $"óġ {questCount}/{questMaxCount} ";
    }

    private void QuestClear()
    {
        QuestClearId?.Invoke(questId);
    }

    private void getEnemyID(int EnemyId)
    {
        
    }

}
