using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum DividPanelType
{
    Divid = 0, // Default
    Drop,
}

public class InventoryDividUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    Image itemIcon;
    TMP_InputField inputField;
    Slider slider;
    Button decreaseBtn;
    Button increaseBtn;
    Button okBtn;
    Button cancelBtn;

    InventorySlot targetSlot = null; // �����⸦ ������ ���� 

    /// <summary>
    /// ���� �гλ��� ( �г� ���¿� ���� Ȯ�ι�ư�� �����ϴ� �Լ��� �޶����� , DividPanelType ���� )
    /// </summary>
    public DividPanelType dividPanelType;

    /// <summary>
    /// �ּҰ�
    /// </summary>
    const int minValue = 1;

    /// <summary>
    /// ���� ������ 
    /// </summary>
    int dividCount = 1;

    /// <summary>
    /// ���� �������� ���� �� ������ �ϱ� ���� ������Ƽ
    /// </summary>
    int DividCount
    {
        get => dividCount;
        set
        {
            dividCount = value;
            dividCount = Mathf.Clamp(value, 1, (int)slider.maxValue);
        }
    }

    /// <summary>
    /// ���� �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<InventorySlot, int> onDivid;

    /// <summary>
    /// ������ ����� �� �����ϴ� ��������Ʈ ( ������ ����ϱ� ���� ��������Ʈ )
    /// </summary>
    public Action<InventorySlot, int> onDrop;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        inputField = child.GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener((text) =>
        {
            // ��ǲ �ʵ忡 ���� ������ ����
            if (int.TryParse(text, out int value))
            {
                DividCount = value;
            }
            else
            {
                // inputField�� ������ �ް� �����Ǿ� �־ -���� �ִ� ���� �ƴϸ� ���� �ȵ�                
                DividCount = minValue;
            }

            UpdateValue(DividCount);
        });

        child = transform.GetChild(2);
        slider = child.GetComponent<Slider>();
        slider.onValueChanged.AddListener((float count) =>
        {
            // �����̴� value ������Ʈ
            DividCount = (int)count;
            UpdateValue(DividCount);
        });

        child = transform.GetChild(3);
        decreaseBtn = child.GetComponent<Button>();
        decreaseBtn.onClick.AddListener(() =>
        {
            // ���� ��ư value ������Ʈ ( ���� )
            DividCount--;
            UpdateValue(DividCount);
        });

        child = transform.GetChild(4);
        increaseBtn = child.GetComponent<Button>();
        increaseBtn.onClick.AddListener(() =>
        {
            // ������ ��ư value ������Ʈ ( �߰��ϱ� )
            DividCount++;
            UpdateValue(DividCount);
        });

        child = transform.GetChild(5);
        okBtn = child.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            // Ȯ�� ��ư ( ������ ������ )
            //onDivid?.Invoke(targetSlot, DividCount);
            CheckPanelType(dividPanelType);
            DividUIClose();
        });

        child = transform.GetChild(6);
        cancelBtn = child.GetComponent<Button>();
        cancelBtn.onClick.AddListener(() =>
        {
            // ��� ��ư ( �г� �ݱ� )
            DividUIClose();
        });
    }

    void Start()
    {
        DividUIClose();
    }

    /// <summary>
    /// �г��� ���� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="slot">���� ������ ����</param>
    /// <param name="minCount">������ �ּ� ����</param>
    /// <param name="maxCount">������ �ִ� ����</param>
    public void InitializeValue(InventorySlot slot, int minCount, int maxCount)
    {       
        itemIcon.sprite = slot.SlotItemData.itemIcon;

        slider.minValue = minCount;
        slider.maxValue = maxCount;
        slider.value = DividCount;

        //DividCount = Mathf.Clamp(DividCount, minCount, maxCount);
        targetSlot = slot;
    }

    /// <summary>
    /// ���� ������Ʈ �ϴ� �Լ�
    /// </summary>
    /// <param name="count"></param>
    public void UpdateValue(int count)
    {
        inputField.text = count.ToString();
        slider.value = count;
    }

    /// <summary>
    /// Divid UI ���̰� �ϴ� �Լ�
    /// </summary>
    public void DividUIOpen(DividPanelType type)
    {
        dividPanelType = type;
        canvasGroup.alpha = 1;
    }
    
    /// <summary>
    /// Divid UI ����� �Լ� ( alpha = 0 )
    /// </summary>
    public void DividUIClose()
    {
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// ���� ���� �г��� ������ �ϱ� ���� �г����� Ÿ���� Ȯ���ϴ� �Լ� ( Ȯ�� ��ư���� ������ ��������Ʈ�� ����ȴ�. )
    /// </summary>
    /// <param name="type">divid �г� Ÿ�� </param>
    void CheckPanelType(DividPanelType type)
    {
        switch(type)
        {
            case DividPanelType.Divid:
                onDivid?.Invoke(targetSlot, dividCount);
                break;
            case DividPanelType.Drop:
                onDrop?.Invoke(targetSlot, dividCount);
                break;
        }
    }
}