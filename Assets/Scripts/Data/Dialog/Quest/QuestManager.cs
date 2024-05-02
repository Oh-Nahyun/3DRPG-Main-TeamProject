using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questList = new Dictionary<int, QuestData>();

    private QuestMessage questMessage;

    private List<QuestInfoPanel> questInfoPanels = new List<QuestInfoPanel>();

    public GameObject questInfoPanelPrefab; // QuestInfoPanel ������
    public Transform questInfoPanelParent;  // QuestInfoPanel�� ������ �θ� Transform


    private void Awake()
    {
        questMessage = FindObjectOfType<QuestMessage>();
        GenerateData();
    }

    /// <summary>
    /// Dictionary questList�� ����Ʈ Data�� �߰��ϴ� �Լ� (Ű�� / QuestData(����Ʈ ����, �̸�, ����, ��ǥ))
    /// </summary>
    private void GenerateData()
    {
        questList.Add(0, new QuestData(QuestData.QuestType.None, "����Ʈ �̸�", "����Ʈ ����", "����Ʈ ��ǥ", 1, gameObject));
        questList.Add(10, new QuestData(QuestData.QuestType.Hunt, "����Ʈ ���", "����Ʈ ���� ���", "����Ʈ ��ǥ 10����", 10, gameObject));
        questList.Add(20, new QuestData(QuestData.QuestType.GiveItem, "����Ʈ ������ ���", "����Ʈ ���� ������ ���", "����Ʈ ��ǥ 10��", 10, gameObject));
        questList.Add(30, new QuestData(QuestData.QuestType.ClearDungeon, "����Ʈ ���� Ŭ����", "����Ʈ ���� ���� Ŭ����", "����Ʈ ��ǥ ����", 1, gameObject));
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
            questMessage.OnQuestMessage(questData.questName, complete);

            
            if (!complete)
            {

                // ����Ʈ ������ ��
                // �ش� ����Ʈ�� ���� QuestInfoPanel�� �̹� �����Ǿ����� Ȯ��
                QuestInfoPanel existingPanel = questInfoPanels.Find(panel => panel.questId == id);
                if (existingPanel == null)
                {
                    // QuestInfoPanel ���� ���� �� �ʱ�ȭ
                    QuestInfoPanel newQuestInfoPanel = CreateQuestInfoPanel();
                    newQuestInfoPanel.Initialize(questData.questType , id, questData.questName, questData.questContents, questData.questObjectivesText, questData.questObjectivesCount);

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
}
