using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestInputBase
{
    Inventory inventory;

    [Header("���� ����")]

    [Tooltip("������ �ڵ� �Է�")]
    public int code;
    [Tooltip("���� �ε���")]
    public int index;

    void Start()
    {
        inventory = new Inventory();
    }

    protected override void OnKey1Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inventory.AddItem(0, 1);
            inventory.ShowInventory();
        }
    }
}
