using System;
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

    // ���Կ� ������ �߰�
    public void AddSlotItem(int code, int count, uint index = 0)
    {
        if(index > maxSlot)
        {
            Debug.Log($"�������� �ʴ� ���� �ε��� �Դϴ�.");
        }

        if (slots[index].SlotItemData != null) // �ش� ���Կ� �������� �����Ѵ� (1���̻�)
        {            
            if(slots[index].CurrentItemCount == slots[index].SlotItemData.maxCount) // ������ �� á���� üũ
            {                
                Debug.Log($"slot[{index}] is Full");

                uint vaildIndex = FindSlot(code, index); // ���� ĭ üũ
                if (vaildIndex >= maxSlot)
                {
                    Debug.Log($"����ִ� ������ �����ϴ�.");
                    return;
                }
                else
                {
                    slots[vaildIndex].AddItem(code, count);
                }
            }
            else
            {
                if (slots[index].SlotItemData.itemCode != ((ItemCode)code)) // �߰��Ϸ��� �������� ���� �ٸ� �������̴�.
                {               
                    uint vaildIndex = FindSlot(code, index); // ����ִ� ĭ Ȯ��

                    if (vaildIndex >= maxSlot)
                    {
                        Debug.Log($"����ִ� ������ �����ϴ�.");
                        return;
                    }
                    else
                    {
                        slots[vaildIndex].AddItem(code, count);
                    }

                }
                else // �߰��Ϸ��� �������� ���� ���� �������̴�.
                {
                    slots[index].AddItem(code, count);  // ������ �߰�
                }
            }
        }
        else // �ش� ���Կ� �������� ����.
        {
            slots[index].AddItem(code, count);  // ������ �߰�
        }
    }

    public void DiscardSlotItem(int count, uint index = 0)
    {
        if(slots[index].SlotItemData == null)
        {
            Debug.Log($"������ ����ֽ��ϴ�.");
            return;
        }

        slots[index].DiscardItem(count);
    }
    
    /// <summary>
    /// ���� ������ȣ�� ����ִ� ������ ã���ִ� �Լ�
    /// </summary>
    /// <param name="code">ã�� ������ �ڵ�</param>
    /// <param name="start">���� �ε���</param>
    uint FindSlot(int code, uint start)
    {
        uint index = start;
        foreach(var slot in slots)
        {
            // ã�� ���� 
            if (slot.SlotItemData == null) // �����Ͱ� ����ִ�
                break;
            else if (slot.SlotItemData.itemCode == (ItemCode)code && 
                     slot.CurrentItemCount < slot.SlotItemData.maxCount)
                break;

            index++;
        }

        return index;
    }

#if UNITY_EDITOR
    public void TestShowInventory()
    {
        string str = null;
        str += $"(";
        for(int i = 0; i < maxSlot; i++)
        {
            if(slots[i].SlotItemData == null)
            {
                str += $"��ĭ";
            }
            else
            {
                str += $"[{slots[i].SlotItemData.itemName} ({slots[i].CurrentItemCount}/ {slots[i].SlotItemData.maxCount}]";
            }
            str += $", ";
        }
        str += $")";

        Debug.Log($"{str}");
    }
#endif
}