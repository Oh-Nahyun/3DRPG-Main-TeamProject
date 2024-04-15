using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    /// <summary>
    /// Map Object ���̾ �ִ� ������Ʈ�� Material
    /// </summary>
    Renderer mapPointMaterial;

    /// <summary>
    /// MapPointMaterial�� ������ �����ϱ� ���� y��ǥ��
    /// </summary>
    float position_Y => transform.position.y;

    /// <summary>
    /// ������ ���� �迭
    /// </summary>
    public Color[] color;

    /// <summary>
    /// ���� ����
    /// </summary>
    private int colorCount = 3;

    /// <summary>
    /// ������ ���� ���� ( �� ���̺��� ������ �ٸ� ����)
    /// </summary>
    public float colorGap = 5f;

    void Start()
    {
        Transform child = transform.GetChild(0);
        mapPointMaterial = child.GetChild(0).GetComponent<Renderer>();

        color = new Color[colorCount];
        for(int i = 0; i < color.Length; i++)
        {
            color[i] = new Color(i*0.3f, i*0.3f, i*0.3f);
        }

        SetColor();
    }

    void SetColor()
    {
        for(int i = 0; i < 3; i++)
        {
            if(i * colorGap < position_Y && position_Y <= (i + 1) * colorGap)
            {                
                mapPointMaterial.material.color = color[i];
            }
        }
    }
}
