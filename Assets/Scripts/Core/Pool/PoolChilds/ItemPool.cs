using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<ItemDataObject>
{
    /// <summary>
    /// ItemPool���� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="slot">������ �������� ����</param>
    /// <param name="position">������ ��ġ</param>
    /// <returns></returns>
    public GameObject GetItemObject(ItemData itemData, uint count = 1, Vector3? position = null)
    {
        GameObject itemObj = itemData.ItemPrefab;                      // ������ ������ ����

        ItemDataObject parentObj = GetObject(position);                         // Ǯ���� ������ ������
        parentObj.GetComponent<ItemDataObject>().SetData(itemData);    // ���� �������� ������ ������ ����
        
        for(int i = 0; i < count; i++)
        {
            Instantiate(itemObj, parentObj.transform);                              // ������ ������ ������ ����
        }

        return parentObj.gameObject;                                            // Factory�� ������Ʈ ��ȯ ( �������� �ڽ� 0��° )
    }
}
