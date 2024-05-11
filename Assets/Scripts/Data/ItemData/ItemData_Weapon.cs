using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ������
/// </summary>
[CreateAssetMenu(fileName = "ItemData-Weapon", menuName = "ScriptableObjects/ItemData-Weapon", order = 1)]
public class ItemData_Weapon : ItemData_Equipment
{
    [Header("���� ����")]
    public float Damage;

    public WeaponType WeaponType;
}