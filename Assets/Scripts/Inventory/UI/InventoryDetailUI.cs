using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class InventoryDetailUI : MonoBehaviour
{
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDesc;
    TextMeshProUGUI itemPrice;

    RectTransform rectTransform;

    /// <summary>
    /// �ӽ� ���� UIâ�� ���ȴ��� Ȯ���ϴ� ������Ƽ ( true : �������� , false : �������� )
    /// </summary>
    bool IsOpen => transform.localScale == Vector3.one;

    void Start()
    {
        Transform child = transform.GetChild(0);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        itemDesc = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        itemPrice = child.GetChild(1).GetComponent<TextMeshProUGUI>();

        itemName.text = $"������ ��";
        itemDesc.text = $"������ ����";
        itemPrice.text = $"999999999";
    }

    void Update()
    {
        if(IsOpen)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            MovePosition(mousePos);
        }
    }

    public void MovePosition(Vector2 mousePosition)
    {
        RectTransform rect = (RectTransform)transform;

        rect.position = mousePosition;
        // ���� ������ + width > maxwidth ������ = maxwidth
        int over = (int)(rect.position.x + rect.sizeDelta.x);

        over = Mathf.Max(0, over);

        if(over > Screen.width)
        {
            rect.position = new Vector3(Screen.width - rect.sizeDelta.x, rect.position.y);
        }

    }

    /// <summary>
    /// ������ ���� �ؽ�Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="name">������ �̸�</param>
    /// <param name="desc">������ ����</param>
    /// <param name="price">������ ����</param>
    public void SetDetailText(string name, string desc, uint price)
    {
        itemName.text = $"{name}";
        itemDesc.text = $"{desc}";
        itemPrice.text = price.ToString("N0");
    }

    /// <summary>
    /// ������ ���� �ʱ�ȭ �Լ�
    /// </summary>
    public void ClearText()
    {
        itemName.text = $"������ ��";
        itemDesc.text = $"������ ����";
        itemPrice.text = $"999999999";
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