using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ������ �ڵ�
/// </summary>
public enum ItemCode
{
    portion = 0,
    apple,
    cheese,
    Hammer,
    Sword,
    HP_portion,
    HP_portion_Tick,
    Coin,
    Bow,
    Arrow
}

/// <summary>
/// ���� ���
/// </summary>
public enum SortMode
{
    Name = 0,
    Price,
    Count
}

/// <summary>
/// IEquipTarget�� ������ ��� ���� ����
/// </summary>
public enum EquipPart
{
    Hand_R,
    Hand_L
}

/// <summary>
/// ���� �������� ������ Ÿ�� enum
/// </summary>
public enum WeaponType
{
    Melee = 0,
    Range
}