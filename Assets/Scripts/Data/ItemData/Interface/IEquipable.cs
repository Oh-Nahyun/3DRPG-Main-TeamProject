using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ �������� ��ӹ޴� �������̽�
/// </summary>
interface IEquipable
{
    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    public void EquipItem(GameObject owner, InventorySlot slot);

    /// <summary>
    /// ������ �������� �ϴ� �Լ�
    /// </summary>
    public void UnEquipItem(GameObject owner);
}
