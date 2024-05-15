using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<int, QuestData> questList = new Dictionary<int, QuestData>();

    private QuestMessage questMessage;

    /// <summary>
    /// QuestMessage �ʱ�ȭ�� ���� ������Ƽ
    /// </summary>
    private QuestMessage QuestMessage
    {
        get
        {
            if(questMessage == null)
            {
                questMessage = FindObjectOfType<QuestMessage>();
            }
            return questMessage;
        }
        set => questMessage = value;
    }

    private List<QuestInfoPanel> questInfoPanels = new List<QuestInfoPanel>();

    public GameObject questInfoPanelPrefab; // QuestInfoPanel ������
    public Transform questInfoPanelParent;  // QuestInfoPanel�� ������ �θ� Transform

    public QuestInfo questInfo;

    /// <summary>
    /// QuestInfo �ʱ�ȭ�� ������Ƽ
    /// </summary>
    public QuestInfo QuestInfo
    {
        get
        {
            if(questInfo == null)
            {
                questInfo = FindObjectOfType<QuestInfo>();
            }
            return questInfo;
        }
        set => questInfo = value;
    }

    public List<int> onQuestID;
    public List<int> clearQuestID;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        GenerateData();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        QuestMessage = FindObjectOfType<QuestMessage>();
        QuestInfo = FindObjectOfType<QuestInfo>();
        questInfoPanelParent = questInfo.transform.GetChild(2);
    }

    /// <summary>
    /// Dictionary questList�� ����Ʈ Data�� �߰��ϴ� �Լ� (Ű�� / QuestData(����Ʈ ����, �̸�, ����, ��ǥ ID))
    /// </summary>
    private void GenerateData()
    {
        questList.Add(0, new QuestData(QuestData.QuestType.None, "����Ʈ �̸�", "����Ʈ ����", "����Ʈ ��ǥ", 1, 0));
        questList.Add(10, new QuestData(QuestData.QuestType.Hunt, "����Ʈ ���", "����Ʈ ���� ���", "����Ʈ ��ǥ 10����", 10, 1));
        questList.Add(20, new QuestData(QuestData.QuestType.GiveItem, "����Ʈ ������ ���", "����Ʈ ���� ������ ���", "����Ʈ ��ǥ 10��", 10, 100));
        questList.Add(30, new QuestData(QuestData.QuestType.ClearDungeon, "��� �����ϱ�", "����� ������ �����϶�", "��� Ŭ����", 1, 0));
    }

    /// <summary>
    /// Quest���� ��ȭ�� ����Ǿ����� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="id"></param>
    /// <param name="complete"></param>
    public void GetQuestTalkIndex(int id, bool complete)
    {
        if (questList.ContainsKey(id))
        {
            QuestData questData = questList[id];
            QuestMessage.OnQuestMessage(questData.questName, complete);
          
            if (!complete)
            {

                // ����Ʈ ������ ��
                // �ش� ����Ʈ�� ���� QuestInfoPanel�� �̹� �����Ǿ����� Ȯ��
                QuestInfoPanel existingPanel = questInfoPanels.Find(panel => panel.questId == id);
                if (existingPanel == null)
                {
                    // QuestInfoPanel ���� ���� �� �ʱ�ȭ
                    QuestInfoPanel newQuestInfoPanel = CreateQuestInfoPanel();
                    newQuestInfoPanel.Initialize(questData.questType , id, questData.questName, questData.questContents, questData.questObjectivesText, questData.questObjectivesCount, questData.questObjectID);

                    // ������ QuestInfoPanel�� ����Ʈ�� �߰�
                    questInfoPanels.Add(newQuestInfoPanel);
                }
            }
            else
            {
                // ����Ʈ �Ϸ��� ��
                // id�� �ش��ϴ� QuestInfoPanel ã�Ƽ� ����
                QuestInfoPanel panelToRemove = questInfoPanels.Find(panel => panel.questId == id);
                if (panelToRemove != null)
                {
                    DestroyQuestInfoPanel(panelToRemove);
                }
            }

        }
    }

    /// <summary>
    /// QuestInfoPanel������ ���� �Լ�
    /// </summary>
    /// <returns></returns>
    private QuestInfoPanel CreateQuestInfoPanel()
    {
        // QuestInfoPanel �������� Instantiate�Ͽ� ����
        GameObject newPanelObject = Instantiate(questInfoPanelPrefab, questInfoPanelParent);
        QuestInfoPanel newQuestInfoPanel = newPanelObject.GetComponent<QuestInfoPanel>();

        return newQuestInfoPanel;
    }

    /// <summary>
    /// QuestInfoPanel������ ���� �Լ�
    /// </summary>
    /// <param name="panel"></param>
    private void DestroyQuestInfoPanel(QuestInfoPanel panel)
    {
        // ����Ʈ���� �����ϰ� GameObject�� �ı�
        questInfoPanels.Remove(panel);
        Destroy(panel.gameObject);
    }

    public void OpenQuest()
    {
        QuestInfo.gameObject.SetActive(true);
        QuestInfo.OnQuestInfo();
    }
}
