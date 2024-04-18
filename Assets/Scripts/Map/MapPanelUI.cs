using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapPanelUI : MonoBehaviour
{
    /// <summary>
    /// �Է��� ���� inputAction
    /// </summary>
    PlayerinputActions inputActions; 

    /// <summary>
    /// Map�� ���� ī�޶�
    /// </summary>
    public Camera mapCamera;

    LargeMapUI mapUI;

    public GameObject pointPrefab;


    private void Awake()
    {
        inputActions = new PlayerinputActions();
        mapUI = GetComponentInChildren<LargeMapUI>();

        mapUI.onClick += OnClickInput;
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += OnClickInput;
    }

    private void OnClickInput(InputAction.CallbackContext context)
    {
        
    }

    private void OnDisable()
    {
        inputActions.UI.Click.performed -= OnClickInput;
        inputActions.UI.Disable();        
    }

    private void OnClickInput(Vector2 vector)
    {
        Ray ray = mapCamera.ScreenPointToRay(vector);   // ray
        RaycastHit hit;                                 // rayHit ����

        if(Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Map Object"))) // Map Object Ž��
        {
            Debug.Log($"Hit");
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5f);
        }
        Debug.Log($"Hit Next");

        Instantiate(pointPrefab, hit.point, Quaternion.identity);  // PointObject
    }
}
