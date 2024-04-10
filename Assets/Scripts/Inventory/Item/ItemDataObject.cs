using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataObject : RecycleObject
{
    // Ȱ��ȭ �� �� �κ��丮�� ������ �ޱ�
    public ItemData Data;

    void Start()
    {
        onDisable += OnItemDisable;
    }

    private void OnItemDisable()
    {
        Debug.Log($"������ ������");

        int childObjCount = transform.childCount;

        Debug.Log($"{childObjCount}");

        for(int i = 0; i < childObjCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
