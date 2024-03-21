using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// �κ��丮
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// �κ��丮 ���ٿ� ������Ƽ
    /// </summary>
    Inventory Inventory => inventory;

    /// <summary>
    /// UI slots
    /// </summary>
    InventorySlotUI[] slots;

    /// <summary>
    /// UI ���� ���ٿ� �ε���
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public InventorySlotUI this[uint index] => slots[index];     

    void Awake()
    {
        inventory = new Inventory();
        slots = new InventorySlotUI[Inventory.slotSize];

        for(uint i = 0; i < Inventory.slotSize; i++)
        {
            slots = GetComponentsInChildren<InventorySlotUI>();
            slots[i].SlotData = Inventory[i]; 
        }

        int a = 0;
    }
}
