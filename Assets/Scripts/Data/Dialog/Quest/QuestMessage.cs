using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestMessage : MonoBehaviour
{
    /// <summary>
    /// ����Ʈ �̸�
    /// </summary>
    TextMeshProUGUI questName;

    TextMeshProUGUI QuestName => questName;

    CanvasGroup questMessage;

    CanvasGroup completeMessage;

    public float alphaChangeSpeed = 5.0f;
    /// <summary>
    /// ����Ʈ �޽����� �����ִ� �ð�
    /// </summary>
    float delayTime = 5.0f;


    private void Awake()
    {
        questName = GetComponentInChildren<TextMeshProUGUI>();
        questMessage = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(2);
        completeMessage = child.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        questMessage.alpha = 0;
    }



    /// <summary>
    /// ����Ʈ�� ����, Ŭ����� ǥ��
    /// </summary>
    /// <param name="text">����Ʈ �̸�</param>
    /// <param name="Complete">true�� ����Ʈ Ŭ����</param>
    public void onQuestMessage(string text, bool Complete)
    {
        StartCoroutine(SetOnAlphaChange());
        questName.text = text;
        if (Complete)
        {
            completeMessage.alpha = 1;
        }
        else
        {
            completeMessage.alpha = 0;
        }
        StartCoroutine(DelayTime(delayTime));
        StartCoroutine(SetOffAlphaChange());
    }

    IEnumerator SetOnAlphaChange()
    {
        while (questMessage.alpha > 0.0f)
        {
            questMessage.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    IEnumerator SetOffAlphaChange()
    {
        while (questMessage.alpha > 0.0f)
        {
            questMessage.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    IEnumerator DelayTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

}
