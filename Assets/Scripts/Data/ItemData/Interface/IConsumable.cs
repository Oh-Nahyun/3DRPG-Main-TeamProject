using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Һ� ������ ������ �����Ͱ� ��� �޴� �������̽�
/// </summary>
public interface IConsumable
{
    /// <summary>
    /// ������ �Һ��� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="slot">����� ������ �κ��丮 ����</param>
    public void Consum(GameObject owner, InventorySlot slot);
}
