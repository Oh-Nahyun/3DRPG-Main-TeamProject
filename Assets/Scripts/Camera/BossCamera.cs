using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float delay = 1.5f; // ���� �ð�, 2�ʷ� ����

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        // �ڷ�ƾ ����
        StartCoroutine(LowerPriorityAfterDelay());
    }

    IEnumerator LowerPriorityAfterDelay()
    {
        // ������ ���� �ð� ���� ���
        yield return new WaitForSeconds(delay);

        // Priority�� 0���� ����
        virtualCamera.Priority = 0;
    }
}
