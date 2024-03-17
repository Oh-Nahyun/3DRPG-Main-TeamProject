using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ��丮 Ŭ����
/// </summary>
public class Inventory
{
    // ���Ե����

    const uint maxSlot = 6;

    public InventorySlot[] slots;

    public Inventory()
    {
        slots = new InventorySlot[maxSlot];

        for(int i = 0; i < maxSlot; i++)
        {
            slots[i] = new InventorySlot((uint)i);
        }
    }

    /// <summary>
    /// ���Կ� ������ �߰� ( �� �� ���Կ� �߰� )
    /// </summary>
    /// <param name="data">������ �ڵ�</param>
    /// <param name="n">������ ����</param>
    public void AddItem(int code, int n)
    {
        for(uint i = 0; i < maxSlot; i++)
        {
            if(slots[i].SlotItem == null)
            {   // check Slot is Empty
                slots[i].AddItem(ItemDataManager.itemDataManager[code], n);
                return;
            }
        }
    }

    /// <summary>
    /// Ư�� ���Կ� ������ �߰�
    /// </summary>
    /// <param name="data">������ �ڵ�</param>
    /// <param name="n">������ ����</param>
    /// <param name="index">���� ��ġ</param>
    public void AddItem(ItemData data, uint n, uint index)
    {
        if(index > maxSlot)
        {
            Debug.Log($"�������� �ʴ� �����Դϴ�.");
            return;
        }
    }

#if UNITY_EDITOR

    string str;
    public void ShowInventory()
    {
        for(int i = 0; i < maxSlot; i++)
        {
            if(slots[i].SlotItem != null)
            {
                str += $"{slots[i].SlotItem.itemName}" +
                       $"{slots[i].CurrentItemCount} / " +
                       $"{slots[i].SlotItem.maxCount}";
            }
            else
            {
                str += $"��ĭ";
            }
            str += $", ";
        }

        Debug.Log(str);
    }
#endif
}
