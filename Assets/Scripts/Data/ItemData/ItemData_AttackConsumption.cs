using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� �Һ��ϴ� ������ 
/// </summary>
[CreateAssetMenu(fileName = "ItemData-AttackConsum", menuName = "ScriptableObjects/ItemData-Consum", order = 4)]
public class ItemData_AttackConsumption : ItemData
{
    /// <summary>
    /// ����� �� ������ ������ ������Ʈ
    /// </summary>
    [Tooltip("�Һ��� �� ���忡 ������ ������ ������Ʈ�� �ִ´�.")]
    public GameObject WorldItemPrefab;

    /// <summary>
    /// ������ 1���� �Һ��ϴ� �Լ�
    /// </summary>
    /// <param name="inventorySlot">���� �������� �ִ� ������ ����</param>
    public void ConsumItem(InventorySlot inventorySlot)
    {
        inventorySlot.DiscardItem(1);
    }
}