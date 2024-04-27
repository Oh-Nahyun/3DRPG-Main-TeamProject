using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleInfo : MonoBehaviour
{
    InventoryUI inventoryUI;

    /// <summary>
    /// UI slots
    /// </summary>
    InventorySlotUI[] slotsUIs;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    private void Start()
    {
       
    }
    public void InitializeInventoryUI(Inventory playerInventory)
    {
        slotsUIs = GetComponentsInChildren<InventorySlotUI>();  // �Ϲ� ����

        for (uint i = 0; i < inventoryUI.Inventory.SlotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(inventoryUI.Inventory[i]); // �κ��丮������ slotUI�� ����
        }
    }
}