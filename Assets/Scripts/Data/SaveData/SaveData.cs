using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// �������ڵ�� ������ �����ϴ� Ŭ����
/// </summary>
[System.Serializable]
public class ItemDataClass
{
    /// <summary>
    /// ����� ������ �ڵ�
    /// </summary>
    public int itemCode;

    /// <summary>
    /// ����� ������ ����
    /// </summary>
    public int count;
}

/// <summary>
/// �÷��̾� ������ ����ü
/// </summary>
[System.Serializable]
public struct PlayerData
{
    /// <summary>
    /// �÷��̾� ��ġ��
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// �÷��̾� ȸ����
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// �÷��̾� �κ��丮 - �����ҷ��� �÷��̾� �κ��丮 ���ٿ�
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// �÷��̾� �κ��丮 ����
    /// </summary>
    InventorySlot[] slots;

    public ItemDataClass[] itemDataClass;

    /// <summary>
    /// ���̺� ������ ĭ ��
    /// </summary>
    const int saveCount = 5;

    public PlayerData(Vector3 pos, Vector3 rot, Inventory inven)
    {
        this.position = pos;
        this.rotation = rot;  
        this.inventory = inven;

        // �κ��丮�� NULL�̸� �ӽ� ���� ���� �ο� ( 1�� )
        uint slotSize = this.inventory == null ? 1 : this.inventory.SlotSize;
        this.slots = new InventorySlot[slotSize];                       // ���� �ʱ�ȭ
        this.itemDataClass = new ItemDataClass[slotSize];

        if(slotSize == 1) // �κ��丮�� NULL�̸�
        {
            this.slots[0] = new InventorySlot(0);
            this.itemDataClass[0] = new ItemDataClass();
        }
        else // �κ��丮�� NULL�� �ƴϸ�
        {
            // �κ��丮 ���� ������ �ʱ�ȭ
            for (int i = 0; i < slotSize; i++)
            {
                this.slots[i] = inventory[(uint)i]; // ���� ������ �߰�
            }

            // ������ ������ �ʱ�ȭ
            for(int i = 0; i < saveCount; i++)
            {
                if (slots[i].SlotItemData == null)
                {
                    continue;
                }
                else
                {
                    this.itemDataClass[i] = new ItemDataClass();
                    itemDataClass[i].itemCode = (int)slots[i].SlotItemData.itemCode;
                    itemDataClass[i].count = slots[i].CurrentItemCount;
                }
            }
        }
    }
}

/// <summary>
/// json ���� ����� Ŭ���� ( Scene��ȣ, �÷��̾� ��ġ, �÷��̾� �κ��丮)
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// ����� �� ��ȣ
    /// </summary>
    public int[] SceneNumber;

    public PlayerData[] playerInfos;
}