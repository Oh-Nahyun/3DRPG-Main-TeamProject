using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class SellPanelUI : MonoBehaviour
{
    /// <summary>
    /// ������ ���� UI
    /// </summary>
    InventoryDetailUI detailUI;

    /// <summary>
    /// ������ �Ĵ� ������ �����ϴ� UI
    /// </summary>
    SellCountUI sellCountUI;

    /// <summary>
    /// Check �г�
    /// </summary>
    SellCheckUI sellCheckUI;

    /// <summary>
    /// ��ǥ ������Ʈ�� �κ��丮 Ŭ����
    /// </summary>
    Inventory targetInventory;

    /// <summary>
    /// ������ ��� ���� ����
    /// </summary>
    SellSlotUI[] slots;

    /// <summary>
    /// �Ǹ��ҷ��� �������� ����
    /// </summary>
    InventorySlot targetSlot;

    /// <summary>
    /// �г� �ݴ� ��ư
    /// </summary>
    Button closeButton;

    /// <summary>
    /// �κ��丮 ���� ������
    /// </summary>
    public GameObject invenSlotPrefab;
    CanvasGroup canvasGroup;

    /// <summary>
    /// target�� �κ��丮 ������
    /// </summary>
    uint inventorySize = 0;

    /// <summary>
    /// �ްԵ� ���� ����
    /// </summary>
    uint totalGetGold = 0;

    /// <summary>
    /// �ȰԵ� ���� ������ ��
    /// </summary>
    int totalSellItemCount = 0;

    /// <summary>
    /// ù �������� Ȯ���ϴ� ���� (���� �������� true, �ѹ��̶� ���������� false)
    /// </summary>
    bool isFirst = true;

    /// <summary>
    /// ���� �ǸŰ� ���������� Ȯ���ϴ� ���� (�ǸŰ� �������̸� true �ƴϸ� false)
    /// </summary>
    bool isProcess = false;

    /// <summary>
    /// isProcess�� �����ϱ����� ������Ƽ (�ǸŰ� �������̸� true �ƴϸ� false)
    /// </summary>
    public bool IsProcess => isProcess;

    public Action<uint> onShowDetail;
    public Action onCloseDetail;
    public Action<uint> onShowCheckPanel;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        detailUI = GetComponentInChildren<InventoryDetailUI>();
        sellCountUI = GetComponentInChildren<SellCountUI>();
        sellCheckUI = GetComponentInChildren<SellCheckUI>();
        closeButton = transform.GetChild(4).GetComponent<Button>();
        closeButton.onClick.AddListener(() =>
        {
            CloseSellUI();
        });

        onShowDetail += OnShowDetail;
        onCloseDetail += OnCloseDetail;
        onShowCheckPanel += OnShowSellCount;
        sellCountUI.onSell += OnSellCheck;
        sellCountUI.onCloseSellCount += OnSellClose;
        sellCheckUI.onConformSell += OnConformSellItem;      
    }

    /// <summary>
    /// ������ ������ �ҷ��ͼ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="index">�κ��丮 �ε���</param>
    public void OnShowDetail(uint index)
    {
        if (slots[index].InventorySlotData.SlotItemData != null)
        {
            string name = slots[index].InventorySlotData.SlotItemData.itemName;
            string desc = slots[index].InventorySlotData.SlotItemData.desc;
            uint price = slots[index].InventorySlotData.SlotItemData.price;

            detailUI.SetDetailText(name, desc, price);
            detailUI.ShowItemDetail();
        }
    }

    /// <summary>
    /// ������ ����â�� �ݴ� �Լ�
    /// </summary>
    public void OnCloseDetail()
    {
        detailUI.ClearText();
        detailUI.CloseItemDetail();
    }

    /// <summary>
    /// �Ǹ�â�� ���� �Լ�
    /// </summary>
    public void OpenSellUI()
    {
        if (targetInventory == null)
        {
            Debug.Log($"targetInventory�� �����ϴ�.");
            return;
        }

        SetSlot();
        isFirst = false;
        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// �Ǹ�â�� �ݴ� �Լ�
    /// </summary>
    public void CloseSellUI()
    {
        canvasGroup.alpha = 0f;
        targetInventory = null;
        isProcess = false;
    }

    /// <summary>
    /// SellCountUI ���� �� �����ϴ� �Լ�
    /// </summary>
    private void OnSellClose()
    {
        isProcess = false;
    }

    /// <summary>
    /// SellCount �г��� ���� �Լ�
    /// </summary>
    /// <param name="index">�Ǹ��� ������ ����</param>
    public void OnShowSellCount(uint index)
    {
        if(targetInventory[index] == null)
        {
            Debug.Log("�������� �������� �ʽ��ϴ�.");
            return; 
        }

        if (targetInventory[index].IsEquip)
        {
            Debug.Log("�ش� �������� ���� ���Դϴ�. �Ǹ��� �� �����ϴ�.");
            return;
        }

        isProcess = true;

        // �������� �������� ������ â ����
        if (targetInventory[index].CurrentItemCount > 1)
        {
            sellCountUI.InitializeValue(targetInventory[index], 1, targetInventory[index].CurrentItemCount);
            sellCountUI.SellCountUIOpen();
        }
        else // �������� 1����
        {
            OnSellCheck(targetInventory[index], 1);
        }
    }

    /// <summary>
    /// Ȯ�� â�� ���̰��ϴ� �Լ�
    /// </summary>
    /// <param name="slot">�Ǹ��� ������ ����</param>
    /// <param name="count">�Ǹ��� ����</param>
    private void OnSellCheck(InventorySlot slot, int count)
    {
        // Ȯ�� â ����
        sellCheckUI.ShowCheckPanel();
        sellCheckUI.onCheckSell(slot, count);

        targetSlot = slot;
        totalGetGold = targetInventory[slot.SlotIndex].SlotItemData.price * (uint)count;
        totalSellItemCount = count;
    }

    /// <summary>
    /// ������ ��°� Ȯ�� �Լ�
    /// </summary>
    private void OnConformSellItem()
    {
        targetInventory.AddGold(totalGetGold);
        targetInventory[targetSlot.SlotIndex].DiscardItem(totalSellItemCount);
    }


    /// <summary>
    /// ���� ������Ʈ�� �κ��丮�� ã�� �Լ�
    /// </summary>
    /// <param name="target">�κ��丮 ��ü�� �ִ� ���� ������Ʈ ( Player )</param>
    public void GetTarget(Inventory inventory)
    {
        targetInventory = inventory; // target�� �κ��丮 ����
    }


    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    void SetSlot()
    {
        Transform targetInventoryPanelUI = transform.GetChild(0);

        if(isFirst)
        {
            inventorySize = targetInventory.SlotSize;
            slots = new SellSlotUI[inventorySize];

            for(int i = 0; i < inventorySize; i++)
            {
                Instantiate(invenSlotPrefab, targetInventoryPanelUI);
            }
        }

        for(uint i = 0; i < inventorySize; i++)
        {
             SellSlotUI slotUI = targetInventoryPanelUI.GetChild((int)i).GetComponent<SellSlotUI>();
             slotUI.InitializeSlotUI(targetInventory[i]);
             slots[i] = slotUI;
        }        
    }
}