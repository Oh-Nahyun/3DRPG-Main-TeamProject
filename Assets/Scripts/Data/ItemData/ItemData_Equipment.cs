using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equipment : ItemData, IEquipable
{
    [Header("���� ������ ����")]
    /// <summary>
    /// ������ ������ ������
    /// </summary>
    public GameObject EqiupPrefab;

    public EquipPart equipPart;

    /// <summary>
    /// ������ ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="owner">�κ��丮�� ���� ������Ʈ </param>
    /// <param name="slot">������ ���� �ε���</param>
    public void EquipItem(GameObject owner, InventorySlot slot)
    {
        IEquipTarget equipTarget = owner.GetComponent<IEquipTarget>();

        if(equipTarget != null)
        {
            equipTarget.CharacterEquipItem(EqiupPrefab, equipPart, slot);
            slot.IsEquip = true;
        }
    }

    /// <summary>
    /// ������ ���� ������ �� �����ϴ� �Լ�
    /// </summary>
    public void UnEquipItem(GameObject owner, InventorySlot slot)
    {
        IEquipTarget equipTarget = owner.GetComponent<IEquipTarget>();

        if (equipTarget != null)
        {
            equipTarget.CharacterUnequipItem(equipPart);
            equipTarget.EquipPart[(int)equipPart] = null;
            slot.IsEquip = false;
        }
    }
}
