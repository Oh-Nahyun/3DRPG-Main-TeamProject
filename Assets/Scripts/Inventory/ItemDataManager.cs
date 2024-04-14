using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// ������ �����͵�
    /// </summary>
    public ItemData[] datas;

    /// <summary>
    /// ������ ������ ������ ���� �ε���
    /// </summary>
    /// <param name="index">datas �ε���</param>
    /// <returns></returns>
    public ItemData this[int index] => datas[index];

    /// <summary>
    /// ������ ������ �ڵ�� ���� �ϱ� ���� �ε���
    /// </summary>
    /// <param name="code">������ �ڵ� ��</param>
    /// <returns></returns>
    public ItemData this[ItemCode code] => datas[(int)code];

    /// <summary>
    /// �κ��丮 UI Ŭ����
    /// </summary>
    InventoryUI inventoryUI;

    /// <summary>
    /// �κ��丮 UI Ŭ���� ������ ���� ������Ƽ
    /// </summary>
    public InventoryUI InventoryUI => inventoryUI;

    public GameObject CharaterRenderCamera;

    #region GameManager
    /// <summary>
    /// ItemDataManager Singleton
    /// </summary>
    public static ItemDataManager Instance;

    #endregion

    void Awake()
    {
        Instance = this;    // singleton

        inventoryUI = FindAnyObjectByType<InventoryUI>(); // find inventoryUI
    }
}
