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

    [Header("Currnet Map Size")]
    public float mapSizeX = 300f;
    public float mapSizeY = 300f;
    public float panelSize = 10f;
    public GameObject panelPrefab;

    [Header("Map Object Info")]
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

    /// <summary>
    /// �� �г� UI
    /// </summary>
    MapPanelUI mapPanelUI;

    /// <summary>
    /// �� �г� UI�� �����ϱ� ���� ������Ƽ
    /// </summary>
    public MapPanelUI MapPanelUI => mapPanelUI;

    /// <summary>
    /// Map�� �÷��̾� �̵������� ǥ���� Linerenderer
    /// </summary>
    LineRenderer playerLineRenderer;

    /// <summary>
    /// playerLineRenderer�� �����ϱ� ���� ������Ƽ
    /// </summary>
    public LineRenderer PlayerLineRendere => playerLineRenderer;

    /// <summary>
    /// Map UI�� ī�޶�
    /// </summary>
    Camera mapCamera;

    /// <summary>
    /// mapCamera�� �����ϱ� ���� ������Ƽ
    /// </summary>
    public Camera MapCamera => mapCamera;

    private void Awake()
    {
        Instance = this;

        InitalizeMapFunctions();
    }

    private void Start()
    {
        GenerateWorldMapUI(mapSizeX, mapSizeY);
    }

    /// <summary>
    /// Map�� ���õ� �ʱ�ȭ �Լ��� ��Ƶ� �Լ�
    /// </summary>
    private void InitalizeMapFunctions()
    {
        InitalizeMapUI();
        InitializeMapColor();
    }

    private void InitalizeMapUI()
    {
        mapPanelUI = FindObjectOfType<MapPanelUI>();

        if(mapPanelUI == null)
        {
            Debug.LogWarning("[MapManager] : MapPanelUI�� �������� �ʽ��ϴ�.");
        }

        playerLineRenderer = GameObject.Find("PlayerFollowLine")?.GetComponent<LineRenderer>();

        if(playerLineRenderer == null)
        {
            Debug.LogWarning("[MapManager] : playerlineRenderer�� ���� �����ʽ��ϴ�. " +
                "/ ������Ʈ �̸��� Ȯ�����ּ��� (PlayerFollowLine)");            
        }

        mapCamera = GameObject.Find("MapCamera")?.GetComponent<Camera>();
        if (playerLineRenderer == null)
        {
            Debug.LogWarning("[MapManager] : mapCamera ���� �����ʽ��ϴ�. " +
                "/ ������Ʈ �̸��� Ȯ�����ּ��� (MapCamera)");
        }
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
    /// Map Object Panel�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="mapSizeX">�ִ� �� ������ x</param>
    /// <param name="mapSizeY">�ִ� �� ������ y</param>
    void GenerateWorldMapUI(float mapSizeX, float mapSizeY)
    {
        int panelCountX = Mathf.FloorToInt(mapSizeX / panelSize); // x��ǥ���� �г� ����
        int panelCountY = Mathf.FloorToInt(mapSizeY / panelSize); // y��ǥ���� �г� ����

        GameObject MapObj = new GameObject("MapObject");
        MapObj.transform.parent = transform;
        MapObj.transform.localPosition = new Vector3(0, 20f, 0);

        // panel ����
        for(int y = 0; y < panelCountY; y++)
        {
            for (int x = 0; x < panelCountX; x++)
            {
                Vector3 objVector = new Vector3(x * panelSize, 0f, y * panelSize);
                GameObject panelobj = Instantiate(panelPrefab, Vector3.zero, Quaternion.Euler(90f,0f,0f), MapObj.transform);
                panelobj.transform.localPosition = objVector;
                panelobj.AddComponent<MapObject>().mapObject = panelobj;
                //panelobj.name = $"NO.{y * panelCountX + x} Panel";
                panelobj.name = $"[{x}]_[{y}] Panel";
            }
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