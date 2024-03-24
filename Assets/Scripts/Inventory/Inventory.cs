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
    /// �κ��丮 ũ�� ���ٿ� ������Ƽ
    /// </summary>
    public uint slotSize => maxSlot;

    /// <summary>
    /// �κ��丮 ���Ե�
    /// </summary>
    InventorySlot[] slots;

    /// <summary>
    /// �κ��丮 ���� ������ ���� �ε���
    /// </summary>
    /// <param name="index">���� �ε���</param>
    /// <returns></returns>
    public InventorySlot this[uint index] => slots[index];

    /// <summary>
    /// ������ ������ ���� ����Ʈ
    /// </summary>
    List<InventorySlot> tempSortList;

    /// <summary>
    /// �ӽ� ���� Ŭ����
    /// </summary>
    TempSlot tempSlot;

    /// <summary>
    /// �ӽ� ���� ������ ���� ������Ƽ
    /// </summary>
    public TempSlot TempSlot => tempSlot;

    /// <summary>
    /// �ӽ� ���� �ε���
    /// </summary>
    const uint tempIndex = 999999;

    /// <summary>
    /// �κ��丮 ������
    /// </summary>
    public Inventory()
    {
        slots = new InventorySlot[maxSlot];
        tempSortList = new List<InventorySlot>();
        tempSlot = new TempSlot(tempIndex);

        for (int i = 0; i < maxSlot; i++)
        {
            slots[i] = new InventorySlot((uint)i);
        }
    }

    #region Legacy AddItem Method
/*    /// <summary>
    /// ���Կ� ������ �߰�
    /// </summary>
    /// <param name="code">������ �ڵ�</param>
    /// <param name="count">�߰��� ������ ����</param>
    /// <param name="index">�߰��� ���� ��ġ �ε���</param>
    public void AddSlotItem(uint code, int count, uint index = 0)
    {
        int overCount = 0;
        uint vaildIndex = 0;

        if (!IsVaildSlot(index))
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
    }*/
    #endregion

    /// <summary>
    /// ������ �߰� �Լ� , ���� �����ִ� ������ ä��
    /// </summary>
    /// <param name="code">������ �ڵ�</param>
    /// <param name="count">������ ����</param>
    /// <param name="index">���� �ε���</param>
    public void AddSlotItem(uint code, int count, uint index = 0)
    {
        if (index >= maxSlot) // ���� ��� Ȯ��
        {
            Debug.Log($"{index}�� ������ �������� �ʽ��ϴ�.");
            return;
        }

        if (index == 0) // index���� default���̸� �ڵ� �߰�
        {
            uint slotIndex = FindSlot(code);

            if(slotIndex >= maxSlot)
            {
                Debug.Log("�κ��丮�� ���� ���ֽ��ϴ�");
                return;
            }

             slots[slotIndex].AssignItem(code, count, out int overCount);

            if (overCount > 0) // ��ģ �������� �����Ѵٸ�
            {
                // ��Ž�� �� �ֱ�
                slotIndex = FindSlot(code);
                slots[slotIndex].AssignItem(code, overCount, out _);
            }
        }
        else // Ư�� �ε����� �߰�
        {
            slots[index].AssignItem(code, count, out int overCount);

            if (overCount > 0) // ��ģ �������� �����Ѵٸ�
            {
                // ��Ž�� �� �ֱ�
                uint slotIndex = FindSlot(code);

                if (slotIndex >= maxSlot)
                {
                    Debug.Log("�κ��丮�� ���� ���ֽ��ϴ�");
                    return;
                }
                else
                {
                    slots[slotIndex].AssignItem(code, overCount, out _);
                }
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
            else if (slot.SlotItemData.itemCode == (ItemCode)code &&        // �Ű����� ������ �ڵ�� �����ϰ� 
                     slot.CurrentItemCount < slot.SlotItemData.maxCount)    // �ش� ������ ������ ������ �ִ�ġ���� ���ٸ� ��ȯ
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
        tempSortList = new List<InventorySlot>(slots);

        switch(sortMode)
        {
            case SortMode.Name:
                tempSortList.Sort((current, other) =>
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
                tempSortList.Sort((current, other) =>
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
                tempSortList.Sort((current, other) =>
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
        foreach(var listIndex in tempSortList)
        {
            slots[slotIndex] = listIndex;
            slotIndex++;
        }

        tempSortList.Clear();
    }

    public void AccessTempSlot(uint index, uint itemCode, int itemCount)
    {
        if(TempSlot.SlotItemData == null) // �ӽ� ������ ����ִ�.
        {
            tempSlot.SetTempSlotIndex(index);
            tempSlot.AssignItem(itemCode, itemCount, out _); // temp���� ���� �߰�
        }
        else if(TempSlot.SlotItemData != null)  // �ӽ� ������ ������̴�.
        {
            slots[index].AssignItem(itemCode, itemCount, out _);

            tempSlot.ClearItem();
        }
    }

    /// <summary>
    /// ������ ������ �Լ�
    /// </summary>
    /// <param name="indexA">���� ����</param>
    /// <param name="indexB">���� ������ ���� ����</param>
    /// <param name="count">������ ����</param>
    public void DividItem(uint indexA, uint indexB, int count = 1)
    {
        if(indexA == indexB) // ���� �ε��� Ȯ��
        {
            Debug.Log($"�ε����� �����մϴ�. ���� �� �����ϴ�.");
            return;
        }

        if(slots[indexA].CurrentItemCount < 2) // ������ ���� Ȯ�� ( 1���ϸ� ���� X )
        {
            Debug.Log($"[{slots[indexA]}]�� ������ ������ [{slots[indexA].CurrentItemCount}] �Դϴ�. ���� �� �����ϴ�.");
            return;
        }

        if(count > slots[indexA].CurrentItemCount)
        {
            count = slots[indexA].CurrentItemCount;
        }

        uint itemCode = (uint)slots[indexA].SlotItemData.itemCode;
        slots[indexA].DiscardItem(count);
        slots[indexB].AssignItem(itemCode, count, out _);
    }

    /// <summary>
    /// �ش� �ε����� ���Կ� �������� �� �� �ִ� �� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ȯ���� �ε���</param>
    /// <returns>������ �����ϸ� true �ƴϸ� false</returns>
    public bool IsVaildSlot(uint index)
    {
        return slots[index].SlotItemData == null;
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