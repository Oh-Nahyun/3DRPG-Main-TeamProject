using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryGoldUI : MonoBehaviour
{
    TextMeshProUGUI goldText;

    /// <summary>
    /// ��差�� �ٲ� �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<uint> onGoldChange;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        goldText = child.GetComponent<TextMeshProUGUI>();

        onGoldChange += OnGoldChange;
    }

    /// <summary>
    /// ��差 ����ϴ� �Լ�
    /// </summary>
    /// <param name="gold">����� ��差</param>
    void OnGoldChange(uint gold)
    {
        goldText.text = $"{gold:D}";
    }
}