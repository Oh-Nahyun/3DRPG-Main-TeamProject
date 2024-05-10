using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���̺곪 �ε��� �� Ȱ��ȭ�Ǵ� â�� ������Ʈ
/// </summary>
public class SaveCheckUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI uiText;
    TextMeshProUGUI okBtnText;
    TextMeshProUGUI cancelBtnText;
    Button okBtn;
    Button cancelBtn;

    /// <summary>
    /// ���̺��� �� �����ϴ� �������̵� ( OK ��ư ������ ���� )
    /// </summary>
    public Action<int> onSave;

    /// <summary>
    /// �ε��� �� �����ϴ� ��������Ʈ ( OK ��ư ������ ���� )
    /// </summary>
    public Action<int> onLoad;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        uiText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        okBtn = child.GetComponent<Button>();
        okBtnText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
        okBtnText.text = $"OK";

        child = transform.GetChild(2);  
        cancelBtn = child.GetComponent<Button>();
        cancelBtn.onClick.AddListener(ClosePanel);
        cancelBtnText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
        cancelBtnText.text = $"Cancel";
    }

    /// <summary>
    /// ���̺��� �� Ȯ�� �ϴ� â�� ���� �Լ�
    /// </summary>
    /// <param name="slotIndex">���� �ε���</param>
    public void ShowSaveCheck(int slotIndex)
    {
        OpenPanel();
        uiText.text = $"{slotIndex}���� ���̺긦 �Ͻðڽ��ϱ�?";

        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(() =>
        {
            onSave?.Invoke(slotIndex);
            ClosePanel();
        });
    }

    /// <summary>
    /// �ε��� �� Ȯ���ϴ� â�� ���� �Լ�
    /// </summary>
    /// <param name="slotIndex">���� �ε���</param>
    public void ShowLoadCheck(int slotIndex)
    {
        OpenPanel();
        uiText.text = $"{slotIndex}�� �����͸� �ε� �Ͻðڽ��ϱ�?";

        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(() =>
        {
            onLoad?.Invoke(slotIndex);
            ClosePanel();
        });
    }

    /// <summary>
    /// �г��� �� �� �����ϴ� �Լ�
    /// </summary>
    private void OpenPanel()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// �г��� ���� �� �����ϴ� �Լ�
    /// </summary>
    private void ClosePanel()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}