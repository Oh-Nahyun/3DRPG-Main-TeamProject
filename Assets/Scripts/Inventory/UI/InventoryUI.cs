using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    InventoryDividUI dividUI;

    /// <summary>
    /// ������ ���� UI
    /// </summary>
    InventorySortUI sortUI;

    public Action<uint> onSlotDragBegin;
    public Action<GameObject> onSlotDragEnd;
    public Action onSlotDragEndFail;
    public Action<uint> onShowDetail;
    public Action onCloseDetail;
    public Action<uint> onDivdItem;
    public Action<GameObject> onEquipClick;

    /// <summary>
    /// �κ��丮 UI�� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="playerInventory">�÷��̾� �κ��丮</param>
    public void InitializeInventoryUI(Inventory playerInventory)
    {
        inventory = playerInventory;    // �ʱ�ȭ�� �κ��丮 ���� �ޱ�
        slotsUIs = new InventorySlotUI[Inventory.SlotSize]; // ���� ũ�� �Ҵ�
        slotsUIs = GetComponentsInChildren<InventorySlotUI>();  // �Ϲ� ����
        tempSlotUI = GetComponentInChildren<TempSlotUI>(); // �ӽ� ����
        detailUI = GetComponentInChildren<InventoryDetailUI>(); // ������ ���� �г�
        dividUI = GetComponentInChildren<InventoryDividUI>(); // ������ ������ �г�
        sortUI = GetComponentInChildren<InventorySortUI>(); // ������ ���� UI

        for (uint i = 0; i < Inventory.SlotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(Inventory[i]); // �κ��丮������ slotUI�� ����
        }
        tempSlotUI.InitializeSlotUI(Inventory.TempSlot); // null ����

        onSlotDragBegin += OnSlotDragBegin;
        onSlotDragEnd += OnSlotDragEnd;
        onShowDetail += OnShowDetail;
        onCloseDetail += OnCloseDetail;
        onSlotDragEndFail += OnSlotDragFail;
        onDivdItem += OnClickItem;
        dividUI.onDivid += DividItem;
        sortUI.onSortItem += OnSortItem;

        onEquipClick += OnEquipItemClick;
    }

    private void OnEquipItemClick(GameObject obj)
    {
        
    }

    /// <summary>
    /// �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sortMode">������ ���� ���</param>
    /// <param name="isAcending">true�� ��������, false�� ��������</param>
    private void OnSortItem(uint sortMode, bool isAcending)
    {
        // �������� ���������� ������ �������� ����� �����ϱ�

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
            bool targetIsEquip = Inventory[index].IsEquip;


            tempSlotUI.OpenTempSlot();

            Inventory.AccessTempSlot(targetSlotIndex, targetSlotItemCode, targetItemSlotCount);
            Inventory.TempSlot.IsEquip = targetIsEquip;
            inventory[index].ClearItem();
        }
    }

    /// <summary>
    /// ���� �巡�� ����
    /// </summary>
    /// <param name="index">�������� ���� �κ��丮 ���� �ε���</param>
    private void OnSlotDragEnd(GameObject slotObj)
    {
        if(slotObj == null) // �巡�� ���� ������ �����Ǵ� ������ ����.
        {
            OnSlotDragFail();
            Debug.Log("�������� �ʴ� ������Ʈ�Դϴ�");
            return;
        }
        else
        {
            SlotUI_Base slotUI = slotObj.GetComponent<SlotUI_Base>();
            bool isSlot = slotUI is SlotUI_Base;

            if(!isSlot) // �巡�� ������ ������ ������ �ƴϴ�.
            {
                OnSlotDragFail();
                Debug.Log("������ �ƴմϴ�");
                return;
            }

            // ���� �ε���
            uint index = slotUI.InventorySlotData.SlotIndex;
            uint tempFromIndex = Inventory.TempSlot.FromIndex;

            // �ӽ� ���Կ� ����ִ� ����
            uint tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
            int tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;
            bool tempSlotIsEqiup = Inventory.TempSlot.IsEquip;

            if (Inventory[index].SlotItemData != null)   // �������� ����ִ�.
            {
                if (Inventory[index].SlotItemData.itemCode == Inventory.TempSlot.SlotItemData.itemCode) // ��ȯ�Ϸ��� �������� ������
                {
                    Inventory[index].AssignItem(tempSlotItemCode, tempSlotItemCount, out int overCount);

                    if (overCount > 0) // ���Կ� �־��µ� ��������
                    {
                        OnSlotDragFail();
                    }

                    Inventory.TempSlot.ClearItem();
                }
                else // �������� ����ְ� ��ǥ ������ �����Ѵ�.
                {
                    uint targetSlotItemCode = (uint)Inventory[index].SlotItemData.itemCode;
                    int targetSlotItemCount = Inventory[index].CurrentItemCount;
                    bool targetSlotIsEquip = Inventory[index].IsEquip;

                    inventory[index].ClearItem();
                    Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount); // target ���Կ� ������ ����
                    Inventory[index].IsEquip = tempSlotIsEqiup;

                    Inventory.AccessTempSlot(index, targetSlotItemCode, targetSlotItemCount); // target ���Կ� �־��� ������ ���� �ӽ� ���Կ� ����
                    Inventory.TempSlot.IsEquip = targetSlotIsEquip;


                    tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
                    tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;
                    tempSlotIsEqiup = Inventory.TempSlot.IsEquip;

                    Inventory.AccessTempSlot(tempFromIndex, tempSlotItemCode, tempSlotItemCount);
                    Inventory[tempFromIndex].IsEquip = tempSlotIsEqiup;
                }
            }
            else
            {
                Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount);
            }

            tempSlotUI.CloseTempSlot();
        }
    }

    /// <summary>
    /// ������ �巡�׸� ���������� �������� ������ �� �����ϴ� �Լ� ( �ٽ� ���� �������� �ǵ�����. )
    /// </summary>
    private void OnSlotDragFail()
    {
        if (Inventory.TempSlot.SlotItemData == null)
            return;

        uint fromIndex = Inventory.TempSlot.FromIndex;
        uint tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
        int tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;
        bool tempSlotIsEquip = Inventory.TempSlot.IsEquip;

        Inventory.AccessTempSlot(fromIndex, tempSlotItemCode, tempSlotItemCount);
        Inventory[fromIndex].IsEquip = tempSlotIsEquip;
        
        tempSlotUI.CloseTempSlot();
    }

    /// <summary>
    /// ������ ������ �г��� �����ִ� �Լ�
    /// </summary>
    /// <param name="index">�����ٷ��� ������ ���� �ε���</param>
    private void OnShowDetail(uint index)
    {
        if (Inventory[index].SlotItemData != null)
        {
            string name = Inventory[index].SlotItemData.itemName;
            string desc = Inventory[index].SlotItemData.desc;
            uint price = Inventory[index].SlotItemData.price;

            detailUI.SetDetailText(name, desc, price);
            detailUI.ShowItemDetail();
        }
    }

    /// <summary>
    /// ������ ���� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="index">���� ������ ���� �ε���</param>
    private void OnClickItem(uint index)
    {
        // Key Q
        bool isPressedQ = Keyboard.current.qKey.ReadValue() > 0;
      
        if(isPressedQ) // dividUI ����
        {
            if (Inventory[index].CurrentItemCount <= 1)
            {
                Debug.Log($"[{Inventory[index].SlotItemData.itemName}]�� �������� [{Inventory[index].CurrentItemCount}]�� �ֽ��ϴ�.");
                return;
            }
            dividUI.InitializeValue(Inventory[index], 1, (int)Inventory[index].CurrentItemCount - 1);
            dividUI.DividUIOpen();
        }
        else // Ŭ���ϸ�
        {
            bool isEquip = Inventory[index].SlotItemData is IEquipable;
            if (isEquip)
            {
                EquipItem(index);
            }
        }
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
                if(nextIndex >= Inventory.SlotSize)
                {
                    Debug.LogError($"�ش� �κ��丮�� ������ �����մϴ�.");
                    return;
                }
            }

            Inventory.DividItem(slot.SlotIndex, nextIndex, count);
        }
    }

    private void EquipItem(uint index)
    {
        Inventory[index].IsEquip = !Inventory[index].IsEquip;
    }

    /// <summary>
    /// ������ ������ �г��� �ݴ� �Լ�
    /// </summary>
    private void OnCloseDetail()
    {
        detailUI.ClearText();
        detailUI.CloseItemDetail();
    }
    // UI ����
    // UI �ݱ�
}