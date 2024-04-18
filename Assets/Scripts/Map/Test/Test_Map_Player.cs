using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Map_Player : MonoBehaviour
{
    PlayerinputActions inputActions;
    public CanvasGroup map_CanvasGroup;
    public LineRenderer playerLineRenderer;
    public Camera mapCamera;

    public float speed = 5f;
    public float rotatePower = 90f;

    float moveInput = 0f;
    float rotateInput = 0f;

    public int lineMaxCount = 10;

    void Awake()
    {
        inputActions = new PlayerinputActions();

        InitLine();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Open_Map.performed += OnOpenMap;
        inputActions.Player.Open_Map.canceled += OnOpenMap;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }
    void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Open_Map.canceled -= OnOpenMap;
        inputActions.Player.Open_Map.performed -= OnOpenMap;
        inputActions.Player.Disable();
        
    }

    void Update()
    {
        transform.position += transform.forward * moveInput * Time.deltaTime;
        transform.Rotate(Vector3.up * rotateInput * Time.deltaTime);

        DrawLine();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        moveInput = inputVector.y * speed;
        rotateInput = inputVector.x * rotatePower;
    }

    private void OnOpenMap(InputAction.CallbackContext context)
    {
        // �ӽ� �¿���
        if(context.performed)
        {
            if(map_CanvasGroup.alpha == 1f)
            {
                map_CanvasGroup.alpha = 0f;
            }
            else
            {
                map_CanvasGroup.alpha = 1f;
            }
        }
    }

    /// <summary>
    /// LineRenderer �ʱ�ȭ
    /// </summary>
    void InitLine()
    {
        // ������ �ʱ�ȭ
        playerLineRenderer.positionCount = 0;

        // LineRenderer ���� ����
        playerLineRenderer.startWidth = 5f;
        playerLineRenderer.endWidth = 5f;
    }

    /// <summary>
    /// LineRenderer�� ��ġ������ �ϱ����� �÷��̾� ��ġ ����
    /// </summary>
    public Vector3 playerPos;

    /// <summary>
    /// LineRenderer�� ���� ��ġ ��
    /// </summary>
    public Vector3 prePos;

    void DrawLine()
    {
        playerPos = new Vector3(Mathf.FloorToInt(transform.position.x), 10f, Mathf.FloorToInt(transform.position.z));   // Line Position ��ġ

        if(playerLineRenderer.positionCount == 0) // ���� ���� ( �Ÿ��� ������ ���� ���� ���� ������ )
        {
            AddLine(playerPos);
            prePos = playerPos;                                                 // ���� ��ġ�� ����

            //linePrefab.positionCount++;                                         // size ����
        }
        else
        {
            float betweenVertex = (playerPos - prePos).sqrMagnitude;    // �Ÿ�
            float maxLength = 5f;                                       // �� Vertex�� �ִ� �Ÿ�
            if(betweenVertex >= maxLength * maxLength)                  // betweenVertex���� �Ÿ��� ũ��
            {
                if (playerLineRenderer.positionCount > 10)
                {
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
        if(lineCount > lineMaxCount)
        {

            playerLineRenderer.positionCount = 2;
            playerLineRenderer.SetPosition(0, prePos);
            playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, playerPos);
        }
    }
}