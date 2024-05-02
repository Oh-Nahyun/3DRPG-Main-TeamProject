using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������ ������Ʈ�� ItemData�� �����ϱ� ���� Ŭ����
/// </summary>
public class ItemDataObject : RecycleObject
{
    // Ȱ��ȭ �� �� �κ��丮�� ������ �ޱ�
    ItemData Data;          // ������ ������

    uint currentItemCode = 0;  // ������ �ڵ�

    void Start()
    {
        onDisable += OnItemDisable;
    }

    private void OnItemDisable()
    {
        Debug.Log($"������ ������");                  

        int childObjCount = transform.childCount;           // �ڽ� ������Ʈ ����

        Debug.Log($"{childObjCount}");

        for(int i = 0; i < childObjCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);      // ��� �ڽ� ������Ʈ �ı�
        }
    }

    /// <summary>
    /// ��� �������� ������ �����͸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="data">������ ������ ������</param>
    public void SetData(ItemData data)
    {
        Data = data;
        currentItemCode = (uint)Data.itemCode;
    }

    /// <summary>
    /// ȹ���� �������� �κ��丮�� �������� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="ownerInventory">�������� ���� �κ��丮</param>
    public void AdditemToInventory(Inventory ownerInventory)
    {
        if(currentItemCode == (uint)ItemCode.Coin)  // �������� �����̸�
        {
            ownerInventory.AddGold(Data.price);     // ��� ����
        }
        else
        {
            ownerInventory.AddSlotItem(currentItemCode);   // ������ �߰�
        }
        gameObject.SetActive(false);            // ������ ��Ȱ��ȭ
    }
}
