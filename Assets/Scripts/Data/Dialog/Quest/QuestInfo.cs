using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static QuestData;

public class QuestInfo : MonoBehaviour
{
    // QuestInfoPanel ������Ʈ ����
    public QuestInfoPanel questInfoPanel;

    CanvasGroup canvasGroup;

    bool onInfo = false;

    /// <summary>
    /// ����Ʈâ �� ������� �ӵ� 
    /// </summary>
    public float alphaChangeSpeed = 5.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ����Ʈ â�� �Ѱ� ���� �Լ�
    /// </summary>
    public void OnQuestInfo()
    {
        if (!onInfo)
        {
            gameObject.SetActive(true);
        }
        onInfo = !onInfo;
        StartCoroutine(setAlphaChange(onInfo));
    }

    IEnumerator setAlphaChange(bool onInfo)
    {
        if (!onInfo)
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            gameObject.SetActive(false);
        }
        else
        {
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
        }
    }
}
