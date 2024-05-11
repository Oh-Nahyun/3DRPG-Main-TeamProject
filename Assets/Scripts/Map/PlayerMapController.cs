using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMapController : MonoBehaviour
{
    CanvasGroup largeMap_CanvasGroup;
    LineRenderer playerLineRenderer;

    /// <summary>
    /// Linerenderer의 최대 정점 개수
    /// </summary>
    public int lineMaxCount = 10;

    /// <summary>
    /// LineRenderer의 Y좌표 값
    /// </summary>
    public float lineY = 50f;

    /// <summary>
    /// LineRenderere의 넓이 
    /// </summary>
    public float LineWidth = 5f;

    /// <summary>
    /// LineRenderer을 위치설정을 하기위한 플레이어 위치 벡터
    /// </summary>
    public Vector3 playerPos;

    /// <summary>
    /// LineRenderer의 이전 위치 값
    /// </summary>
    public Vector3 prePos;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        DrawLine();
    }

    private void LateUpdate()
    {
        FollowMapCam();
        MapManager.Instance.SetPlayerMarkPosition(playerPos);
    }

    /// <summary>
    /// 플레이어를 따라다니는 Map Camera 위치 설정 함수
    /// </summary>
    void FollowMapCam()
    {
        if(!GameManager.Instance.MapManager.IsOpenedLargeMap)
        {
            MapManager.Instance.SetCameraPosition(playerPos);
        }
    }

    /// <summary>
    /// PlayerMapController 변수 초기화 함수
    /// </summary>
    void Initialize()
    {
        largeMap_CanvasGroup = MapManager.Instance.LargeMapPanelUI.GetComponent<CanvasGroup>();
        playerLineRenderer = MapManager.Instance.PlayerLineRendere;

        InitLine();
    }

    /// <summary>
    /// LineRenderer 초기화
    /// </summary>
    private void InitLine()
    {
        // 사이즈 초기화
        playerLineRenderer.positionCount = 0;

        // LineRenderer 넓이 설정
        playerLineRenderer.startWidth = LineWidth;
        playerLineRenderer.endWidth = LineWidth;
    }

    /// <summary>
    /// Linerenderer을 그리는 함수
    /// </summary>
    void DrawLine()
    {
        //playerPos = new Vector3(Mathf.FloorToInt(transform.position.x), lineY, Mathf.FloorToInt(transform.position.z));   // Line Position 위치
        playerPos = new Vector3(transform.position.x, lineY, transform.position.z);   // Line Position 위치

        if (playerLineRenderer.positionCount == 0) // 최초 지점 ( 거리를 측정할 이전 값이 없기 때문에 )
        {
            AddLine(playerPos);
            prePos = playerPos;                                                 // 이전 위치값 저장

            //linePrefab.positionCount++;                                         // size 증가
        }
        else
        {
            float betweenVertex = (playerPos - prePos).sqrMagnitude;    // 거리
            float maxLength = 5f;                                       // 각 Vertex의 최대 거리
            if (betweenVertex > maxLength * maxLength)                  // betweenVertex보다 거리가 크다
            {
                if (playerLineRenderer.positionCount > 10)
                {
                    playerPos = new Vector3(transform.position.x, lineY, transform.position.z);
                    AddLine(playerPos);
                    ResetLines(playerLineRenderer.positionCount);
                }

                AddLine(playerPos);
                prePos = playerPos; // 이전 위치값 저장
            }
        }
    }

    /// <summary>
    /// 라인을 추가 하는 함수
    /// </summary>
    /// <param name="linePosition">추가할 라인 위치</param>
    void AddLine(Vector3 linePosition)
    {
        playerLineRenderer.positionCount++;
        playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, linePosition);    // 새로운 LineRenderer 위치 설정
    }

    /// <summary>
    /// 라인 개수가 최대 개수(lineMaxCount)에 도달하면 초기화 하는 함수
    /// </summary>
    /// <param name="lineCount">체크할 라인 수</param>
    void ResetLines(int lineCount)
    {
        if (lineCount > lineMaxCount)
        {
            playerLineRenderer.positionCount = 2;
            playerLineRenderer.SetPosition(0, prePos);
            playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, playerPos);
        }
    }
}
