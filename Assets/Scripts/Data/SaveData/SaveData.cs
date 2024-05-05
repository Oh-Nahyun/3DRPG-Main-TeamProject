using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ������ ����ü
/// </summary>
[System.Serializable]
public struct PlayerData
{
    public Vector3 position;
    public Vector3 rotation;
    Inventory inventory;
    InventorySlot[] slots;
    public List<InventorySlot> slotLists;

    public PlayerData(Vector3 pos, Vector3 rot, Inventory inven)
    {
        this.position = pos;
        this.rotation = rot;  
        this.inventory = inven;

        // �κ��丮�� NULL�̸� �ӽ� ���� ���� �ο� ( 1�� )
        uint slotSize = this.inventory == null ? 1 : this.inventory.SlotSize;
        this.slots = new InventorySlot[slotSize];

        if(slotSize == 1)
        {
            this.slots[0] = new InventorySlot(0);
            slotLists = new List<InventorySlot>(0);
        }
        else
        {
            slotLists = new List<InventorySlot>((int)this.inventory.SlotSize);
            for (int i = 0; i < slotSize; i++)
            {
                this.slots[i] = inventory[(uint)i];
                slotLists.Add(slots[i]);
            }
            int a = 0;
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

    //public PlayerData[] playerInfos;
    public List<PlayerData> playerInfos;
}
