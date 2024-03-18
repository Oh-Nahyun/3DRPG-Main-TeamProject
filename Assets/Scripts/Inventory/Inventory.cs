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
        if (slots[index].SlotItemData != null) // �ش� ���Կ� �������� �����Ѵ� (1���̻�)
        {            
            if(slots[index].CurrentItemCount == slots[index].SlotItemData.maxCount) // ������ �� á���� üũ
            {                
                Debug.Log($"slot[{index}] is Full");

                uint vaildIndex = FindEmptySlot(index); // ���� ĭ üũ
                if (vaildIndex > maxSlot)
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
                    uint vaildIndex = FindEmptySlot(index); // ����ִ� ĭ Ȯ��

                    if (vaildIndex > maxSlot)
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
    
    /// <summary>
    /// ���� ������ȣ�� ����ִ� ������ ã���ִ� �Լ�
    /// </summary>
    /// <param name="start">���� �ε���</param>
    uint FindEmptySlot(uint start)
    {
        uint index = start;
        foreach(var slot in slots)
        {
            if (slot.SlotItemData == null) // �����Ͱ� ������ break
                break;

            index++;
        }

        return index;
    }
}