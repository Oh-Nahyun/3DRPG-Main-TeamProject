using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class TextSelect : MonoBehaviour
{
    private TextBox textBox;
    TextMeshProUGUI buttonText1;
    TextMeshProUGUI buttonText2;
    TextMeshProUGUI buttonText3;

    private void Awake()
    {
        textBox = FindObjectOfType<TextBox>(); // TextBox Ŭ������ �ν��Ͻ��� ã��

        Transform child = transform.GetChild(0);
        Button select1 = child.GetComponent<Button>();
        select1.onClick.AddListener(() => Select(1)); //������ 1�� ������ id + 1
        buttonText1 = child.GetComponentInChildren<TextMeshProUGUI>();

        child = transform.GetChild(1);
        Button select2 = child.GetComponent<Button>();
        select2.onClick.AddListener(() => Select(2)); //������ 2�� ������ id + 1
        buttonText2 = child.GetComponentInChildren<TextMeshProUGUI>();

        child = transform.GetChild(2);
        Button select3 = child.GetComponent<Button>();
        select3.onClick.AddListener(() => Select(3)); //������ 3�� ������ id + 1
        buttonText3 = child.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        onSeletEnd();
    }

    public void setButtonText(string text1, string text2, string text3)
    {
        buttonText1.text = text1;
        buttonText2.text = text2;
        buttonText3.text = text3;
    }

    /// <summary>
    /// ���� ���۽� ����� �Լ�
    /// </summary>
    public void onSeletStart()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ���� ����� ����� �Լ�
    /// </summary>
    public void onSeletEnd()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �������� ���� Id���� ������ �Լ�
    /// </summary>
    /// <param name="selectId">������ ��</param>
    void Select(int selectId)
    {
        textBox.OnSelect(selectId);
    }


}
