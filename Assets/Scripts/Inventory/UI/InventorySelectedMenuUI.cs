using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelectedMenuUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Button dividButton;
    Button dropButton;

    /// <summary>
    /// �Ŵ� �����ٶ� alpha��
    /// </summary>
    readonly float ShowPanelValue = 1f;

    /// <summary>
    /// �Ŵ� ���涧 alpha��
    /// </summary>
    readonly float HidePanelValue = 0f;

    /// <summary>
    /// ������ ��ư ������ �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action OnDividButtonClick;

    /// <summary>
    /// ��� ��ư ������ �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action OnDropButtonClick;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0); // background Object
        dividButton = child.GetChild(0).GetComponent<Button>();
        dividButton.onClick.AddListener(() => 
        {
            OnDividButtonClick?.Invoke();
        });

        dropButton = child.GetChild(1).GetComponent<Button>();
        dropButton.onClick.AddListener(() =>
        {
            OnDropButtonClick?.Invoke();
        });

        HideMenu();
    }

    public void SetPosition(Vector2 postiion)
    {
        transform.position = postiion;
    }

    /// <summary>
    /// SelecetdMenuUI �����ֱ�
    /// </summary>
    public void ShowMenu()
    {
        canvasGroup.alpha = ShowPanelValue;
    }

    /// <summary>
    /// SelectedMenuUI �����
    /// </summary>
    public void HideMenu()
    {
        canvasGroup.alpha = HidePanelValue;
    }
}