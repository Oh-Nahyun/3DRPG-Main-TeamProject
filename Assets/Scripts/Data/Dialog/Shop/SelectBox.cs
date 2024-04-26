using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectBox : MonoBehaviour
{

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

    CanvasGroup canvasGroup;

    /// <summary>
    /// â �� ������� �ӵ� 
    /// </summary>
    public float alphaChangeSpeed = 5.0f;

    bool selectCheck = false;
    public bool SelectCheck => selectCheck;

    Button buttonCheck;
    Button buttonCancel;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = GetComponent<Transform>();
        child = transform.GetChild(1);
        selectText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        buttonCheck = child.GetComponent<Button>();
        buttonCheck.onClick.AddListener(() => Select(true));

        child = child.GetChild(0);
        selectText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        buttonCancel = child.GetComponent<Button>();
        buttonCancel.onClick.AddListener(() => Select(false));

        child = child.GetChild(0);
        selectText = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Select(bool isCheck)
    {
        selectCheck = isCheck;
        selectCheck = false;
        StartCoroutine(SetAlphaChange());
    }

    IEnumerator SetAlphaChange()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        selectCheck = false;
        gameObject.SetActive(false);
    }


}
