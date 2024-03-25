using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ������")]
    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    public float moveSpeed = 1.0f;

    public float damage = 20.0f;

    /// <summary>
    /// ���� HP
    /// </summary>
    float hp = 1;

    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0) // HP�� 0 ���ϰ� �Ǹ� �״´�.
            {
                hp = 0;
                OnDie();
            }
        }
    }

    /// <summary>
    /// �ִ� HP
    /// </summary>
    public float maxHP = 100.0f;

    /// <summary>
    /// ���� ���� �� ����� ��������Ʈ
    /// </summary>
    Action onDie;

    /// <summary>
    /// ������ �� �÷��̾�
    /// </summary>
    Player player;


    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();     // �� �ʱ�ȭ �۾�
    }

    protected override void OnDisable()
    {
        if (player != null)
        {
            onDie = null;               // Ȯ���ϰ� �����Ѵٰ� ǥ��
            player = null;
        }

        base.OnDisable();
    }

    /// <summary>
    /// EnemyWave �迭�� �ʱ�ȭ �Լ�
    /// </summary>
    protected virtual void OnInitialize()
    {
        if (player == null)
        {
            player = GameManager.Instance.Player;   // �÷��̾� ã��
        }

        HP = maxHP; // HP �ִ�� ����
    }

    /// <summary>
    /// ��� ó���� �Լ�
    /// </summary>
    protected virtual void OnDie()
    {
        onDie?.Invoke();                // �׾��ٴ� ��ȣ������

        gameObject.SetActive(false);    // �ڱ� �ڽ� ��Ȱ��ȭ
    }

}
