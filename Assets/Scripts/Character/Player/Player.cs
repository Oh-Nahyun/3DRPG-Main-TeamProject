using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

/// <summary>
/// �÷��̾� ��ũ��Ʈ
/// </summary>
public class Player : MonoBehaviour
{
    PlayerController controller;
    
    /// <summary>
    /// PlayerSKills�� �ޱ����� ������Ƽ
    /// </summary>
    public PlayerSkills Skills => gameObject.GetComponent<PlayerSkills>();

    Animator animator;
    CharacterController characterController;

    #region PlayerMove
    /// <summary>
    /// �Էµ� �̵� ����
    /// </summary>
    Vector3 inputDirection = Vector3.zero;

    /// <summary>
    /// �ȴ� �ӵ�
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// �޸��� �ӵ�
    /// </summary>
    public float runSpeed = 7.0f;

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    public float currentSpeed = 0.0f;

    /// <summary>
    /// �̵� ���
    /// </summary>
    enum MoveMode
    {
        Walk = 0,   // �ȱ�
        Run         // �޸���
    }

    /// <summary>
    /// ���� �̵� ���
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Walk;

    /// <summary>
    /// ���� �̵� ��� Ȯ�� �� ������ ������Ƽ
    /// </summary>
    MoveMode CurrentMoveMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;        // ���� ����
            if (currentSpeed > 0.0f)        // �̵� ������ �ƴ��� Ȯ��
            {
                // �̵� ���̸� ��忡 �°� �ӵ��� �ִϸ��̼� ����
                MoveSpeedChange(currentMoveMode);
            }
        }
    }

    /// <summary>
    /// ĳ������ ��ǥ�������� ȸ����Ű�� ȸ��
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// ȸ�� �ӵ�
    /// </summary>
    public float turnSpeed = 10.0f;

    /// <summary>
    /// �߷�
    /// </summary>
    //[Range(-1, 1)]
    //public float gravity = 0.96f;

    /// <summary>
    /// �����̵� ����
    /// </summary>
    public float slidePower = 5.0f;

    /// <summary>
    /// ���� �ð� ����
    /// </summary>
    //public float jumpTimeLimit = 4.0f;

    /// <summary>
    /// ���� �ð�
    /// </summary>
    //[SerializeField]
    //public float jumpTime;

    /// <summary>
    /// ���� ����
    /// </summary>
    public float jumpPower = 5.0f;

    /// <summary>
    /// ���� �ӵ���
    /// </summary>
    //public float jumpVelocity;

    /// <summary>
    /// ���� ������ �ƴ��� Ȯ�ο� ����
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// ������ �������� Ȯ���ϴ� ������Ƽ (�������� �ƴ� ��)
    /// </summary>
    bool IsJumpAvailable => !isJumping;

    /// <summary>
    /// �ֺ� �þ� ��ư�� ���ȴ��� �ƴ��� Ȯ�ο� ����
    /// </summary>
    public bool isLook = false;

    /// <summary>
    /// �ֺ� �þ� ���� ����
    /// </summary>
    Vector3 lookVector = Vector3.zero;

    /// <summary>
    /// �ֺ� �þ� ī�޶�
    /// </summary>
    public GameObject cameraRoot;

    /// <summary>
    /// �ֺ� �þ� ī�޶� ȸ�� ����
    /// </summary>
    public float followCamRotatePower = 5.0f;

    // �ִϸ����Ϳ� �ؽð�
    //readonly int IsMoveBackHash = Animator.StringToHash("IsMoveBack");
    readonly int IsJumpHash = Animator.StringToHash("IsJump");
    readonly int IsSlideHash = Animator.StringToHash("IsSlide");
    readonly int SpeedHash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;
    #endregion

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        // exception
        if (cameraRoot == null)
        {
            Debug.LogError("CameraRoot�� ����ֽ��ϴ�. CameraRoot Prefab ������Ʈ�� �־��ּ��� ( PlayerLookVCam ��ũ��Ʈ �ִ� ������Ʈ )");
        }

        controller.onMove += OnMove;
        controller.onMoveModeChagne += OnMoveModeChange;
        controller.onLook += OnLookAround;
        controller.onSlide += OnSlide;
        controller.onJump += OnJump;
    }

    void Update()
    {        
        LookRotation();
        Jump();
    }

    void FixedUpdate()
    {
        characterController.Move(Time.fixedDeltaTime * currentSpeed * inputDirection); // ĳ������ ������
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);  // ��ǥ ȸ������ ����
    }

    /// <summary>
    /// Get Player input Values
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="isMove">check press button ( wasd )</param>
    void OnMove(Vector2 input, bool isMove)
    {
        // �Է� ���� ����
        inputDirection.x = input.x;
        inputDirection.y = 0;
        inputDirection.z = input.y;

        // �Է��� ������ ��Ȳ
        if (isMove)
        {
            // �Է� ���� ȸ����Ű��
            Quaternion followCamY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);   // ī�޶��� yȸ���� ���� ����
            inputDirection = followCamY * inputDirection;                                                   // �Է� ������ ī�޶��� yȸ���� ���� ������ ȸ����Ű��
            targetRotation = Quaternion.LookRotation(inputDirection);                                       // ȸ�� ����

            // �̵� ��� ����
            MoveSpeedChange(CurrentMoveMode);
        }

        // �Է��� ���� ��Ȳ
        else
        {
            currentSpeed = 0.0f; // ����
            animator.SetFloat(SpeedHash, AnimatorStopSpeed);
        }
    }

    /// <summary>
    /// �̵� ��� ���� �Լ�
    /// </summary>
    private void OnMoveModeChange()
    {
        if (CurrentMoveMode == MoveMode.Walk)
        {
            CurrentMoveMode = MoveMode.Run;
        }
        else
        {
            CurrentMoveMode = MoveMode.Walk;
        }
    }

    /// <summary>
    /// Check Look Around
    /// </summary>
    /// <param name="lookInput">lookInput value</param>
    /// <param name="isLookingAround">true : , false : No input Value</param>
    void OnLookAround(Vector2 lookInput, bool isLookingAround)
    {
        if (isLookingAround)
        {
            isLook = true;
            lookVector = lookInput;
        }

        if (!isLookingAround)
        {
            isLook = false;
        }
    }

    /// <summary>
    /// Change Player Movemode
    /// </summary>
    /// <param name="mode">MoveMode</param>
    void MoveSpeedChange(MoveMode mode)
    {
        // �̵� ��忡 ���� �ӵ��� �ִϸ��̼� ����
        switch (mode)
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                animator.SetFloat(SpeedHash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                animator.SetFloat(SpeedHash, AnimatorRunSpeed);
                break;
        }
    }

    /// <summary>
    /// Player Camera Rotation
    /// </summary>
    void LookRotation()
    {
        if (!isLook)
            return;

        cameraRoot.transform.localRotation *= Quaternion.AngleAxis(lookVector.x * followCamRotatePower, Vector3.up);
        cameraRoot.transform.localRotation *= Quaternion.AngleAxis(-lookVector.y * followCamRotatePower, Vector3.right);

        var angles = cameraRoot.transform.localEulerAngles;
        angles.z = 0;

        var angle = cameraRoot.transform.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        cameraRoot.transform.localEulerAngles = angles;
        cameraRoot.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);
    }
    private void OnJump(bool isJump)
    {
        if (isJump)
        {
            // �����ϴ� ���� �ƴ� ��� => ���� ����
            isJumping = false;
        }
        else
        {
            // ���� ���� ��� => ���� �Ұ���
            isJumping = true;
        }
    }

    void Jump()
    {
        // ������ ������ ���
        if (IsJumpAvailable)
        {
            animator.SetTrigger(IsJumpHash);
        }

        isJumping = true;
    }

    /// <summary>
    /// ȸ�� ó�� �Լ�
    /// </summary>
    private void OnSlide()
    {
        animator.SetTrigger(IsSlideHash);
    }

    /// <summary>
    /// ĳ������ Collider�� �Ѵ� �Լ� (Animation ������)
    /// </summary>
    public void CharacterColliderEnable()
    {
        characterController.enabled = true;
    }

    /// <summary>
    /// ĳ������ Collider�� ���� �Լ� (Animation ������)
    /// </summary>
    public void CharacterColliderDisable()
    {
        characterController.enabled = false;
    }

    // 
    public void LookForwardPlayer(Vector3 rotate)
    {
        //rotate.x = 0;
        //rotate.z = 0;
        rotate.y = 0;
        transform.forward = rotate;
    }
}