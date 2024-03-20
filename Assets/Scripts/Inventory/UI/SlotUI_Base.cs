using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���� UI ���̽� Ŭ����
/// </summary>
public class SlotUI_Base : MonoBehaviour
{
    /// <summary>
    /// ���� ������
    /// </summary>
    Image slotIcon;

    /// <summary>
    /// ������ ����
    /// </summary>
    TextMeshProUGUI slotItemCount;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        slotIcon = child.GetComponent<Image>();

        child = transform.GetChild(1);
        slotItemCount = child.GetComponent<TextMeshProUGUI>();
    }
}
