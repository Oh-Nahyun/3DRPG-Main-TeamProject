using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkeletonPool : ObjectPool<SwordSkeleton>
{
    /// <summary>
    /// ������ ����� ��������Ʈ��. �ݵ�� �ϳ��� �־�� �Ѵ�.
    /// </summary>
    public Waypoints[] waypoints;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        waypoints = child.GetComponentsInChildren<Waypoints>(); // Ǯ�� �ڽĿ��� ��� ã��
    }

    /// <summary>
    /// Ǯ���� ������� �ʴ� ������Ʈ�� �ϳ� ���� �� ���� �ϴ� �Լ�
    /// </summary>
    /// <param name="index">����� ��������Ʈ �ε���</param>
    /// <param name="position">��ġ�� ��ġ(������ǥ)</param>
    /// <param name="eulerAngle">��ġ�� ���� ����</param>
    /// <returns>Ǯ���� ���� ������Ʈ(Ȱ��ȭ��)</returns>
    public SwordSkeleton GetObject(int index, Vector3? position = null, Vector3? eulerAngle = null)
    {
        SwordSkeleton enemy = GetObject(position, eulerAngle);
        enemy.waypoints = waypoints[index];

        return enemy;
    }

    protected override void OnGenerateObject(SwordSkeleton comp)
    {
        comp.waypoints = waypoints[0];  // ����Ʈ�� ù��° ��������Ʈ ���
    }
}
