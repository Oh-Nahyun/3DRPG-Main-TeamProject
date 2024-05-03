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
    /// Linerenderer�� �ִ� ���� ����
    /// </summary>
    public int lineMaxCount = 10;

    /// <summary>
    /// LineRenderer�� Y��ǥ ��
    /// </summary>
    public float lineY = 50f;

    /// <summary>
    /// LineRenderere�� ���� 
    /// </summary>
    public float LineWidth = 5f;

    /// <summary>
    /// LineRenderer�� ��ġ������ �ϱ����� �÷��̾� ��ġ ����
    /// </summary>
    public Vector3 playerPos;

    /// <summary>
    /// LineRenderer�� ���� ��ġ ��
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
    }

    /// <summary>
    /// �÷��̾ ����ٴϴ� Map Camera ��ġ ���� �Լ�
    /// </summary>
    void FollowMapCam()
    {
        if (GameManager.Instance.Player.IsOpenMapPanel == false)
        {
            GameManager.Instance.MapManager.SetCameraPosition(playerPos);
        }
    }

    /// <summary>
    /// PlayerMapController ���� �ʱ�ȭ �Լ�
    /// </summary>
    void Initialize()
    {
        largeMap_CanvasGroup = GameManager.Instance.MapManager.LargeMapPanelUI.GetComponent<CanvasGroup>();
        playerLineRenderer = GameManager.Instance.MapManager.PlayerLineRendere;

        InitLine();
    }

    /// <summary>
    /// LineRenderer �ʱ�ȭ
    /// </summary>
    private void InitLine()
    {
        // ������ �ʱ�ȭ
        playerLineRenderer.positionCount = 0;

        // LineRenderer ���� ����
        playerLineRenderer.startWidth = LineWidth;
        playerLineRenderer.endWidth = LineWidth;
    }

    /// <summary>
    /// Linerenderer�� �׸��� �Լ�
    /// </summary>
    void DrawLine()
    {
        //playerPos = new Vector3(Mathf.FloorToInt(transform.position.x), lineY, Mathf.FloorToInt(transform.position.z));   // Line Position ��ġ
        playerPos = new Vector3(transform.position.x, lineY, transform.position.z);   // Line Position ��ġ

        if (playerLineRenderer.positionCount == 0) // ���� ���� ( �Ÿ��� ������ ���� ���� ���� ������ )
        {
            AddLine(playerPos);
            prePos = playerPos;                                                 // ���� ��ġ�� ����

            //linePrefab.positionCount++;                                         // size ����
        }
        else
        {
            float betweenVertex = (playerPos - prePos).sqrMagnitude;    // �Ÿ�
            float maxLength = 5f;                                       // �� Vertex�� �ִ� �Ÿ�
            if (betweenVertex > maxLength * maxLength)                  // betweenVertex���� �Ÿ��� ũ��
            {
                if (playerLineRenderer.positionCount > 10)
                {
                    playerPos = new Vector3(transform.position.x, lineY, transform.position.z);
                    AddLine(playerPos);
                    ResetLines(playerLineRenderer.positionCount);
                }

                AddLine(playerPos);
                prePos = playerPos; // ���� ��ġ�� ����
            }
        }
    }

    /// <summary>
    /// ������ �߰� �ϴ� �Լ�
    /// </summary>
    /// <param name="linePosition">�߰��� ���� ��ġ</param>
    void AddLine(Vector3 linePosition)
    {
        playerLineRenderer.positionCount++;
        playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, linePosition);    // ���ο� LineRenderer ��ġ ����
    }

    /// <summary>
    /// ���� ������ �ִ� ����(lineMaxCount)�� �����ϸ� �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    /// <param name="lineCount">üũ�� ���� ��</param>
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
