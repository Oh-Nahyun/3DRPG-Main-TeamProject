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
    int currentItemCount = 0;
    public int CurrentItemCount => currentItemCount;

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
    /// <param name="count">�߰��� ���� (Default = 1)</param>
    public void AddItem(int code, int count = 1)
    {
        
        itemData = ItemDataManager.Instance.datas[code];
        currentItemCount += count;
    }

    //����
    public void DiscardItem(int discardCount)
    {
        currentItemCount -= discardCount;
    }

    //clear
    public void ClearItem()
    {
        itemData = null;
        currentItemCount = 0;
    }
}
