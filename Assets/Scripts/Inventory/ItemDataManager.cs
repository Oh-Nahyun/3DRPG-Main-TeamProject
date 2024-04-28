using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��� �� ������ ���� Ŭ���� ( UI ���� )
/// </summary>
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

    /// <summary>
    /// Inventory RenderTexture Object Point
    /// </summary>
    public GameObject CharaterRenderCameraPoint;

    public void InitializeItemDataUI()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>(); // find inventoryUI
    }
}
