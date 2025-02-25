using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestData;

/// <summary>
/// 보스 스테이지 세팅 스크립트
/// </summary>
public class BossStageSetting : MonoBehaviour
{
    Boss boss;

    public Transform dropposition;
    public GameObject exitObj;
    public GameObject chestObj;

    void Start()
    {
        boss = FindAnyObjectByType<Boss>(); // 보스 찾기
        boss.gameObject.SetActive(false);   // 보스 비활성화

        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[4], dropposition.position);
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[8], dropposition.position);
        Factory.Instance.GetItemObjects(GameManager.Instance.ItemDataManager[9],5 ,dropposition.position, false);
        Factory.Instance.GetItemObjects(GameManager.Instance.ItemDataManager[9],5 ,dropposition.position, false);
    }

    private void Update()
    {
        if(!boss.IsAlive)   // 보스가 사망하면
        {
            exitObj.SetActive(true);
            chestObj.SetActive(true);

            QuestManager.Instance.GetQuestTalkIndex((int)QuestType.ClearBoss * 10, true, true);
            QuestManager.Instance.checkClearQuests[(int)QuestType.ClearBoss] = true;
        }
    }

    public Boss GetBoss()
    {
        if (boss == null)
        {
            Debug.LogWarning($"BossStageSetting : Boss 오브젝트를 찾을 수 없습니다");
        }

        return boss;
    }
}
