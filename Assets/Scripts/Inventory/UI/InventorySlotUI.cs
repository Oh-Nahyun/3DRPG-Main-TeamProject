using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlotUI : SlotUI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler,
                                            IPointerClickHandler, 
                                            IPointerEnterHandler, IPointerExitHandler
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
            // ������ �ƴϴ�
            invenUI.onSlotDragEndFail?.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        // Ŭ���ϸ� ������� Ȯ��
        // ���� ����
        // �ٸ� ������ ����
        // ������ Ŭ���ϸ� �޴�?

        if (obj != null)
        {
            bool isPressedQ = Keyboard.current.qKey.ReadValue() > 0;

            if(isPressedQ) // dividUI ����
            {
                invenUI.onDivdItem(InventorySlotData.SlotIndex);
                Debug.Log($"������");
            }
        }
        else
        {
            Debug.Log(obj);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        invenUI.onShowDetail?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        invenUI.onCloseDetail?.Invoke();
    }
}
