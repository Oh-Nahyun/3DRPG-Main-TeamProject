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
    ItemData itemData;
    public ItemData SlotItemData => itemData;

    /// <summary>
    /// Current Item count
    /// </summary>
    uint currentItemCount = 0;
    public uint CurrentItemCount => currentItemCount;

    //���� ����

    /// <summary>
    /// InventorySlot ������
    /// </summary>
    /// <param name="index"></param>
    public InventorySlot(uint index)
    {
        slotIndex = index;
        itemData = null;
        currentItemCount = 0;
    }

    /// <summary>
    /// ������ �߰� �Լ�
    /// </summary>
    /// <param name="code">������ �ڵ�</param>
    /// <param name="count">�߰��� ����</param>
    public void AddItem(uint code, uint count, out uint over)
    {
        uint overCount = 0;
        // ��ģ�ٸ�?
        itemData = ItemDataManager.Instance.datas[code];
        currentItemCount += count;  // add item

        if (currentItemCount > SlotItemData.maxCount)
        {
            overCount = currentItemCount - SlotItemData.maxCount;  // ������ �ʰ��ϴ� ������

            currentItemCount = SlotItemData.maxCount;
        }
        over = overCount;
    }

    //����
    public void DiscardItem(uint discardCount)
    {
        currentItemCount -= (uint)discardCount;
        if (currentItemCount < 1)
        {
            currentItemCount = 0;
            ClearItem();
        }
    }

    //clear
    public void ClearItem()
    {
        itemData = null;
        currentItemCount = 0;
    }
}
