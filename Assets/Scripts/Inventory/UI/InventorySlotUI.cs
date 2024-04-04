using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : SlotUI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler,
                                            IPointerClickHandler, 
                                            IPointerEnterHandler, IPointerExitHandler
{
    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = ItemDataManager.Instance.InventoryUI;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // temp�� ������ �ű�� (slot -> temp)
        inventoryUI.onSlotDragBegin?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"�巡�� �� : {eventData}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;

        inventoryUI.onSlotDragEnd?.Invoke(obj);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;

        // ����ó��
        if(InventorySlotData.SlotItemData == null)
        {
            Debug.Log($"���Կ� �������� �����ϴ�.");
            return;
        }

        // OnPointerClick �̺�Ʈ ó��
        if (obj != null)
        {
            inventoryUI.onDivdItem(InventorySlotData.SlotIndex);
        }
        else
        {
            Debug.Log($"������Ʈ�� �����ϴ�.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryUI.onShowDetail?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.onCloseDetail?.Invoke();
    }
}
