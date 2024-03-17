using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item Data class
/// </summary>

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// �ش� ������ �ڵ�
    /// </summary>
    public ItemCode itemCode;

    /// <summary>
    /// ������ �̸�
    /// </summary>
    public string itemName;

    /// <summary>
    /// ������ ������
    /// </summary>
    public Sprite itemIcon;

    /// <summary>
    /// ������ ����
    /// </summary>
    public string desc;

    /// <summary>
    /// ������ ����
    /// </summary>
    public uint price;

    /// <summary>
    /// ������ �ִ� ������
    /// </summary>
    public uint maxCount;
}
