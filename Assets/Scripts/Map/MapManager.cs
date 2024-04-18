using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Map UI�� ���� ���� �ٷ�� Manager Ŭ����
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary>
    /// �ӽ� Map Singleton
    /// </summary>
    public static MapManager Instance;

    /// <summary>
    /// ���� ��� �� ( Color Gap���� �ٸ� ������ ǥ�� )
    /// </summary>
    public Color[] color;

    /// <summary>
    /// ��� �� ����
    /// </summary>
    public uint ColorCount;

    /// <summary>
    /// ���� Object y�� ����
    /// </summary>
    public float colorGap = 5f;

    private void Awake()
    {
        Instance = this;

        InitializeMapColor();
    }

    private void Start()
    {

    }

    private void InitializeMapColor()
    {
        color = new Color[ColorCount];

        for (int i = 0; i < color.Length; i++)
        {
            float ratio = 1 / (float)ColorCount;    // ���򰳼� ���� �� ���� ����
            color[i] = Color.white * ratio * (i + 1);         // ���� ���ϱ�
            color[i].a = 1f;                        // alpha���� 1�� �ٽ� ����
        }
    }

    /// <summary>
    /// ������Ʈ�� ������ �����ִ� �Լ�
    /// </summary>
    /// <param name="yPosition">������Ʈ�� y��ǥ �� ( World )</param>
    /// <returns>yPosition / ColorCount�� ��� �� �迭 ��</returns>
    public Color SetColor(float yPosition)
    {
        Color resultColor = Color.white;

        int colorIndex = Mathf.FloorToInt(yPosition / (float)ColorCount); // color �ε��� �� ����

        resultColor = color[colorIndex];

        return resultColor;
    }
}
