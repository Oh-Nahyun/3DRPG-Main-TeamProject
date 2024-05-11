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
    /// �Ǹ�â UI Ŭ����
    /// </summary>
    SellPanelUI sellPanelUI;

    /// <summary>
    /// sellPanelUI ������ ���� ������Ƽ
    /// </summary>
    public SellPanelUI SellPanelUI => sellPanelUI;  

    /// <summary>
    /// Inventory RenderTexture Object Point
    /// </summary>
    public GameObject CharaterRenderCameraPoint;

    /// <summary>
    /// ItemDataManager Ŭ���� �ʱ�ȭ �Լ� ( Player �ʱ�ȭ���Ŀ� �� �� )
    /// </summary>
    public void InitializeItemDataUI()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>(); // find inventoryUI
        sellPanelUI = FindAnyObjectByType<SellPanelUI>();

        if(GameManager.Instance.Player != null)
        {
            Player player = GameManager.Instance.Player;    
            CharaterRenderCameraPoint = player.gameObject.transform.GetChild(player.transform.childCount - 1).gameObject;
        }
    }
}
