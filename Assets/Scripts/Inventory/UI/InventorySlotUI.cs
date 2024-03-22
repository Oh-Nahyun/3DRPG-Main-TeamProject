using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : SlotUI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    InventoryUI invenUI;

    void Awake()
    {
        invenUI = ItemDataManager.Instance.InventoryUI;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // temp�� ������ �ű�� (slot -> temp)
        invenUI.onSlotDragBegin?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"�巡�� �� : {eventData}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if(obj != null)
        {
            invenUI.onSlotDragEnd?.Invoke(obj.GetComponent<SlotUI_Base>().InventorySlotData.SlotIndex);
        }
        else
        {
            Debug.Log(obj);
        }
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
    }
}
