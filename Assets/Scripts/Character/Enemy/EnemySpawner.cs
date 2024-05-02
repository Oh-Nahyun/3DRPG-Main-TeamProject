using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : Waypoints
{
    public enum EnemyType : byte
    {
        SwordSkeleton = 0,
        NightmareDragon,
    }

    public EnemyType enemyType = EnemyType.SwordSkeleton;

    /// <summary>
    /// ���� ����
    /// </summary>
    public float interval = 1.0f;

    /// <summary>
    /// ������ �������� ���� �ð�
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// �����ʿ��� ���ÿ� �ִ�� ���������� ���� ��
    /// </summary>
    public int capacity = 3;

    /// <summary>
    /// ���� ������ ���� ��
    /// </summary>
    int count = 0;


    private void Update()
    {
        if (count < capacity)            // ĳ�۽�Ƽ Ȯ���ϰ�
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > interval)  // ���͹� Ȯ��
            {
                Spawn();                // �� �� ����� �Ǹ� ����
                elapsedTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// ���� �Ѹ��� �����ϴ� �Լ�
    /// </summary>
    void Spawn()
    {
        // �� �ϳ� ����(waypoint�� �� �������� �ϳ��� �����ؼ� ����)

        int randPos = Random.Range(0, children.Length);
        float randRot = Random.Range(0, 360.0f);

        switch (enemyType)
        {
            case EnemyType.SwordSkeleton:
                SwordSkeleton swordSkeleton = Factory.Instance.GetEnemy(children[randPos].position, randRot);
                swordSkeleton.onDie += () =>
                {
                    count--;
                };
                count++;
                break;
            case EnemyType.NightmareDragon:
                NightmareDragon nightmareDragon = Factory.Instance.GetNightmareDragonEnemy(children[randPos].position, randRot);
                nightmareDragon.onDie += () =>
                {
                    count--;
                };
                count++;
                break;
        }

    }
}