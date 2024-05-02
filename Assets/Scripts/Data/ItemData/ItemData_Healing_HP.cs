using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ü�� ȸ�� ������ ������
/// </summary>
[CreateAssetMenu(fileName = "ItemData-Healing-HP", menuName = "ScriptableObjects/ItemData-Healing-HP", order = 2)]
public class ItemData_Healing_HP : ItemData, IConsumable
{
    /// <summary>
    /// ȸ���� ü���� �� ( ƽ ȸ�� x)
    /// </summary>
    public float healing_Hp;

    /// <summary>
    /// ȸ���ϴµ� �ɸ��� �ð�
    /// </summary>
    public float duration;

    /// <summary>
    /// �ش� �������� �Һ�� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="owner">������ ����ϴ� ������Ʈ</param>
    /// <param name="slot">���Ǵ� ������ ����</param>
    public void Consum(GameObject owner, InventorySlot slot)
    {
        IHealth health = owner.GetComponent<IHealth>();
       
        if(health != null)
        {
            // ������ ����
            slot.DiscardItem(1);    // ������ 1�� ����
            // IHealth�� ü�� ȸ�� 
            health.HealthRegenerate(healing_Hp, duration);
        }
        else
        {
            Debug.Log($"[{owner.name}] ������Ʈ���� IHealth �������̽��� �������� �ʽ��ϴ�.");
        }
    }    
}
