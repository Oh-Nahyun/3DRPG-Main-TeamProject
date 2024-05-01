using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData-Healing-HP-Tick", menuName = "ScriptableObjects/ItemData-Healing-HP-Tick", order = 3)]
public class ItemData_Healing_Hp_Tick : ItemData, IConsumable
{
    /// <summary>
    /// ƽ�� ȸ���� ��
    /// </summary>
    public float tickRegen;

    /// <summary>
    /// ƽ ���͹�
    /// </summary>
    public float inverval;

    /// <summary>
    /// ȸ�� ƽ ����
    /// </summary>
    public uint tickCount;

    /// <summary>
    /// �ش� �������� �Һ�� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="owner">������ ����ϴ� ������Ʈ</param>
    /// <param name="slot">���Ǵ� ������ ����</param>
    public void Consum(GameObject owner, InventorySlot slot)
    {
        IHealth health = owner.GetComponent<IHealth>();

        if (health != null)
        {
            // ������ ����
            slot.DiscardItem(1);    // ������ 1�� ����
            // IHealth�� ü�� ȸ�� 
            health.HealthRegenerateByTick(tickRegen, inverval, tickCount);
        }
        else
        {
            Debug.Log($"[{owner.name}] ������Ʈ���� IHealth �������̽��� �������� �ʽ��ϴ�.");
        }
    }
}
