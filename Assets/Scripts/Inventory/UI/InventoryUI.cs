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

    /// <summary>
    /// ���� �巡�� ����
    /// </summary>
    /// <param name="index">�ӽ� ���Կ� �� �κ��丮 ���� �ε���</param>
    private void OnSlotDragBegin(uint index)
    {
        if (Inventory[index].SlotItemData != null)
        {
            uint targetSlotIndex = index;
            uint targetSlotItemCode = (uint)Inventory[index].SlotItemData.itemCode;
            int targetItemSlotCount = Inventory[index].CurrentItemCount;

            Inventory.AccessTempSlot(targetSlotIndex, targetSlotItemCode, targetItemSlotCount);
            inventory[index].ClearItem();
        }
    }

    /// <summary>
    /// ���� �巡�� ����
    /// </summary>
    /// <param name="index">�������� ���� �κ��丮 ���� �ε���</param>
    private void OnSlotDragEnd(uint index)
    {
        uint tempFromIndex = Inventory.TempSlot.FromIndex;

        // �ӽ� ���Կ� ����ִ� ����
        uint tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
        int tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;

        if(Inventory[index].SlotItemData != null)   // �������� ����ִ�.
        {
            uint targetSlotItemCode = (uint)Inventory[index].SlotItemData.itemCode;
            int targetSlotItemCount = Inventory[index].CurrentItemCount;

            Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount); // target ���Կ� ������ ����
            Inventory.AccessTempSlot(index, targetSlotItemCode, targetSlotItemCount); // target ���Կ� �־��� ������ ���� �ӽ� ���Կ� ����
            
            tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
            tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;
            
            Inventory.AccessTempSlot(tempFromIndex, tempSlotItemCode, tempSlotItemCount);
        }
        else
        {
            Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount);
        }
    }
    // UI ����
    // UI �ݱ�
}
