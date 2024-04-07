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
    /// ������ ����
    /// </summary>
    TextMeshProUGUI slotEquip;

    /// <summary>
    /// slotUI�� ���Ե�����
    /// </summary>
    InventorySlot inventorySlot;

    /// <summary>
    /// slotUI ������ ���� ������Ƽ
    /// </summary>
    public InventorySlot InventorySlotData => inventorySlot;

    /// <summary>
    /// �κ��丮 ������ �����͸� �޾Ƽ� �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    /// <param name="slot">�κ��丮 ����</param>
    public void InitializeSlotUI(InventorySlot slot)
    {
        Transform child = transform.GetChild(0);
        slotIcon = child.GetComponent<Image>();

        child = transform.GetChild(1);
        slotItemCount = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        slotEquip = child.GetComponent<TextMeshProUGUI>();

        inventorySlot = slot;
        InventorySlotData.onChangeSlotData = Refresh;   // inventorySlot�� ��������Ʈ�� UI�� ������ �Լ� ���
        Refresh();
    }
    
    /// <summary>
    /// �κ��丮 ���� ���ΰ�ħ�ϴ� �Լ�
    /// </summary>
    private void Refresh()
    {
        if(InventorySlotData.SlotItemData == null)
        {
            // ���Կ� �������� ������
            slotIcon.color = Color.clear;
            slotIcon.sprite = null;
            slotItemCount.text = string.Empty;

            slotEquip.color = Color.clear;
        }
        else
        {   // ���Կ� ������ �����Ͱ� ������ ����
            slotIcon.color = Color.white;
            slotIcon.sprite = InventorySlotData.SlotItemData.itemIcon;
            slotItemCount.text = InventorySlotData.CurrentItemCount.ToString();            

            slotEquip.color = InventorySlotData.IsEquip ? Color.white : Color.clear; // ���� ���� 
        }
    }
}
