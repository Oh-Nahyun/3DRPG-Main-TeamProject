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

    Collider collider;

    void Start()
    {
        bossCamera = FindAnyObjectByType<BossCamera>();
        stageSetting = GetComponentInParent<BossStageSetting>();
        bossNameUI = FindAnyObjectByType<FadeInOutTextUI>();
        bossHPSlider = FindAnyObjectByType<BossHPSlider>();

        collider = GetComponent<Collider>();
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
        bossNameUI.StartFadeInOut();
        bossCamera.StartBossCameraCoroutine(bossTransform);
        bossTransform.gameObject.SetActive(true);

        bossHPSlider.ShowPanel();

        collider.isTrigger = false; // Ʈ���� ��Ȱ��ȭ
        transform.localPosition += Vector3.left * 2f;
    }
}
