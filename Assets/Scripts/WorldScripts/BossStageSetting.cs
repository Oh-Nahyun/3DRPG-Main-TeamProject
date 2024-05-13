using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ���� ��ũ��Ʈ
/// </summary>
public class BossStageSetting : MonoBehaviour
{
    Boss boss;

    void Start()
    {
        boss = FindAnyObjectByType<Boss>(); // ���� ã��
        boss.gameObject.SetActive(false);   // ���� ��Ȱ��ȭ
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
