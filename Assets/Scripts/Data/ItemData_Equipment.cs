using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equipment : ItemData, IEquipable
{
    /// <summary>
    /// ������ ������ ������
    /// </summary>
    public GameObject EqiupPrefab;

    public void EquipItem()
    {
        throw new System.NotImplementedException();
    }

    public void UnEquipItem()
    {
        throw new System.NotImplementedException();
    }
}
