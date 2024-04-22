using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map Mark ������Ʈ�� ����ϴ� Ŭ����
/// </summary>
public class MapPointMark : MonoBehaviour
{
    /// <summary>
    /// Mark ������Ʈ�� ������ Ŭ������ �� �����ϴ� �Լ�
    /// </summary>
    public void ShowMarkInfo()
    {
        Debug.Log($"GameObject Name : {transform.position}");

        Destroy(transform.parent.gameObject);  // �� ������Ʈ ����
    }
}
