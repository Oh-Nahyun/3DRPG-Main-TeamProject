using System;
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

    /// <summary>
    /// slotUI�� ���Ե�����
    /// </summary>
    InventorySlot inventorySlot;

    /// <summary>
    /// slotUI ������ ���� ������Ƽ
    /// </summary>
    public InventorySlot Slot => inventorySlot;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        slotIcon = child.GetComponentInChildren<Image>();

        child = transform.GetChild(1);
        slotItemCount = child.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitializeSlotUI(InventorySlot slot)
    {
        inventorySlot = slot;
        inventorySlot.onChangeSlotData = Refresh;
        Refresh();
    }

    protected virtual void Refresh()
    {
        if(Slot.SlotItemData == null)
        {
            slotIcon.color = Color.clear;
            slotIcon.sprite = null;
            slotItemCount.text = string.Empty;
        }
        else
        {
            slotIcon.color = Color.white;
            slotIcon.sprite = Slot.SlotItemData.itemIcon;
            slotItemCount.text = Slot.CurrentItemCount.ToString();
        }

        Debug.Log($"{Slot.SlotIndex}, {Slot.CurrentItemCount}");
    }
}
