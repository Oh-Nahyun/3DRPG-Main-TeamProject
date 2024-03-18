using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestInputBase
{
    Inventory inven;

    [Header("���� ����")]

    [Tooltip("������ �ڵ� �Է�")]
    public int code;
    [Tooltip("���� �ε���")]
    [Range(0,5)]
    public uint index;

    void Start()
    {
        inven = new Inventory();
    }

    protected override void OnKey1Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.AddSlotItem(code,1,index);
        }
    }
}
