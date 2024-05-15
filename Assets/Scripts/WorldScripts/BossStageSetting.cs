using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ���� ��ũ��Ʈ
/// </summary>
public class BossStageSetting : MonoBehaviour
{
    Boss boss;

    public Transform dropposition;
    public GameObject exitObj;

    void Start()
    {
        boss = FindAnyObjectByType<Boss>(); // ���� ã��
        boss.gameObject.SetActive(false);   // ���� ��Ȱ��ȭ

        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[4], dropposition.position);
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[8], dropposition.position);
        Factory.Instance.GetItemObjects(GameManager.Instance.ItemDataManager[9],5 ,dropposition.position);
    }

    private void Update()
    {
        if(!boss.IsAlive)
        {
            exitObj.SetActive(true);
        }
    }

    public Boss GetBoss()
    {
        if (boss == null)
        {
            Debug.LogWarning($"BossStageSetting : Boss ������Ʈ�� ã�� �� �����ϴ�");
        }

        return boss;
    }
}
