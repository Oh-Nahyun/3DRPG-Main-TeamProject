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
    InventorySlotUI[] slotsUIs;

    /// <summary>
    /// �κ��丮 UI�� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="playerInventory">�÷��̾� �κ��丮</param>
    public void InitializeInventoryUI(Inventory playerInventory)
    {
        inventory = playerInventory;    // �ʱ�ȭ�� �κ��丮 ���� �ޱ�
        slotsUIs = new InventorySlotUI[Inventory.slotSize]; // ���� ũ�� �Ҵ�
        slotsUIs = GetComponentsInChildren<InventorySlotUI>();  // ������Ʈ �ޱ�

        for (uint i = 0; i < Inventory.slotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(Inventory[i]); // �κ��丮������ slotUI�� ����
        }
    }

    // UI ����
    // UI �ݱ�
}
