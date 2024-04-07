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

    CanvasGroup canvasGroup;

    /// <summary>
    /// �ӽ� ���� UIâ�� ���ȴ��� Ȯ���ϴ� ������Ƽ ( true : �������� , false : �������� )
    /// </summary>
    bool IsOpen => transform.localScale == Vector3.one;

    float fadeInSpeed = 3f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        itemDesc = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        itemPrice = child.GetChild(1).GetComponent<TextMeshProUGUI>();

        itemName.text = $"������ ��";
        itemDesc.text = $"������ ����";
        itemPrice.text = $"999999999";

        CloseItemDetail();
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
        int overWidth = (int)(rect.position.x + rect.sizeDelta.x);

        overWidth = Mathf.Max(0, overWidth);

        if(overWidth > Screen.width)
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
    public void ShowItemDetail()
    {
        //canvasGroup.alpha = 1;
        StartCoroutine(FadeInDetail());
    }

    /// <summary>
    /// �ӽ� ������ �ݴ� �Լ�
    /// </summary>
    public void CloseItemDetail()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0;
    }

    IEnumerator FadeInDetail()
    {
        float timeElpased = 0;

        while (timeElpased < 1f)
        {
            timeElpased += Time.deltaTime;
            canvasGroup.alpha = timeElpased * fadeInSpeed;
            yield return null;
        }
    }
}