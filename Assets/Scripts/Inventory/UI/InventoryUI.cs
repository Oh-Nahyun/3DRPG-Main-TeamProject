using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// �κ��丮
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// �κ��丮 ���ٿ� ������Ƽ
    /// </summary>
    Inventory Inventory => inventory;

    /// <summary>
    /// UI slots
    /// </summary>
    InventorySlotUI[] slotsUIs;

    /// <summary>
    /// �ӽ� ���� UI
    /// </summary>
    TempSlotUI tempSlotUI;

    public Action<uint> onSlotDragBegin;
    public Action<uint> onSlotDragEnd;

    /// <summary>
    /// �κ��丮 UI�� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="playerInventory">�÷��̾� �κ��丮</param>
    public void InitializeInventoryUI(Inventory playerInventory)
    {
        inventory = playerInventory;    // �ʱ�ȭ�� �κ��丮 ���� �ޱ�
        slotsUIs = new InventorySlotUI[Inventory.slotSize]; // ���� ũ�� �Ҵ�
        slotsUIs = GetComponentsInChildren<InventorySlotUI>();  // �Ϲ� ����
        tempSlotUI = GetComponentInChildren<TempSlotUI>(); // �ӽ� ����

        for (uint i = 0; i < Inventory.slotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(Inventory[i]); // �κ��丮������ slotUI�� ����
        }
        tempSlotUI.InitializeSlotUI(Inventory.TempSlot); // null ����

        onSlotDragBegin += OnSlotDragBegin;
        onSlotDragEnd += OnSlotDragEnd;

    }

    // �巡�� ����
    private void OnSlotDragBegin(uint index)
    {
        if (Inventory[index].SlotItemData != null)
        {
            Debug.Log(Inventory.TempSlot);
            Inventory.SlotToTemp(Inventory[index].SlotIndex,
                           (uint)Inventory[index].SlotItemData.itemCode,
                                 Inventory[index].CurrentItemCount);
        }
    }

    // �巡�� ����
    private void OnSlotDragEnd(uint index)
    {
        Debug.Log(Inventory.TempSlot.SlotIndex);
        if (Inventory[index].SlotItemData != null) // �ű�� ���Կ� �������� �ִ�
        {
            ItemData tempData = Inventory[index].SlotItemData;
            uint tempIndex = Inventory[index].SlotIndex;
            int tempItemCount = Inventory[index].CurrentItemCount;

            Inventory.TempToSlot(Inventory.TempSlot.SlotIndex,
                           (uint)Inventory.TempSlot.SlotItemData.itemCode,
                                 Inventory.TempSlot.CurrentItemCount);
            Inventory.SlotToTemp(tempIndex, (uint)tempData.itemCode, tempItemCount);
        }
        else // �ű�� ���� �������� ����.
        {
            Inventory.TempToSlot(Inventory.TempSlot.SlotIndex,
                           (uint)Inventory.TempSlot.SlotItemData.itemCode,
                                 Inventory.TempSlot.CurrentItemCount);
        }
    }

    // UI ����
    // UI �ݱ�
}
