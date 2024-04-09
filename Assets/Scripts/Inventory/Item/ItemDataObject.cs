using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataObject : RecycleObject
{
    // Ȱ��ȭ �� �� �κ��丮�� ������ �ޱ�
    public ItemData Data;

    GameObject itemObj;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (Data != null)
        {            
            itemObj = Data.ItemPrefab;
            Instantiate(itemObj, this.transform);        
        }
    }
}
