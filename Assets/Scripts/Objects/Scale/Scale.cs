using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    private Transform scaleObjR; // ������ ���� ��(Transform)
    private Transform scaleObjL; // ���� ���� ��(Transform)

    public float weightPerUnit = 1f; // ���� ���� (1 ����Ƽ ������ �� kg����)

    private float transitionDuration = 1f; // �̵��� �ɸ��� �ð�

    private float transitionTimer = 0f; // ���� �̵��� �ð�

    private float initialHeightR; // ������ ���� ���� �ʱ� Y��
    private float initialHeightL; // ���� ���� ���� �ʱ� Y��

    private Vector3 targetHeightR; // ������ ���� ���� ��ǥ ����
    private Vector3 targetHeightL; // ���� ���� ���� ��ǥ ����

    private float totalWeightR; // ������ ���￡ �÷��� �� ����
    private float totalWeightL; // ���� ���￡ �÷��� �� ����

    ScaleObj scaleColliderR;
    ScaleObj scaleColliderL;

    private float heightR;
    private float heightL;

    private void Awake()
    {

        Transform child = transform.GetChild(0);
        scaleObjR = child.GetComponent<Transform>();
        scaleColliderR = child.GetComponent<ScaleObj>();
        child = transform.GetChild(1);
        scaleObjL = child.GetComponent<Transform>();
        scaleColliderL = child.GetComponent<ScaleObj>();

    }

    private void Start()
    {
        initialHeightR = scaleObjR.localPosition.y;
        initialHeightL = scaleObjL.localPosition.y;

        heightR = initialHeightR;
        heightL = initialHeightL;
    }

    private void Update()
    {
        transitionTimer += Time.deltaTime;
        
        totalWeightR = scaleColliderR.totalWeight - scaleColliderL.totalWeight;
        totalWeightL = scaleColliderL.totalWeight - scaleColliderR.totalWeight;
        
        float t = Mathf.Clamp01(transitionTimer / transitionDuration); // �ð��� ����� ���� ���� �� ���

        // ������ ��ġ ����
        Vector3 newHeightR = Vector3.Lerp(scaleObjR.localPosition, targetHeightR, t);
        Vector3 newHeightL = Vector3.Lerp(scaleObjL.localPosition, targetHeightL, t);

        InitialHeight();
        // �� ���� ���� ��ġ ������Ʈ
        scaleObjR.localPosition = newHeightR;
        scaleObjL.localPosition = newHeightL;
    }

    /// <summary>
    /// ������ ���Ը� ���ϴ� �Լ�
    /// </summary>
    private void InitialHeight()
    {

        if (totalWeightR == totalWeightL)
        {
            heightR = initialHeightR;
            heightL = initialHeightL;
        }
        else if (totalWeightR < totalWeightL)
        {
            heightR = initialHeightR + (totalWeightR / weightPerUnit);
            heightL = initialHeightL - (totalWeightR / weightPerUnit);
        }
        else if (totalWeightL < totalWeightR)
        {
            heightR = initialHeightR - (totalWeightL / weightPerUnit);
            heightL = initialHeightL + (totalWeightL / weightPerUnit);
        }

        // �� ���� ���� ��ǥ ���� ���

        // ��ǥ ���� ����
        targetHeightR = scaleObjR.localPosition;
        targetHeightR.y = heightR;

        targetHeightL = scaleObjL.localPosition;
        targetHeightL.y = heightL;

        targetHeightR.y = Mathf.Clamp(heightR, -8f, 0f);
        targetHeightL.y = Mathf.Clamp(heightL, -8f, 0f);
        // �̵��� �ð� �ʱ�ȭ
        transitionTimer = 0f;
        
    }
}

