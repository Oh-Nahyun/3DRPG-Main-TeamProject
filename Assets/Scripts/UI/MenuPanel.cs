using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MenuState
{
    Nomal = 0,
    Inventory,
    Map,
    Save
}

public class MenuPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    
    TextMeshProUGUI currnetPanelName;
    TextMeshProUGUI prePanelName;
    TextMeshProUGUI nextPanelName;
    
    PlayerinputActions inputActions;

    SaveHandler saveHandler;

    /// <summary>
    /// ���� �г� ����
    /// </summary>
    MenuState state = MenuState.Nomal;

    /// <summary>
    /// �г� ���¸� �����ϴ� ������Ƽ 
    /// </summary>
    MenuState State
    {
        get => state;
        set
        {
            state = value;            
            switch(state)
            {
                case MenuState.Nomal:
                    prePanelName.text = $"���̺�";
                    currnetPanelName.text = $"�븻";
                    nextPanelName.text = $"�κ��丮";
                    ShowNormal();
                    break;
                case MenuState.Inventory:
                    prePanelName.text = $"�븻";
                    currnetPanelName.text = $"�κ��丮";
                    nextPanelName.text = $"��";
                    ShowInventory();
                    break;
                case MenuState.Map:
                    prePanelName.text = $"�κ��丮";
                    currnetPanelName.text = $"��";
                    nextPanelName.text = $"���̺�";
                    ShowMap();
                    break;
                case MenuState.Save:
                    prePanelName.text = $"��";
                    currnetPanelName.text = $"���̺�";
                    nextPanelName.text = $"�븻";
                    ShowSave();
                    break;
                default:
                    break;
            }
        }
    }

    private void Awake()
    {
        Transform topPanel = transform.GetChild(0);
        Transform child = topPanel.transform.GetChild(0);
        currnetPanelName = child.GetComponent<TextMeshProUGUI>();
        child = topPanel.transform.GetChild(2);
        prePanelName = child.GetComponent<TextMeshProUGUI>();
        child = topPanel.transform.GetChild(4);
        nextPanelName = child.GetComponent<TextMeshProUGUI>();

        inputActions = new PlayerinputActions();
    }

    void OnEnable()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        saveHandler = FindAnyObjectByType<SaveHandler>();
    }

    void OnDisable() // ��Ȱ��ȭ �Ǹ� ��ǲ ����
    {
        inputActions.UI.PrePanel.performed -= OnLeftArrow;
        inputActions.UI.NextPanel.performed -= OnRightArrow;
        inputActions.UI.Close.performed -= OnClose;
        inputActions.UI.Disable();
    }

    void OnDestroy()
    {
        canvasGroup = null;
    }

    private void OnRightArrow(InputAction.CallbackContext context)
    {
        if ((int)State == System.Enum.GetValues(typeof(MenuState)).Length - 1)
        {
            State = MenuState.Nomal;
        }
        else
        {
            State++;
        }
    }

    private void OnLeftArrow(InputAction.CallbackContext context)
    {
        if((int)State == 0)
        {
            State = MenuState.Save;
        }
        else
        {
            State--;
        }
    }

    private void OnClose(InputAction.CallbackContext context)
    {
        CloseMenu();
    }

    public void ShowNormal()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.CloseMapUI();
        saveHandler.CloseSavePanel(); // ���̺� �г� �ݱ�

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// ��� UI�� �ݰ� �κ��丮�� ���� �Լ�
    /// </summary>
    public void ShowInventory()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.ShowInventory();
        GameManager.Instance.ItemDataManager.CharaterRenderCameraPoint.transform.eulerAngles = new Vector3(0, 180f, 0); // RenderTexture �÷��̾� ��ġ �ʱ�ȭ
        GameManager.Instance.MapManager.CloseMapUI();
        saveHandler.CloseSavePanel(); // ���̺� �г� �ݱ�

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// ��� UI�� �ݰ� ���� ���� �Լ�
    /// </summary>
    public void ShowMap()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.OpenMapUI();
        saveHandler.CloseSavePanel(); // ���̺� �г� �ݱ�

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// ��� UI�� �ݰ� ���̺�â ���� �Լ�
    /// </summary>
    public void ShowSave()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.CloseMapUI();
        saveHandler.ShowSavePanel(); // ���̺� �г� ����

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// �Ŵ��г��� Ȱ��ȭ �ϴ� �Լ�
    /// </summary>
    public void ShowMenu(MenuState setState)
    {
        State = setState;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        inputActions.UI.Enable();
        inputActions.UI.PrePanel.performed += OnLeftArrow;
        inputActions.UI.NextPanel.performed += OnRightArrow;
        inputActions.UI.Close.performed += OnClose;

        GameManager.Instance.MapManager.CloseMiniMapUI();

        GameManager.Instance.MapManager.IsOpenedLargeMap = true;
    }


    /// <summary>
    /// �Ŵ��г��� ��Ȱ��ȭ �ϴ� �Լ�
    /// </summary>
    public void CloseMenu()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        inputActions.UI.PrePanel.performed -= OnLeftArrow;
        inputActions.UI.NextPanel.performed -= OnRightArrow;
        inputActions.UI.Close.performed -= OnClose;
        inputActions.UI.Disable();

        GameManager.Instance.MapManager.OpenMiniMapUI();

        GameManager.Instance.MapManager.IsOpenedLargeMap = false;
        GameManager.Instance.Player.UIPanelClose();
    }
}
