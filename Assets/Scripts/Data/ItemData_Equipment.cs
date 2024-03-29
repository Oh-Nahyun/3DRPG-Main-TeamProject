using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class ItemData_Equipment : ItemData, IEquipable
{
    /// <summary>
    /// ������ ������ ������
    /// </summary>
    public GameObject EqiupPrefab;

    /// <summary>
    /// ������ ������ �� �����ϴ� �Լ�
    /// </summary>
    public void EquipItem(GameObject owner, InventorySlot slot)
    {
        IEquipTarget equipTarget = owner.GetComponent<IEquipTarget>();

        if(equipTarget != null)
        {            
            equipTarget.CharacterEquipItem(EqiupPrefab);
        }
    }

    /// <summary>
    /// ������ ���� ������ �� �����ϴ� �Լ�
    /// </summary>
    public void UnEquipItem(GameObject owner)
    {
        IEquipTarget equipTarget = owner.GetComponent<IEquipTarget>();

        if (equipTarget != null)
        {
            equipTarget.CharacterUnequipItem();
        }
    }
}
