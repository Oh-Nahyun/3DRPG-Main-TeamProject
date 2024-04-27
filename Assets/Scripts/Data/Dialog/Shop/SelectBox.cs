using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectBox : MonoBehaviour
{
    CanvasGroup canvasGroup;

    /// <summary>
    /// �߾� �ؽ�Ʈ
    /// </summary>
    public TextMeshProUGUI selectText;

    /// <summary>
    /// Ȯ�ι�ư �ؽ�Ʈ
    /// </summary>
    public TextMeshProUGUI buttonCheckText;

    /// <summary>
    /// ��ҹ�ư �ؽ�Ʈ
    /// </summary>
    public TextMeshProUGUI buttonCancelText;

    /// <summary>
    /// â �� ������� �ӵ� 
    /// </summary>
    public float alphaChangeSpeed = 5.0f;

    Button buttonCheck;
    Button buttonCancel;

    public Action onButtonCheck;
    public Action onButtonCancel;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = GetComponent<Transform>();
        child = transform.GetChild(1);
        selectText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        buttonCheck = child.GetComponent<Button>();
        buttonCheck.onClick.AddListener(() => onButtonCheck?.Invoke());

        child = child.GetChild(0);
        buttonCheckText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        buttonCancel = child.GetComponent<Button>();
        buttonCancel.onClick.AddListener(() => SetButtonCancel());

        child = child.GetChild(0);
        buttonCancelText = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        canvasGroup.alpha = 1;
        gameObject.SetActive(false);
    }

    private void SetButtonCancel()
    {
        onButtonCancel?.Invoke();
        gameObject.SetActive(false);
    }
}
