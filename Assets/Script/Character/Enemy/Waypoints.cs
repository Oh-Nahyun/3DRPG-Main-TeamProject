using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������Ʈ�� �����ϰ� ó���ϴ� Ŭ����
/// </summary>
public class Waypoints : MonoBehaviour
{
    /// <summary>
    /// ��������Ʈ ������
    /// </summary>
    Transform[] children;

    /// <summary>
    /// ���� �������� �ε���
    /// </summary>
    int index = 0;

    /// <summary>
    /// ���� �������� ��ġ
    /// </summary>
    public Vector3 NextTarget => children[index].position;

    private void Awake()
    {
        // �ڽĵ��� ���� ��������Ʈ�� ���
        children = new Transform[transform.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// ���� ��������Ʈ ������ �����ϱ� ���� �Լ�
    /// </summary>
    public void StepNextWaypoint()
    {
        index++;
        index %= children.Length;
    }
}