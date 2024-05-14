using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldSetting : MonoBehaviour
{
    public Transform EnemySpawnPosition;
    public Transform ItemDropPosition;

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            Vector3 noisePosition = Random.onUnitSphere.normalized * 5f;  // �� ������ ���� ��ġ ����
            Factory.Instance.GetEnemy(EnemySpawnPosition.position + noisePosition);
        }

            Factory.Instance.GetNightmareDragonEnemy(EnemySpawnPosition.position);

        // ������ ��ȯ
        ItemDataManager dataManager = GameManager.Instance.ItemDataManager;

        Factory.Instance.GetItemObject(dataManager[ItemCode.Sword], ItemDropPosition.position);
        Factory.Instance.GetItemObject(dataManager[ItemCode.Bow], ItemDropPosition.position);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.Arrow], 10, ItemDropPosition.position, true);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.HP_portion_Tick], 2, ItemDropPosition.position);
    }
}
