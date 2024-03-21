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

    InventorySlot slotData;

    public InventorySlot SlotData
    {
        get => slotData;
        set
        {
            if(slotData != value)
            {
                slotData = value;
                // ����
                //slotIcon.sprite = slotData.SlotItemData.itemIcon;
                //slotItemCount.text = slotData.CurrentItemCount.ToString();
            }
        }
    }

    void Awake()
    {
        Transform child = transform.GetChild(0);
        slotIcon = child.GetComponent<Image>();

        child = transform.GetChild(1);
        slotItemCount = child.GetComponent<TextMeshProUGUI>();
    }
}
