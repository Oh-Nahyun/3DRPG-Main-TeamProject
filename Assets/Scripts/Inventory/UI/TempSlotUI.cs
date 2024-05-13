using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TempSlotUI : SlotUI_Base
{
    /// <summary>
    /// �ӽ� ���� UIâ�� ���ȴ��� Ȯ���ϴ� ������Ƽ ( true : �������� , false : �������� )
    /// </summary>
    public bool IsOpen => transform.localScale == Vector3.one;
    void Start()
    {
        CloseTempSlot();    
    }

    void Update()
    {
        if(IsOpen)
        {
            transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// �ӽ� ������ ���� �Լ�
    /// </summary>
    public void OpenTempSlot()
    {
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// �ӽ� ������ �ݴ� �Լ�
    /// </summary>
    public void CloseTempSlot()
    {
        transform.localScale = Vector3.zero;
    }
}
