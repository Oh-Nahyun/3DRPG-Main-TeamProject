using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryPlayerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// �巡�� ������ġ
    /// </summary>
    float startDragPosition = 0;

    /// <summary>
    /// ���� �巡�� ���� ������ ��ġ
    /// </summary>
    float currentDragPosition = 0;

    /// <summary>
    /// �巡�� ���� ��ġ���� �巡���� ���� (���� ��ġ - ���� ��ġ)
    /// </summary>
    float dragValue = 0;

    /// <summary>
    /// dragValue �����ϱ����� ������Ƽ
    /// </summary>
    float DragValue
    {
        get => dragValue;
        set
        {
            if(dragValue != value)
            {
                dragValue = Mathf.Clamp(value, -5f, 5f);
            }
        }
    }

    /// <summary>
    /// ���콺�� �����̴� �� üũ�ϴ� ����
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// RenderCharater �ؽ��ĸ� �����ִ� ī�޶� ������Ʈ
    /// </summary>
    GameObject rendererCamera;

    void Start()
    {
        rendererCamera = ItemDataManager.Instance.CharaterRenderCamera;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������ �ʱ�ȭ ( ����ġ ���� ȸ�� ���� )
        DragValue = 0f;
        currentDragPosition = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMove = eventData.IsPointerMoving();
        if(isMove)
        {
            OnCharacterRenderPanelDrag(eventData.position.x);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �ʱ�ȭ
        StartCoroutine(SpinActive());
    }

    /// <summary>
    /// �巡�װ� ������ �����Ͱ� ���� �������� ��� �����ϴ� �Լ�
    /// </summary>
    IEnumerator SpinActive()
    {
        while(DragValue != 0.0f)
        {
            if(DragValue < 0) // Drag�� ����( ���� ���� ȸ�� )
            {
                DragValue += Time.deltaTime;
                rendererCamera.transform.rotation = Quaternion.Euler(0, rendererCamera.transform.eulerAngles.y + DragValue * 0.4f, 0);
            }
            else if(DragValue > 0) // Drag�� ��� ( ������ ���� ȸ�� )
            {
                DragValue -= Time.deltaTime;
                rendererCamera.transform.rotation = Quaternion.Euler(0, rendererCamera.transform.eulerAngles.y + DragValue * 0.4f, 0);
            }
            yield return null;
        }
    }

    void OnCharacterRenderPanelDrag(float pointerValue)
    {
        currentDragPosition = pointerValue;                     // �巡���� ������ ��ġ ����
        DragValue = startDragPosition - currentDragPosition;    // �巡�� ������ �������κ��� ũ�� ( �巡�� ���� ��ġ - ���� ������ ��ġ)
        startDragPosition = currentDragPosition;

        rendererCamera.transform.eulerAngles = new Vector3(0, rendererCamera.transform.eulerAngles.y + DragValue, 0); // ī�޶� ȸ���� ����
    }
}