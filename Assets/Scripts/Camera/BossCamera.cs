using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    /// <summary>
    /// �÷��̾� vcam
    /// </summary>
    public CinemachineVirtualCamera virtualCamera;
    public float delay = 1.5f; // ���� �ð�, 2�ʷ� ����

    /// <summary>
    /// �ٶ� ����� �����ϴ� Ʈ������
    /// </summary>
    Transform target;

    /// <summary>
    /// �������� �������� �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action OnEntryBossStage;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// ī�޶� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="transform">�ٶ� ��� Ʈ������</param>
    public void StartBossCameraCoroutine(Transform transform)
    {
        if(!SetLookAt(transform))
        {
            Debug.Log($"ī�޶� �ٶ� ����� �������� �ʽ��ϴ�.");
        }

        OnEntryBossStage?.Invoke();
        StartCoroutine(LowerPriorityAfterDelay());
    }

    /// <summary>
    /// �ٶ� ����� �����ϴ� �Լ�
    /// </summary>
    /// <param name="transform"></param>
    bool SetLookAt(Transform targetTransform)
    {
        bool result = true;

        target = targetTransform;
        virtualCamera.LookAt = targetTransform;

        if (target == null) result = false;

        return result;
    }

    IEnumerator LowerPriorityAfterDelay()
    {
        // ������ ���� �ð� ���� ���
        virtualCamera.Priority = 100;
        yield return new WaitForSeconds(delay);

        // Priority�� 0���� ����
        virtualCamera.Priority = 0;
    }
}
