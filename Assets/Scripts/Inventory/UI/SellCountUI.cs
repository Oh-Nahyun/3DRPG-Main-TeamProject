using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellCountUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    Image itemIcon;
    TMP_InputField inputField;
    Slider slider;
    Button decreaseBtn;
    Button increaseBtn;
    Button okBtn;
    Button cancelBtn;

    InventorySlot targetSlot = null; // �Ǹ��� ������ ����

    /// <summary>
    /// ���� ������ 
    /// </summary>
    int sellCount = 1;

    /// <summary>
    /// ���� �������� ���� �� ������ �ϱ� ���� ������Ƽ
    /// </summary>
    int SellCount
    {
        get => sellCount;
        set
        {
            sellCount = value;
            sellCount = Mathf.Clamp(value, 1, (int)slider.maxValue);
        }
    }

    /// <summary>
    /// �Ǹ��� �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<InventorySlot, int> onSell;
    
    /// <summary>
    /// SellCountUIâ�� �ݾ��� �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action onCloseSellCount;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        inputField = child.GetComponent<TMP_InputField>();
        child = transform.GetChild(2);
        slider = child.GetComponent<Slider>();
        slider.onValueChanged.AddListener((float count) =>
        {
            // �����̴� value ������Ʈ
            SellCount = (int)count;
            UpdateValue(SellCount);
        });

        child = transform.GetChild(3);
        decreaseBtn = child.GetComponent<Button>();
        decreaseBtn.onClick.AddListener(() =>
        {
            // ���� ��ư value ������Ʈ ( ���� )
            SellCount--;
            UpdateValue(SellCount);
        });

        child = transform.GetChild(4);
        increaseBtn = child.GetComponent<Button>();
        increaseBtn.onClick.AddListener(() =>
        {
            // ������ ��ư value ������Ʈ ( �߰��ϱ� )
            SellCount++;
            UpdateValue(SellCount);
        });

        child = transform.GetChild(5);
        okBtn = child.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            // Ȯ�� ��ư ( ������ ������ )
            onSell?.Invoke(targetSlot, SellCount);
            SellCountUIClose();
        });

        child = transform.GetChild(6);
        cancelBtn = child.GetComponent<Button>();
        cancelBtn.onClick.AddListener(() =>
        {
            // ��� ��ư ( �г� �ݱ� )
            SellCountUIClose();
        });
    }

    void Start()
    {
        SellCountUIClose();
    }

    public void InitializeValue(InventorySlot slot, int minCount, int maxCount)
    {
        itemIcon.sprite = slot.SlotItemData.itemIcon;

        slider.minValue = minCount;
        slider.maxValue = maxCount;
        slider.value = SellCount;

        //DividCount = Mathf.Clamp(DividCount, minCount, maxCount);
        targetSlot = slot;
    }

    public void UpdateValue(int count)
    {
        inputField.text = count.ToString();
        slider.value = count;
    }

    /// <summary>
    /// SellCountUI ���̰� �ϴ� �Լ�
    /// </summary>
    public void SellCountUIOpen()
    {
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// SellCountUI ����� �Լ� ( alpha = 0 )
    /// </summary>
    public void SellCountUIClose()
    {
        canvasGroup.alpha = 0;
        onCloseSellCount?.Invoke();
    }
}