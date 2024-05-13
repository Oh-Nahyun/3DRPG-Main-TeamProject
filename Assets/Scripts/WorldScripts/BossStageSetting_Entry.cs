using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� �Ա� ��ũ��Ʈ
/// </summary>
public class BossStageSetting_Entry : MonoBehaviour
{
    /// <summary>
    /// �������� ����
    /// </summary>
    BossStageSetting stageSetting;

    /// <summary>
    /// ���� ���� ī�޶�
    /// </summary>
    BossCamera bossCamera;

    /// <summary>
    /// ���� �̸�
    /// </summary>
    FadeInOutTextUI bossNameUI;

    /// <summary>
    /// ���� ü�¹�
    /// </summary>
    BossHPSlider bossHPSlider;

    void Start()
    {
        bossCamera = FindAnyObjectByType<BossCamera>();
        stageSetting = GetComponentInParent<BossStageSetting>();
        bossNameUI = FindAnyObjectByType<FadeInOutTextUI>();
        bossHPSlider = FindAnyObjectByType<BossHPSlider>();

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnStageEnter();
        }
    }

    /// <summary>
    /// �������� ������ �� �����ϴ� �Լ�
    /// </summary>
    void OnStageEnter()
    {
        Transform bossTransform = stageSetting.GetBoss().gameObject.transform;
        // Boss spawn
        bossNameUI.StartFadeIn();
        bossCamera.StartBossCameraCoroutine(bossTransform);
        bossTransform.gameObject.SetActive(true);
        bossNameUI.StartFadeOut();

        bossHPSlider.ShowPanel();
    }
}
