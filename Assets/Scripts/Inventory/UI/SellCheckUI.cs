using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class SellCheckUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    /// <summary>
    /// Ȯ�� ���� �ؽ�Ʈ 
    /// </summary>
    TextMeshProUGUI checkText;

    /// <summary>
    /// Ȯ�� ��ư 
    /// </summary>
    Button okButton;

    /// <summary>
    /// ��� ��ư 
    /// </summary>
    Button cancelButton;

    /// <summary>
    /// show CheckPanel delegate
    /// </summary>
    public Action<InventorySlot, int> onCheckSell;

    /// <summary>
    /// ConformSell item delegate
    /// </summary>
    public Action onConformSell;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Transform child = transform.GetChild(0);
        checkText = child.GetChild(0).GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        okButton = child.GetChild(0).GetComponent<Button>();
        okButton.onClick.AddListener(() =>
        {
            onConformSell?.Invoke();
            ClosePanel();
        });

        cancelButton = child.GetChild(1).GetComponent<Button>();
        cancelButton.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        onCheckSell += SetText;
    }

    /// <summary>
    /// �ؽ�Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="slot">������ ������ �ִ� slot</param>
    public void SetText(InventorySlot slot, int count)
    {
        ItemData itemData = slot.SlotItemData;
        string name = itemData.itemName;
        uint price = itemData.price;

        checkText.text = $"[{name}]�� [{count}]��ŭ �첲 \n" +
                         $"[{price * count}]�� ���� �� ��������";
    }

    public void ShowCheckPanel()
    {
        canvasGroup.alpha = 1.0f;
    }

    void ClosePanel()
    {
        canvasGroup.alpha = 0.0f;
    }
}