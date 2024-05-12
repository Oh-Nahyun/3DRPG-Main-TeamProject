using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    Button StartBtn;
    Button LoadBtn;
    Button ExitBtn;
    SaveHandler_Menu LoadMenu;

    PlayerinputActions playerInputAcitons;

    private void Awake()
    {
        playerInputAcitons = new PlayerinputActions();        
    }

    private void Start()
    {

        Transform child = transform.GetChild(0);
        StartBtn = child.GetComponent<Button>();
        StartBtn.onClick.AddListener(OnStart);

        child = transform.GetChild(1);
        LoadBtn = child.GetComponent<Button>();
        LoadBtn.onClick.AddListener(OnLoad);

        child = transform.GetChild(2);
        ExitBtn = child.GetComponent<Button>();
        ExitBtn.onClick.AddListener(OnExit);

        LoadMenu = FindAnyObjectByType<SaveHandler_Menu>();
    }

    private void OnEnable()
    {
        playerInputAcitons.UI.Enable();
        playerInputAcitons.UI.Close.performed += OnClose;
    }

    private void OnDisable()
    {
        playerInputAcitons.UI.Close.performed -= OnClose;
        playerInputAcitons.UI.Disable();        
    }

    // ���� ��ŸƮ ��ư
    void OnStart()
    {
        // ���� ó������ ������ �̵�
        string sceneName = $"Main_Map_Test";
        GameManager.Instance.ChangeToTargetScene(sceneName, GameManager.Instance.Player.gameObject);
    }

    /// <summary>
    /// ���� �ε� ��ư
    /// </summary>
    void OnLoad()
    {
        LoadMenu.ShowSavePanel();
    }

    /// <summary>
    /// ���� ���� ��ư
    /// </summary>
    void OnExit()
    {
        Application.Quit();
    }

    private void OnClose(InputAction.CallbackContext context)
    {
        LoadMenu.CloseSavePanel();
    }
}