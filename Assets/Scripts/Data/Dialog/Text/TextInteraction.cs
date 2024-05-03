using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInteraction : MonoBehaviour
{
    CanvasGroup canvasGroup;

    TextMeshProUGUI tagText;
    public TextMeshProUGUI TagText => tagText;
    TextBox textBox;
    TextBoxItem textBoxItem;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        tagText = GetComponentInChildren<TextMeshProUGUI>();
        textBox = FindAnyObjectByType<TextBox>();
        textBoxItem = FindAnyObjectByType<TextBoxItem>();
    }

    private void Start()
    {
        canvasGroup.alpha = 1; 
    }

    /// <summary>
    /// ���� ����� ������Ʈ�� ��ȣ�ۿ� �ؽ�Ʈ�� ����ϴ� �Լ�
    /// </summary>
    /// <param name="obj">���� ����� ������Ʈ</param>
    public void SetTagText(GameObject obj)
    {
        switch (obj.tag)
        {
            case "NPC":
                TagText.SetText("���ϱ�");
                break;
            case "Item":
                TagText.SetText("�ݱ�");
                break;
            case "Chest":
                TagText.SetText("����");
                break;
            case "Warp":
                TagText.SetText("�̵��ϱ�");
                break;
            case "DoorOpen":
                TagText.SetText("�ݱ�");
                break;
            case "DoorClose":
                TagText.SetText("����");
                break;
            case "Lever":
                TagText.SetText("����");
                break;
            default:
                TagText.SetText("Ȯ���ϱ�");
                break;
        }
    }

    /// <summary>
    /// TextInteraction������Ʈ�� OnOff������ �ϱ� ����
    /// </summary>
    /// <param name="OnOff">ture = On</param>
    public void TextActive(bool OnOff)
    {
        if (textBox.TalkingEnd || textBoxItem.Talking)
        {
            gameObject.SetActive(false);
        }else if (OnOff)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
