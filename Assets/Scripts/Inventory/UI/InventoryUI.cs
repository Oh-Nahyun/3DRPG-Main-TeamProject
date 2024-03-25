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

    /// <summary>
    /// ������ ���� UI
    /// </summary>
    InventoryDetailUI detailUI;

    /// <summary>
    /// ������ ������ �г�
    /// </summary>
    DividUI dividUI;

    /// <summary>
    /// ������ ���� UI
    /// </summary>
    SortUI sortUI;

    public Action<uint> onSlotDragBegin;
    public Action<uint> onSlotDragEnd;
    public Action onSlotDragEndFail;
    public Action<uint> onShowDetail;
    public Action onCloseDetail;
    public Action<uint> onDivdItem;

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
        detailUI = GetComponentInChildren<InventoryDetailUI>(); // ������ ���� �г�
        dividUI = GetComponentInChildren<DividUI>(); // ������ ������ �г�
        sortUI = GetComponentInChildren<SortUI>(); // ������ ���� UI

        for (uint i = 0; i < Inventory.slotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(Inventory[i]); // �κ��丮������ slotUI�� ����
        }
        tempSlotUI.InitializeSlotUI(Inventory.TempSlot); // null ����

        onSlotDragBegin += OnSlotDragBegin;
        onSlotDragEnd += OnSlotDragEnd;
        onShowDetail += OnShowDetail;
        onCloseDetail += OnCloseDetail;
        onSlotDragEndFail += OnSlotDragFail;
        onDivdItem += OnDividItem;
        dividUI.onDivid += DividItem;
        sortUI.onSortItem += OnSortItem;
    }

    private void OnSortItem(uint sortMode, bool isAcending)
    {
        Inventory.SortSlot((SortMode)sortMode, isAcending);        
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

            tempSlotUI.OpenTempSlot();

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
            if(Inventory[index].SlotItemData.itemCode == Inventory.TempSlot.SlotItemData.itemCode)
            {
                Inventory[index].AssignItem(tempSlotItemCode, tempSlotItemCount, out int overCount);

                if(overCount > 0) // ���Կ� �־��µ� ��������
                {
                    OnSlotDragFail();
                }

                Inventory.TempSlot.ClearItem();
            }
            else
            {
                uint targetSlotItemCode = (uint)Inventory[index].SlotItemData.itemCode;
                int targetSlotItemCount = Inventory[index].CurrentItemCount;

                inventory[index].ClearItem();
                Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount); // target ���Կ� ������ ����
                Inventory.AccessTempSlot(index, targetSlotItemCode, targetSlotItemCount); // target ���Կ� �־��� ������ ���� �ӽ� ���Կ� ����
            
                tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
                tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;

                Inventory.AccessTempSlot(tempFromIndex, tempSlotItemCode, tempSlotItemCount);
            }
        }
        else
        {
            Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount);
        }

        tempSlotUI.CloseTempSlot();
    }

    private void OnSlotDragFail()
    {
        uint fromIndex = Inventory.TempSlot.FromIndex;
        uint tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
        int tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;

        Inventory.AccessTempSlot(fromIndex, tempSlotItemCode, tempSlotItemCount);
        tempSlotUI.CloseTempSlot();
    }

    private void OnShowDetail(uint index)
    {
        if (Inventory[index].SlotItemData != null)
        {
            string name = Inventory[index].SlotItemData.itemName;
            string desc = Inventory[index].SlotItemData.desc;
            uint price = Inventory[index].SlotItemData.price;

            detailUI.SetDetailText(name, desc, price);
            detailUI.OpenTempSlot();
        }
    }

    /// <summary>
    /// ������ ���� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="index">���� ������ ���� �ε���</param>
    private void OnDividItem(uint index)
    {
        dividUI.InitializeValue(Inventory[index], 1, (int)Inventory[index].CurrentItemCount - 1);
        dividUI.DividUIOpen();
    }

    /// <summary>
    /// �������� ���� �� ��������Ʈ�� ��ȣ�� ������ �Լ�
    /// </summary>
    /// <param name="InventorySlot">���� ������ ����</param>
    /// <param name="count">���� �����۾�</param>
    private void DividItem(InventorySlot slot, int count)
    {
        uint nextIndex = slot.SlotIndex;

        if(!Inventory.IsVaildSlot(nextIndex)) // ���� ���Ը� ã�Ƽ� ������ ���� X
        {
            Debug.Log("������ �������� �ʽ��ϴ�.");
            return;
        }
        else
        {
            while(Inventory.IsVaildSlot(nextIndex))
            {
                nextIndex++;
                if(nextIndex >= Inventory.slotSize)
                {
                    Debug.LogError($"�ش� �κ��丮�� ������ �����մϴ�.");
                    return;
                }
            }

            Inventory.DividItem(slot.SlotIndex, nextIndex, count);
        }
    }

    private void OnCloseDetail()
    {
        detailUI.ClearText();
        detailUI.CloseTempSlot();
    }
    // UI ����
    // UI �ݱ�
}
