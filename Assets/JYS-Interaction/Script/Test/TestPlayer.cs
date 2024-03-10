using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class TestPlayer : MonoBehaviour
{

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    float currentSpeed = 5.0f;

    /// <summary>
    /// �Էµ� �̵� ����
    /// </summary>
    Vector3 inputDirection = Vector3.zero;  // y�� ������ �ٴ� ����

    /// <summary>
    /// ĳ���� ȸ�� �ӵ�
    /// </summary>
    public float turnSpeed = 10.0f;

    // �Է¿� ��ǲ �׼�
    PlayerInputActions inputActions;

    GameManager gameManager;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Use.performed += OnUse;
    }

    private void OnDisable()
    {
        inputActions.Player.Use.performed -= OnUse;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * currentSpeed * inputDirection);   // �� �� ����
        //characterController.SimpleMove(currentSpeed * inputDirection);            // �� �� �ڵ�
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);  // ��ǥ ȸ������ ����
        if (!(inputDirection.x == 0 && inputDirection.z == 0))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDirection), Time.deltaTime * turnSpeed);
        }
    }

    /// <summary>
    /// �̵� �Է� ó���� �Լ�
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();

        inputDirection.x = input.x;     // �Է� ���� ����
        inputDirection.y = 0;
        inputDirection.z = input.y;

    }

    private void OnUse(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
       
    }


}
