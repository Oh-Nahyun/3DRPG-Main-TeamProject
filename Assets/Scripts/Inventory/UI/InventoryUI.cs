using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// �κ��丮
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// �κ��丮 ���ٿ� ������Ƽ
    /// </summary>
    public Inventory Inventory => inventory;

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

    /// <summary>
    /// ������ ��� UI �г�
    /// </summary>
    InventoryGoldUI goldUI;

    /// <summary>
    /// ������ Ŭ���ϸ� ������ ������ �Ŵ� UI
    /// </summary>
    InventorySelectedMenuUI selectedMenuUI;

    CanvasGroup canvasGroup;

    /// <summary>
    /// �׷��װ� ���۵Ǹ� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<uint> onSlotDragBegin;

    /// <summary>
    /// �巡�װ� ������ �����ϴ� ��������Ʈ
    /// </summary>
    public Action<GameObject> onSlotDragEnd;

    /// <summary>
    /// �巡�װ� �����ϸ� �����ϴ� ��������Ʈ
    /// </summary>
    public Action onSlotDragEndFail;

    /// <summary>
    /// ���Կ� ���콺 �����͸� �ø��� ����Ǵ� ��������Ʈ ( ������ ������ �ҷ��´� )
    /// </summary>
    public Action<uint> onShowDetail;

    /// <summary>
    /// ���Կ� ���콺 �����Ͱ� ����� �����ϴ� ��������Ʈ ( ������ ����â�� �ݴ´� )
    /// </summary>
    public Action onCloseDetail;

    /// <summary>
    /// ������ ������ ���� Ŭ���� �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<uint> onLeftClickItem;
    
    /// <summary>
    /// ������ ������ ������ Ŭ���� �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<uint, Vector2> onRightClickItem;

    /// <summary>
    /// ������ ��ȣ�ۿ� �Ŵ��� ���ȴ��� Ȯ���ϴ� ����
    /// </summary>
    private bool isOpenedMenuPanel = false;

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
        goldUI = GetComponentInChildren<InventoryGoldUI>(); // ������ ��� UI
        selectedMenuUI = GetComponentInChildren<InventorySelectedMenuUI>(); // ������ ���� �Ŵ� UI
        canvasGroup = GetComponent<CanvasGroup>();

        RefreshInventoryUI();
        tempSlotUI.InitializeSlotUI(Inventory.TempSlot);

        onSlotDragBegin += OnSlotDragBegin;
        onSlotDragEnd += OnSlotDragEnd;
        onShowDetail += OnShowDetail;
        onCloseDetail += OnCloseDetail;
        onSlotDragEndFail += OnSlotDragFail;
        onLeftClickItem += OnLeftClickItem;
        onRightClickItem += OnRightClickItem;
        dividUI.onDivid += DividItem;
        dividUI.onDrop += OnDropItem;
        sortUI.onSortItem += OnSortItem;

        Inventory.onInventoryGoldChange += goldUI.onGoldChange; // Iventory�� ��差�� ������ �� goldUI�� �����ǰ� �Լ� �߰�

        goldUI.onGoldChange?.Invoke(Inventory.Gold);            // ��� �ʱ�ȭ
    }

    /// <summary>
    /// �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sortMode">������ ���� ���</param>
    /// <param name="isAcending">true�� ��������, false�� ��������</param>
    private void OnSortItem(uint sortMode, bool isAcending)
    {
        if (isOpenedMenuPanel)
            return;

        // �������� ���������� ������ �������� ����� �����ϱ�

        Inventory.SortSlot((SortMode)sortMode, isAcending);        
    }

    /// <summary>
    /// ���� �巡�� ����
    /// </summary>
    /// <param name="index">�ӽ� ���Կ� �� �κ��丮 ���� �ε���</param>
    private void OnSlotDragBegin(uint index)
    {
        if (isOpenedMenuPanel)
            return;

        if (Inventory[index].SlotItemData != null)
        {
            // index���� ���� ���� �ӽ� ����
            uint targetSlotIndex = index;
            uint targetSlotItemCode = (uint)Inventory[index].SlotItemData.itemCode;
            int targetItemSlotCount = Inventory[index].CurrentItemCount;
            bool targetIsEquip = Inventory[index].IsEquip;

            // tempSlot�� �巡�׸� ������ ������ ������ ������ ����
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
        if (isOpenedMenuPanel || !tempSlotUI.IsOpen)
            return;

        if (slotObj == null) // �巡�� ���� ������ �����Ǵ� ������ ����.
        {
            OnSlotDragFail();
            Debug.Log("�������� �ʴ� ������Ʈ�Դϴ�");
            return;
        }
        else
        {
            SlotUI_Base slotUI = slotObj.GetComponent<SlotUI_Base>();
            bool isSlot = slotUI is SlotUI_Base;

            if (slotUI == null)
            {
                OnSlotDragFail();
                Debug.Log("������ ���� ���̽� ��ũ��Ʈ�� �������� �ʴ� ������Ʈ �Դϴ�.");
                return;
            }

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

                    // ��� ��ġ �ٲٱ�
                    IEquipTarget equipTarget = Inventory.Owner.GetComponent<IEquipTarget>();    // �κ��丮�� ���� ������Ʈ
                    ItemData_Equipment itemData = Inventory[index].SlotItemData as ItemData_Equipment; // ������ �κ��丮�� ������ ������
                    if (itemData != null && Inventory[index].IsEquip)    // �������� ����̴�
                    {
                        equipTarget.EquipPart[(int)itemData.equipPart] = Inventory[index];  // ��� ������ ���� ���� 
                    }

                    Inventory.AccessTempSlot(index, targetSlotItemCode, targetSlotItemCount); // target ���Կ� �־��� ������ ���� �ӽ� ���Կ� ����
                    Inventory.TempSlot.IsEquip = targetSlotIsEquip;

                    tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
                    tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;
                    tempSlotIsEqiup = Inventory.TempSlot.IsEquip;

                    Inventory[tempFromIndex].IsEquip = tempSlotIsEqiup;
                    Inventory.AccessTempSlot(tempFromIndex, tempSlotItemCode, tempSlotItemCount);
                }
            }
            else // �������� ������� ������
            {
                Inventory[tempFromIndex].IsEquip = Inventory[index].IsEquip; // ���� ĭ�� �������δ� target�� ���� ���η� ����

                Inventory[index].IsEquip = tempSlotIsEqiup;
                Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount);

                IEquipTarget equipTarget = Inventory.Owner.GetComponent<IEquipTarget>();    // �κ��丮�� ���� ������Ʈ
                ItemData_Equipment itemData = Inventory[index].SlotItemData as ItemData_Equipment; // ������ �κ��丮�� ������ ������
                if(itemData != null && Inventory[index].IsEquip)    // �������� ����̰� �������̴�
                {
                    equipTarget.EquipPart[(int)itemData.equipPart] = Inventory[index];  // ��� ������ ���� ����
                }
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
        if (isOpenedMenuPanel)  // �޴� �г��� ���������� ����
            return;

        // ������ �����Ͱ� ������ ������ ���� �����ֱ�
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
    /// ������ ���� Ŭ���� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ŭ���� ���� �ε���</param>
    private void OnLeftClickItem(uint index)
    {
        bool isEquip = Inventory[index].SlotItemData is IEquipable; // ��� �������̸� true �ƴϸ� false
        if (isEquip)    // Ŭ���� ���� �������� ����̸�
        {
            EquipItem(index);   // ������ ���
        }
        bool isConsumalbe = Inventory[index].SlotItemData is IConsumable; // ȸ�� �������̸� true �ƴϸ� false
        if(isConsumalbe)
        {
            ConsumItem(index);  // ������ �Һ�
        }
    }

    /// <summary>
    /// ������ ������ Ŭ���� �� �����ϴ� �Լ� (Slot Menu)
    /// </summary>
    /// <param name="index">Ŭ���� ���� �ε���</param>
    private void OnRightClickItem(uint index, Vector2 position)
    {
        isOpenedMenuPanel = true;

        // ��ư �̺�Ʈ �ο� index�� ���Կ� ���� ���� 

        // ������ ������
        selectedMenuUI.OnDividButtonClick = () =>
        {
            if (Inventory[index].CurrentItemCount <= 1)
            {
                Debug.Log($"[{Inventory[index].SlotItemData.itemName}]�� �������� [{Inventory[index].CurrentItemCount}]�� �ֽ��ϴ�.");
                return;
            }
            dividUI.InitializeValue(Inventory[index], 1, (int)Inventory[index].CurrentItemCount - 1); // �г� �ʱ�ȭ
            dividUI.DividUIOpen(DividPanelType.Divid);

            selectedMenuUI.HideMenu();
            isOpenedMenuPanel = false;
        };

        // ������ ���
        selectedMenuUI.OnDropButtonClick = () =>
        {
            //DropItem(index);
            dividUI.InitializeValue(Inventory[index], 1, (int)Inventory[index].CurrentItemCount); // �г� �ʱ�ȭ
            dividUI.DividUIOpen(DividPanelType.Drop);
            selectedMenuUI.HideMenu();
            isOpenedMenuPanel = false;
        };

        selectedMenuUI.SetPosition(position);
        selectedMenuUI.ShowMenu(); // �Ŵ� �����ֱ�
    }

    private void OnDropItem(InventorySlot slot, int count)
    {
        DropItem(slot.SlotIndex, count);
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

    /// <summary>
    /// �������� ������ �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="index">������ �������� �ε���</param>
    private void EquipItem(uint index)
    {
        if (isOpenedMenuPanel)
            return;

        IEquipable equipable = Inventory[index].SlotItemData as IEquipable;

        bool isEquip = Inventory[index].IsEquip;
        if (equipable != null)
        {
            if (isEquip) // ������ �������� ������ �Ǿ������� ��������
            {
                equipable.UnEquipItem(Inventory.Owner);
                Inventory[index].IsEquip = false;
            }
            else if (!isEquip) // ������ �ȵ������� ����
            {
                IEquipTarget equipTarget = Inventory.Owner.GetComponent<IEquipTarget>();    // �κ��丮�� ���� ������Ʈ�� IEquipTarget
                ItemData_Equipment itemData = Inventory[index].SlotItemData as ItemData_Equipment;  // �����Ϸ��� ������ ������

                /*// ���������� �������� Ȯ���ϴ� �ּ�
                  //int partInedex = (int)itemData.equipPart;   // �����ҷ��� ��� ��ġ �ε���
                  InventorySlot equipedItem = equipTarget.EquipPart[partInedex];
                  if (equipedItem != null)  // ������ �ش� ������ �������� �ִ�
                  {
                      Inventory[equipedItem.SlotIndex].IsEquip = false; // ������ ������ ���� ��������
                  }*/

                // 0 �����ʼ�, 1 �޼� ��������Ȯ��
                for(int i = 0; i <= (int)EquipPart.Hand_L; i++)
                {
                    if (equipTarget.EquipPart[i] != null) // �� ���������� �������� �ִ�.
                    {
                        uint equipIndex = equipTarget.EquipPart[i].SlotIndex; // ���������� �κ��丮 ���� �ε���
                        inventory[equipIndex].IsEquip = false;  // ������ �������� ( UI )

                        IEquipable equipedItem = Inventory[equipIndex].SlotItemData as IEquipable; // ���������� ���� �������̽� ����
                        equipedItem.UnEquipItem(Inventory.Owner); // ������ ĳ������ ������ ��������
                    }
                }

                equipable.EquipItem(Inventory.Owner, Inventory[index]); // ���� ������ ����                
                Inventory[index].IsEquip = true;                        // ������ ���� ( UI )
            }
        }
    }

    /// <summary>
    /// ������ �Һ��� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="index">�Һ��� ������ ���� �ε���</param>
    private void ConsumItem(uint index)
    {
        if (isOpenedMenuPanel)
            return;

        IConsumable consumable = Inventory[index].SlotItemData as IConsumable;

        consumable.Consum(Inventory.Owner, Inventory[index]);
    }

    /// <summary>
    /// �κ��丮���� �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="index"></param>
    private void DropItem(uint index, int count)
    {
        if(!Inventory[index].IsEquip)   // ���� �������� �������¸� ����
        {
            for(int i = 0; i < count; i++)
            {
                if (Inventory[index].CurrentItemCount <= 0) // ������ ���� Ȯ�� ( 0���� ������ X)
                {
                    Debug.Log("���� ������ �����Ͱ� �������� �ʽ��ϴ�.");
                }

                Inventory.DropItem(index); // ������ ���
            }
        }            
    }

    /// <summary>
    /// ������ ������ �г��� �ݴ� �Լ�
    /// </summary>
    private void OnCloseDetail()
    {
        detailUI.ClearText();
        detailUI.CloseItemDetail();
    }

    /// <summary>
    /// �κ��丮 UI�� ���� �Լ�
    /// </summary>
    public void ShowInventory()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        RefreshInventoryUI();
    }

    /// <summary>
    /// �κ��丮 UI�� �ݴ� �Լ�
    /// </summary>
    public void CloseInventory()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        RefreshInventoryUI();
    }

    /// <summary>
    /// �κ��丮 ������ �ʱ�ȭ �ϴ� �Լ� 
    /// </summary>
    public void RefreshInventoryUI()
    {
        for (uint i = 0; i < Inventory.SlotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(Inventory[i]); // �κ��丮������ slotUI�� ����
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// �� ���� ���� ����Ȯ�� �Լ�
    /// </summary>
    void showEquip()
    {
        foreach (var items in slotsUIs)
        {
            Debug.Log($"{items.InventorySlotData.SlotIndex} : {items.InventorySlotData.IsEquip}");
        }
    }
#endif
}