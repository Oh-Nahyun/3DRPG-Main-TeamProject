using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� Ŭ����
/// </summary>
public class InventorySlot
{
    // ���� ������ ����
    // ����, �������ڵ�
    // ������ ���� ����, ����

    /// <summary>
    /// ���� �ε���
    /// </summary>
    uint slotIndex;
    
    /// <summary>
    /// ���� �ε��� ���� ������Ƽ
    /// </summary>
    public uint SlotIndex => slotIndex;

    /// <summary>
    /// ������ ������
    /// </summary>
    ItemData itemData = null;
    public ItemData SlotItemData
    {
        get => itemData;
        private set
        {
            if (itemData != value)
            {
                itemData = value;

                onChangeSlotData?.Invoke();
            }
        }
    }

    /// <summary>
    /// Current Item count
    /// </summary>
    int currentItemCount = 0;

    /// <summary>
    /// ������ ���� ������ ���� ������Ƽ
    /// </summary>
    public int CurrentItemCount
    {
        get => currentItemCount;
        set
        {
            if (currentItemCount != value)
            {
                currentItemCount = value;
                onChangeSlotData?.Invoke();
            }
        }
    }

    //���� ����

    /// <summary>
    /// ������ ������ ������ �˸��� ��������Ʈ
    /// </summary>
    public Action onChangeSlotData;

    /// <summary>
    /// InventorySlot ������
    /// </summary>
    /// <param name="index"></param>
    public InventorySlot(uint index)
    {
        slotIndex = index;
        SlotItemData = null;
        CurrentItemCount = 0;
    }

    /// <summary>
    /// ������ �߰� �Լ�
    /// </summary>
    /// <param name="code">������ �ڵ�</param>
    /// <param name="count">�߰��� ����</param>
    public virtual void AssignItem(uint code, int count, out int over)
    {
        int overCount = 0;
        // ��ģ�ٸ�?
        SlotItemData = ItemDataManager.Instance.datas[code];
        CurrentItemCount += count;  // add item

        if (CurrentItemCount > SlotItemData.maxCount)
        {
            overCount = CurrentItemCount - (int)SlotItemData.maxCount;  // ������ �ʰ��ϴ� ������

            CurrentItemCount = (int)SlotItemData.maxCount;
        }
        over = overCount;
    }

    //����
    public void DiscardItem(int discardCount)
    {
        CurrentItemCount -= discardCount;
        if (CurrentItemCount < 1)
        {
            CurrentItemCount = 0;
            ClearItem();
        }
    }

    //clear
    public virtual void ClearItem()
    {
        SlotItemData = null;
        CurrentItemCount = 0;
    }
}
