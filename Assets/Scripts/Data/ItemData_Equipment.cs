using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void UnEquipItem(InventorySlot slot)
    {
        throw new System.NotImplementedException();
    }
}
