using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class SaleInfo : MonoBehaviour
{
    InventoryUI inventoryUI;
    Inventory inven;
    /// <summary>
    /// UI slots
    /// </summary>
    SaleInfoSolt[] saleInfoSolts;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        inven = inventoryUI.Inventory;
    }

    private void Start()
    {
        InitializeInventoryUI(inven);
    }

    public void InitializeInventoryUI(Inventory playerInventory)
    {
        saleInfoSolts = GetComponentsInChildren<SaleInfoSolt>();  // �Ϲ� ����
        for (uint i = 0; i < inventoryUI.Inventory.SlotSize; i++)
        {
            saleInfoSolts[i].InitializeSlotUI(inventoryUI.Inventory[i]); // �κ��丮������ slotUI�� ����
        }
    }
}