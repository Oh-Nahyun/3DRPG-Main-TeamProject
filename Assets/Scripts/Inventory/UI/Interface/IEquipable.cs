using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ �������� ��ӹ޴� �������̽�
/// </summary>
public interface IEquipable
{
    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    void EquipItem() { }

    /// <summary>
    /// ������ �������� �ϴ� �Լ�
    /// </summary>
    void UnEquipItem() { }
}
