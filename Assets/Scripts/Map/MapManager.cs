using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Map UI�� ���� ���� �ٷ�� Manager Ŭ���� ( ��ġ�� ���� ���� �ؿ� ���� )
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
    MapPanelUI largeMapPanelUI;

    /// <summary>
    /// �� �г� UI�� �����ϱ� ���� ������Ƽ
    /// </summary>
    public MapPanelUI LargeMapPanelUI => largeMapPanelUI;

    /// <summary>
    /// largeMap panel canvasGruop
    /// </summary>
    CanvasGroup largeMapCanvasGroup;

    /// <summary>
    /// �̴ϸ� �г�
    /// </summary>
    CanvasGroup miniMapPanelUI;

    /// <summary>
    /// �̴ϸ� �г� ������ ���� ������Ƽ
    /// </summary>
    public CanvasGroup MiniMapPanelUI => miniMapPanelUI;

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

    public GameObject playerMark;

    /// <summary>
    /// �� ī�޶��� y ���� ��ǥ��
    /// </summary>
    const float mapCameraY = 100f;

    private void Awake()
    {
        Instance = this;

        InitalizeMapFunctions();
    }

    /// <summary>
    /// Map�� ���õ� �ʱ�ȭ �Լ��� ��Ƶ� �Լ�
    /// </summary>
    private void InitalizeMapFunctions()
    {
        InitalizeMapUI();
    }

    private void InitalizeMapUI()
    {
        largeMapPanelUI = FindObjectOfType<MapPanelUI>();

        if(largeMapPanelUI == null)
        {
            Debug.LogWarning("[MapManager] : MapPanelUI�� �������� �ʽ��ϴ�.");
        }
        else
        {
            largeMapCanvasGroup = LargeMapPanelUI.GetComponent<CanvasGroup>();
        }

        miniMapPanelUI = GameObject.Find("MiniMapPanel")?.GetComponent<CanvasGroup>();

        if (miniMapPanelUI == null)
        {
            Debug.LogWarning("[MapManager] : miniMapPanelUI �������� �ʽ��ϴ�." +
                "/ ������Ʈ �̸��� Ȯ�����ּ��� (MiniMapPanel)");
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

    #region MapPanelMethods

    /// <summary>
    /// large ���� Ű�� �Լ� ( largeMap ����, miniMap ���� )
    /// </summary>
    public void OpenMapUI()
    {
        largeMapCanvasGroup.alpha = 1.0f;
        largeMapCanvasGroup.interactable = true;
        largeMapCanvasGroup.blocksRaycasts = true;
        miniMapPanelUI.alpha = 0.0f;

        MapCamera.orthographicSize = 50f;
    }

    /// <summary>
    /// large ���� ���� �Լ� ( largeMap ����, miniMap ���� )
    /// </summary>
    public void CloseMapUI()
    {
        largeMapCanvasGroup.alpha = 0.0f;
        largeMapCanvasGroup.interactable = false;
        largeMapCanvasGroup.blocksRaycasts = false;
        miniMapPanelUI.alpha = 1.0f;

        MapCamera.orthographicSize = 20f;
    }

    /// <summary>
    /// �̴ϸ� �� �� �����ϴ� �Լ�
    /// </summary>
    public void OpenMiniMapUI()
    {
        miniMapPanelUI.alpha = 1.0f;
    }

    /// <summary>
    /// �̴ϸ� �� �� �����ϴ� �Լ�
    /// </summary>
    public void CloseMiniMapUI()
    {
        miniMapPanelUI.alpha = 0.0f;        
    }

    #endregion

    #region ObjectSetting

    /// <summary>
    /// ī�޶� ��ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="position"> �߰��� ī�޶� ��ġ�� ( y��ǥ���� 100���� ���� ) </param>
    public void SetCameraPosition(Vector3 position)
    {
        //Transform child = transform.GetChild(0); // MapObject

        float minX = transform.position.x; // MapManager�� ���� ���� �ϴܿ� �ִ�.
        float minY = transform.position.z;
        float maxX = mapSizeX * 0.5f; // ���� ����� Panel ��ǥ��
        float maxY = mapSizeY * 0.5f;
        //float maxX = child.GetChild(child.childCount - 1).position.x; // ���� ����� Panel ��ǥ��
        //float maxY = child.GetChild(child.childCount - 1).position.z;
        
        mapCamera.transform.position += position; // ī�޶� ��ġ ����
        
        // ī�޶� ��ġ�� ���� ����
        mapCamera.transform.position = new Vector3
                (Mathf.Clamp(position.x, minX, maxX),
                mapCameraY,
                Mathf.Clamp(position.z, minY, maxY));
    }

    /// <summary>
    /// Player Mark ��ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="position">������ ��ġ</param>
    public void SetPlayerMarkPosition(Vector3 position)
    {
        playerMark.transform.position = position;
    }
    #endregion
}