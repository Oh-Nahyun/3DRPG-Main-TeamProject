using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ؽ�Ʈ UI�� ���̵� �� �ƿ� �ϴ� Ŭ����
/// </summary>
public class FadeInOutTextUI : MonoBehaviour
{
    /// <summary>
    /// ���̵� �� �ƿ� �� �ؽ�Ʈ 
    /// </summary>
    public TextMeshProUGUI text;

    public float fadeIntime = 1f;
    public float fadeOutTime = 1f;

    float alpha = 0f;

    float Alpha
    {
        get => alpha;
        set
        {
            alpha = Mathf.Clamp(value, 0f, 1f);
        }
    }

    void Start()
    {
        text.color = new Color(1f, 1f, 1f, 0f);
    }

    /// <summary>
    /// ���̵� �� �� �� �����ϴ� �Լ�
    /// </summary>
    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeIntime)
        {
            timeElapsed += Time.deltaTime;
            Alpha += timeElapsed;
            text.color = new Color(1f, 1f, 1f, Alpha);

            yield return null;
        }
    }

    /// <summary>
    /// ���̵� �ƿ��� �� �����ϴ� �Լ�
    /// </summary>
    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeOutTime)
        {
            timeElapsed += Time.deltaTime;
            Alpha -= timeElapsed;
            text.color = new Color(1f, 1f, 1f, Alpha);

            yield return null;
        }
    }
}
