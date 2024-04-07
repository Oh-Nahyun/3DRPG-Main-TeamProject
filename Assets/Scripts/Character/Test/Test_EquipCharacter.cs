using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ��丮�� �׽�Ʈ ĳ���� ��ũ��Ʈ
/// </summary>
public class Test_EquipCharacter : MonoBehaviour, IEquipTarget, IHealth
{
    public float hp;
    public float HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHP);
            onHealthChange?.Invoke(hp);
        }
    }

    public float maxHP = 5;
    public float MaxHP => maxHP;

    /// <summary>
    /// ü���� ����� �� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// ĳ���Ͱ� ����ִ��� Ȯ���ϴ� ������Ƽ ( 0 �ʰ� : true,)
    /// </summary>
    public bool IsAlive => HP > 0;

    /// <summary>
    /// ĳ���Ͱ� ����ϸ� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// ������ ������ ��ġ ( equipPart ������� �ʱ�ȭ �ؾ���)
    /// </summary>
    [Tooltip("Equip Part�� �����ϰ� ��ġ�� ��")]
    public Transform[] partPosition;

    /// <summary>
    /// ������ ������ �����۵�
    /// </summary>
    private InventorySlot[] equipPart;

    public InventorySlot[] EquipPart
    {
        get => equipPart;
        set
        {
            equipPart = value;
        }
    }

    int partCount = Enum.GetNames(typeof(EquipPart)).Length;

    void Awake()
    {
        EquipPart = new InventorySlot[partCount];

        HP = MaxHP;
    }

    /// <summary>
    /// ĳ���� ������ ������ �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="equipment">��� ������</param>
    /// <param name="part">������ ����</param>
    public void CharacterEquipItem(GameObject equipment, EquipPart part, InventorySlot slot)
    {
        if (EquipPart[(int)part] != null) // ������ �������� ������
        {
            CharacterUnequipItem(part); // �����ߴ� ������ �ı�
            Instantiate(equipment, partPosition[(int)part]); // ������ ������Ʈ ����
            EquipPart[(int)part] = slot;    // ���������� ������ ���� ����
        }
        else // ������ �������� ������
        {
            EquipPart[(int)part] = slot;
            Instantiate(equipment, partPosition[(int)part]); // ������ ������Ʈ ����
        }
    }

    /// <summary>
    /// ĳ���� ������ �������� �� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="part"></param>
    public void CharacterUnequipItem(EquipPart part)
    {
        Destroy(partPosition[(int)part].GetChild(0).gameObject);    // ������ ������Ʈ �ı�
    }

    /// <summary>
    /// ����� ����Ǵ� �Լ�
    /// </summary>
    public void Die()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// ü��ȸ�� �� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="totalRegen">�� ȸ����</param>
    /// <param name="duration">ȸ�� �ֱ� �ð�</param>
    public void HealthRegenerate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegen_Coroutine(totalRegen, duration));
    }

    /// <summary>
    /// ü�� ȸ�� �ڷ�ƾ
    /// </summary>
    /// <param name="totalRegen">���� ȸ���� ü�·�</param>
    /// <param name="Duration">ü��ȸ���ϴ� �ð�</param>
    /// <returns></returns>
    IEnumerator HealthRegen_Coroutine(float totalRegen, float Duration)
    {
        float timeElapsed = 0f;
        while(timeElapsed < Duration)
        {
            timeElapsed += Time.deltaTime;
            HP += (totalRegen / Duration) * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// ƽ�� ü�� ȸ���� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="tickRegen">ƽ�� ȸ����</param>
    /// <param name="tickInterval">ȸ�� �ֱ�</param>
    /// <param name="totalTickCount">���� ƽ ��</param>
    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        StartCoroutine(HealthRegenByTick_Coroutine(tickRegen, tickInterval, totalTickCount));
    }

    /// <summary>
    /// ƽ�� ü�� ȸ�� �ڷ�ƾ
    /// </summary>
    /// <param name="tickRegen">ƽ�� ȸ����</param>
    /// <param name="tickInterval">ȸ�� �ֱ�</param>
    /// <param name="totalTickCount">���� ƽ ��</param>
    /// <returns></returns>
    IEnumerator HealthRegenByTick_Coroutine(float tickRegen, float tickInterval, uint totalTickCount)
    {
        for (int i = 0; i < totalTickCount; i++)
        {
            float timeElapsed = 0f;
            while (timeElapsed < tickInterval)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            HP += tickRegen;
        }
    }
}