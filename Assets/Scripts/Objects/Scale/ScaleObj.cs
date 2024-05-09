using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObj : MonoBehaviour
{
    public float totalWeight; // ���� ���￡ �÷��� �� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            // ���￡ ��ü�� ������ ��
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Weighable weighable = other.GetComponent<Weighable>();
            if (rb != null && weighable != null)
            {
                // ��ü�� ���Ը� �����ͼ� ������ ���Կ� �߰�
                totalWeight -= weighable.weigh;
            }
        }
        else
        {
            totalWeight = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            // ���￡ ��ü�� ������ ��
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Weighable weighable = other.GetComponent<Weighable>();
            if (rb != null)
            {
                // ��ü�� ���Ը� �����ͼ� ������ ���Կ� �߰�
                totalWeight += weighable.weigh;
            }
        }
        else
        {
            totalWeight = 0f;
        }
    }
}
