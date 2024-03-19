using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ��丮 Ŭ����
/// </summary>
public class Inventory
{
    /// <summary>
    /// �ִ� ���԰���
    /// </summary>
    const uint maxSlot = 6;

    /// <summary>
    /// �ӽ� ���� �ε���
    /// </summary>
    const uint tempIndex = 999999;

    /// <summary>
    /// �κ��丮 ���Ե�
    /// </summary>
    public InventorySlot[] slots;

    List<InventorySlot> tempList;

    /// <summary>
    /// �ӽ� ����
    /// </summary>
    TempSlot tempslot;

    public Inventory()
    {
        slots = new InventorySlot[maxSlot];
        tempList = new List<InventorySlot>();

        for(int i = 0; i < maxSlot; i++)
        {
            slots[i] = new InventorySlot((uint)i);
        }
    }

    /// <summary>
    /// ���Կ� ������ �߰�
    /// </summary>
    /// <param name="code">������ �ڵ�</param>
    /// <param name="count">�߰��� ������ ����</param>
    /// <param name="index">�߰��� ���� ��ġ �ε���</param>
    public void AddSlotItem(uint code, int count, uint index = 0)
    {
        int overCount = 0;
        uint vaildIndex = 0;

        if (index > maxSlot)
        {
            Debug.Log($"�������� �ʴ� ���� �ε��� �Դϴ�.");
        }

        if (slots[index].SlotItemData != null) // �ش� ���Կ� �������� �����Ѵ� (1���̻�)
        {
            if(slots[index].CurrentItemCount == slots[index].SlotItemData.maxCount) // ������ �� á���� üũ
            {                
                Debug.Log($"slot[{index}] is Full");

                // �����ִ� ���� ã��
                vaildIndex = FindSlot(code); // ���� ĭ üũ
                if (vaildIndex >= maxSlot) // ��� ������ ���� á����
                {
                    Debug.Log($"����ִ� ������ �����ϴ�.");
                    return;
                }
                else // ��� ������ �� ��á����
                {
                    slots[vaildIndex].AssignItem(code, count, out overCount);
                }
            }
            else
            {
                if (slots[index].SlotItemData.itemCode != ((ItemCode)code)) // �߰��Ϸ��� �������� ���� �ٸ� �������̴�.
                {
                    vaildIndex = FindSlot(code); // ����ִ� ĭ Ȯ��

                    if (vaildIndex >= maxSlot)
                    {
                        Debug.Log($"����ִ� ������ �����ϴ�.");
                        return;
                    }
                    else
                    {
                        slots[vaildIndex].AssignItem(code, count, out overCount);
                    }

                }
                else // �߰��Ϸ��� �������� ���� ���� �������̴�.
                {
                    slots[index].AssignItem(code, count, out overCount);  // ������ �߰�
                }
            }
        }
        else // �ش� ���Կ� �������� ����.
        {
            slots[index].AssignItem(code, count, out overCount);  // ������ �߰�
        }


        // ���� �� �߰�
        if (overCount == 0) return; // ��ġ�°� ������ ����
        else
        {
            vaildIndex = FindSlot(code);
            if(vaildIndex < maxSlot)
            {
                slots[vaildIndex].AssignItem(code, overCount, out _);
            }
            else
            {
                Debug.Log($"�κ��丮�� ����á���ϴ�.");
            }
        }
    }

    /// <summary>
    /// ���Կ� ������ ����
    /// </summary>
    /// <param name="count">������ ���� ����</param>
    /// <param name="index">������ ���� ��ġ �ε���</param>
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
    uint FindSlot(uint code)
    {
        uint index = 0;
        foreach(var slot in slots)
        {
            if (slot.SlotItemData == null) // �����Ͱ� ����ִ�
                break;
            else if (slot.SlotItemData.itemCode == (ItemCode)code &&
                     slot.CurrentItemCount < slot.SlotItemData.maxCount)
                break;

            index++;
        }

        return index;
    }

    /// <summary>
    /// ���� �����Լ� (a�ε��� ���԰� b�ε��� ������ ��ü�Ѵ�)
    /// </summary>
    /// <param name="indexA">������ ���� �ε��� 1</param>
    /// <param name="indexB">������ ���� �ε��� 2</param>
    public void SwapSlot(uint indexA, uint indexB)
    {   
        if(indexA == indexB)
        {
            Debug.Log($"�ε������� �����մϴ�.");
            return;
        }

        uint tempIndex = slots[indexA].SlotIndex;   // ���� �ε���        
        ItemData tempItemdata = slots[indexA].SlotItemData; // ������ ������        
        int tempItemCount = slots[indexA].CurrentItemCount; // ������ ���� ������ ����

        slots[indexA].ClearItem();
        slots[indexA].AssignItem((uint)slots[indexB].SlotItemData.itemCode,
                              slots[indexB].CurrentItemCount,
                              out _);

        slots[indexB].ClearItem();
        slots[indexB].AssignItem((uint)tempItemdata.itemCode,
                              tempItemCount,
                              out _);
    }

    /// <summary>
    /// �����ϴ� �Լ�
    /// </summary>
    /// <param name="sortMode"></param>
    public void SortSlot(SortMode sortMode, bool isAcending)
    {
        tempList = new List<InventorySlot>(slots);

        switch(sortMode)
        {
            case SortMode.Name:
                tempList.Sort((current, other) =>
                {
                    if(current.SlotItemData == null)
                        return 1;
                    if(other.SlotItemData == null)
                        return -1;
                    if(isAcending)
                    {
                        return current.SlotItemData.itemName.CompareTo(other.SlotItemData.itemName);
                    }
                    else
                    {
                        return other.SlotItemData.itemName.CompareTo(current.SlotItemData.itemName);
                    }
                });
                break;
            case SortMode.Price:
                tempList.Sort((current, other) =>
                {
                    if (current.SlotItemData == null)
                        return 1;
                    if (other.SlotItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return current.SlotItemData.price.CompareTo(other.SlotItemData.price);
                    }
                    else
                    {
                        return other.SlotItemData.price.CompareTo(current.SlotItemData.price);
                    }
                });
                break;
            case SortMode.Count:
                tempList.Sort((current, other) =>
                {
                    if (current.SlotItemData == null)
                        return 1;
                    if (other.SlotItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return current.CurrentItemCount.CompareTo(other.CurrentItemCount);
                    }
                    else
                    {
                        return other.CurrentItemCount.CompareTo(current.CurrentItemCount);
                    }
                });
                break;
        }

        int slotIndex = 0;
        foreach(var listIndex in tempList)
        {
            slots[slotIndex] = listIndex;
            slotIndex++;
        }

        tempList.Clear();
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