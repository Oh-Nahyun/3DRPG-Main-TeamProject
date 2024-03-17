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
    /// ������ �ڵ�
    /// </summary>
    ItemData itemData;
    public ItemData SlotItem => itemData;

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
    /// <param name="data">������ ������</param>
    /// <param name="AddCount">�߰��� ������ ����</param>
    public void AddItem(ItemData data, int AddCount)
    {
        itemData = data;
        currentItemCount += AddCount;
    }

    //����
    public void DiscardItem(int discardCount)
    {
        currentItemCount -= discardCount;
    }
    //clear
}
