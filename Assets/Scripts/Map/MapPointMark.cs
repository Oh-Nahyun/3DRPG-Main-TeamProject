using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map Mark ������Ʈ�� ����ϴ� Ŭ����
/// </summary>
public class MapPointMark : MonoBehaviour
{
    GameObject highlightMark;

    void Start()
    {
        Transform child = transform.GetChild(0);
        highlightMark = child.gameObject;

        highlightMark.SetActive(false);
    }

    /// <summary>
    /// Mark ������Ʈ�� ������ Ŭ������ �� �����ϴ� �Լ�
    /// </summary>
    public void DestoryMark()
    {
        Debug.Log($"GameObject Name : {transform.position}");

        Destroy(transform.parent.gameObject);  // �� ������Ʈ ����
    }

    /// <summary>
    /// highlight mark�� Ȱ��ȭ �ϴ� �Լ�
    /// </summary>
    public void EnableHighlightMark()
    {
        highlightMark.SetActive(true);
    }

    /// <summary>
    /// highlight mark�� ��Ȱ��ȭ �ϴ� �Լ�
    /// </summary>
    public void DisableHighlightMark()
    {
        highlightMark.SetActive(false);
    }
}
