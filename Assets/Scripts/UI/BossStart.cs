using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    private CanvasGroup bossTextCanvasGroup;
    private CanvasGroup miniMapPanelCanvasGroup;
    private CanvasGroup bossHpTextCanvasGroup;
    private CanvasGroup sliderCanvasGroup;

    void Start()
    {
        // Canvas�� �ڽ� ������Ʈ���� CanvasGroup ������Ʈ�� ã���ϴ�.
        bossTextCanvasGroup = GameObject.Find("BossText").GetComponent<CanvasGroup>();

        // Canvas�� �ڽ� ������Ʈ�� MiniMapPanel���� CanvasGroup ������Ʈ�� ã���ϴ�.
        miniMapPanelCanvasGroup = GameObject.Find("MiniMapPanel").GetComponent<CanvasGroup>();

        bossHpTextCanvasGroup = GameObject.Find("BossHpText").GetComponent<CanvasGroup>();

        sliderCanvasGroup = GameObject.Find("Slider").GetComponent<CanvasGroup>();

        // �ڷ�ƾ�� �����Ͽ� �ؽ�Ʈ�� ���̵� �ƿ��մϴ�.
        StartCoroutine(FadeOutText());
        StartCoroutine(FadeInMiniMapPanel());
        StartCoroutine(FadeInBossHpText());
        StartCoroutine(FadeInSlider());
    }

    IEnumerator FadeOutText()
    {
        // 1.5�� ���� ���
        yield return new WaitForSeconds(2.1f);

        // Alpha ���� 0���� �����Ͽ� �ؽ�Ʈ�� ���̵� �ƿ�
        bossTextCanvasGroup.alpha = 0;
    }
    IEnumerator FadeInMiniMapPanel()
    {
        // 2.5�� ���� ���
        yield return new WaitForSeconds(2.5f);

        // MiniMapPanel�� Alpha ���� 1�� �����Ͽ� ���̵� ��
        miniMapPanelCanvasGroup.alpha = 1;
    }

    IEnumerator FadeInBossHpText()
    {
        yield return new WaitForSeconds(2.5f);

        bossHpTextCanvasGroup.alpha = 1;
    }

    IEnumerator FadeInSlider()
    {
        yield return new WaitForSeconds(2.5f);

        sliderCanvasGroup.alpha = 1;
    }
}