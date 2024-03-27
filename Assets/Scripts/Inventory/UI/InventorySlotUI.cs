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

    void Start()
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
        InventorySlot slot = obj.GetComponent<SlotUI_Base>().InventorySlotData;

        if (slot != null)
        {
            invenUI.onSlotDragEnd?.Invoke(slot.SlotIndex);
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

        // ����ó��
        if(InventorySlotData.SlotItemData == null)
        {
            Debug.Log($"���Կ� �������� �����ϴ�.");
            return;
        }

        // OnPointerClick �̺�Ʈ ó��
        if (obj != null)
        {
            bool isEquipment = obj.GetComponent<SlotUI_Base>().InventorySlotData.SlotItemData is IEquipable; // �߸��� ����
            if(isEquipment)
            {
                Debug.Log($"[{InventorySlotData.SlotItemData.name}]�� ��� �Դϴ�. < Ŭ�� O >");
            }

            // Key Q
            bool isPressedQ = Keyboard.current.qKey.ReadValue() > 0;

            if(isPressedQ) // dividUI ����
            {

                if (InventorySlotData.CurrentItemCount <= 1)
                {
                    Debug.Log($"[{InventorySlotData.SlotItemData.itemName}]�� �������� [{InventorySlotData.CurrentItemCount}]�� �ֽ��ϴ�.");
                    return;
                }
                else
                {
                    invenUI.onDivdItem(InventorySlotData.SlotIndex);
                }
            }
        }
        else
        {
            Debug.Log($"������Ʈ�� �����ϴ�.");
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
