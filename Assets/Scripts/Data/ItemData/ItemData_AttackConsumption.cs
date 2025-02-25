using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격할 때 소비하는 아이템 
/// </summary>
[CreateAssetMenu(fileName = "ItemData-AttackConsum", menuName = "ScriptableObjects/ItemData-Consum", order = 4)]
public class ItemData_AttackConsumption : ItemData
{
    /// <summary>
    /// 사용할 때 생성할 아이템 오브젝트
    /// </summary>
    [Tooltip("소비할 때 월드에 생성할 아이템 오브젝트를 넣는다.")]
    public GameObject WorldItemPrefab;

    /// <summary>
    /// 아이템 1개를 소비하는 함수
    /// </summary>
    /// <param name="inventorySlot">현재 아이템이 있는 아이템 슬롯</param>
    public void ConsumItem(InventorySlot inventorySlot)
    {
        inventorySlot.DiscardItem(1);
    }
}