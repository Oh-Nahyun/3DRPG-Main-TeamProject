using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ӽ� ���� Ŭ����
/// </summary>
public class TempSlot : InventorySlot
{
    /// <summary>
    /// ���� �ȵ������� �����Ǵ� �ε��� ��ȣ
    /// </summary>
    const uint notSet = uint.MaxValue;

    /// <summary>
    /// ������ �ε��� ���� ����
    /// </summary>
    uint fromIndex = notSet;

    /// <summary>
    /// �ӽ� ���� ������
    /// </summary>
    /// <param name="index">�ε��� ��</param>
    public TempSlot(uint index) : base(index)
    {
        fromIndex = index;
    }

    /// <summary>
    /// �ӽ� ���� ������ �߰�
    /// </summary>
    /// <param name="code">������ �ڵ�</param>
    /// <param name="count">������ ����</param>
    /// <param name="over">��� ����</param>
    public override void AssignItem(uint code, int count, out int _)
    {
        base.AssignItem(code, count, out _);
    }

    /// <summary>
    /// �ӽ� ���� Ŭ����
    /// </summary>
    public override void ClearItem()
    {
        base.ClearItem();
        fromIndex = notSet;
    }
}
