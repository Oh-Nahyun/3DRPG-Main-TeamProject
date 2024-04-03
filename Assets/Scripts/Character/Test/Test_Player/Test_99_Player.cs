using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// �÷��̾� ��ũ��Ʈ
/// </summary>
public class Test_99_Player : MonoBehaviour
{
    Test_99_PlayerController controller;
    Test_99_PlayerSkills skills;

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

    #region Skills
    /// <summary>
    /// ��ü�� ������ ��
    /// </summary>
    public float throwPower = 5.0f;
    /// <summary>
    /// ������ ���� �� �ִ� ����(������)
    /// </summary>
    public float liftRadius = 0.5f;
    /// <summary>
    /// ������ ���� �� �ִ� ����(����)
    /// </summary>
    public float pickUpHeightRange = 0.5f;

    /// <summary>
    /// �Է��� �ִ��� �ľǿ� (true: �Է��� ����)
    /// </summary>
    bool isMoveInput = false;

    /// <summary>
    /// �Է� ����
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// ĳ���� �𵨸� �ڽ� Ʈ������
    /// </summary>
    Transform character;
    /// <summary>
    /// ������Ʈ�� ��� ������ �ڽ� Ʈ������
    /// </summary>
    Transform pickUpRoot;

    /// <summary>
    /// �÷��̾� ��ų��
    /// </summary>
    PlayerSkillController skillController;
    /// <summary>
    /// �÷��̾� ��ų�� ������Ƽ
    /// </summary>
    public PlayerSkillController SkillController => skillController;
    /// <summary>
    /// �÷��̾� ��ų��� �� ������Ʈ ���� ���� ��ġ ������ Ʈ������ (�÷��̾�� ������ ȸ������ ���� = ������ ����)
    /// </summary>
    HandRootTracker handRootTracker;

    /// <summary>
    /// ���� ������� ��ų�� �ִ��� Ȯ�� (true: ��ų �����)
    /// </summary>
    bool IsSkillOn => SkillController.CurrentOnSkill != null;

    // �Է¿� ��������Ʈ
    /// <summary>
    /// ��Ŭ��: ��ȣ�ۿ�
    /// </summary>
    public Action rightClick;
    /// <summary>
    /// ��Ŭ��: ���� (��ų���� ��� x)
    /// </summary>
    public Action leftClick;
    /// <summary>
    /// ��: ���׳�ĳġ ����� �յ��̵�
    /// </summary>
    public Action<float> onScroll;
    /// <summary>
    /// z: ������
    /// </summary>
    Action onThrow;
    /// <summary>
    /// f: ��ų ���
    /// </summary>
    public Action onSkill;
    /// <summary>
    /// x: ��� (�߼� �ൿ �ľ���)
    /// </summary>
    public Action onCancel;

    /// <summary>
    /// ���õ� ��ų�� �ٲ������ �˸��� ��������Ʈ (F1:��������ź F2:��������źť�� F3:���׳�ĳġ F4:���̽�����Ŀ F5:Ÿ�ӷ�)
    /// </summary>
    public Action<SkillName> onSkillSelect;

    /// <summary>
    /// ������Ʈ�� ����� ��츦 �˸��� ��������Ʈ
    /// </summary>
    public Action onPickUp;

    /// <summary>
    /// ���� ���õ� ��ų (���� �ش� ��ų�� �ߵ���)
    /// </summary>
    SkillName selectSkill = SkillName.RemoteBomb;

    /// <summary>
    /// ���� ���õ� ��ų�� ������Ƽ
    /// </summary>
    SkillName SelectSkill
    {
        get => selectSkill;
        set
        {
            if (selectSkill != value)
            {
                switch (selectSkill)
                {
                    case SkillName.RemoteBomb:
                    case SkillName.RemoteBomb_Cube:
                    case SkillName.IceMaker:
                    case SkillName.TimeLock:
                        if (reaction != null && reaction.transform.CompareTag("Skill"))     // ��������ź���� ��ų�� ��� �ִ� ���
                        {
                            DropObject();   // ���� ������
                        }
                        break;
                    case SkillName.MagnetCatch: // ���׳�ĳġ�� Ȱ��ȭ �� ���¸� ��ų ���� �Ұ���
                        value = selectSkill;
                        break;
                }
                selectSkill = value;            // ���� ��ų ����
                Debug.Log($"��ų [{selectSkill}]�� ����");
                onSkillSelect?.Invoke(selectSkill);         // ���� ���õ� ��ų�� �˸�
            }
        }
    }

    /// <summary>
    /// ������Ʈ�� ������ �� Ȯ��(true: ������ ��)
    /// </summary>
    bool isPickUp = false;

    /// <summary>
    /// ������Ʈ�� ������ �� Ȯ�ο� ������Ƽ
    /// </summary>
    bool IsPickUp
    {
        get => isPickUp;
        set
        {
            if (isPickUp != value)  // �ٸ� ���� ���� ���� = �Ǽ��϶��� �� �� �ְ� ��� ���� ���� ���� �� ����
            {
                isPickUp = value;
                animator.SetBool(Hash_IsPickUp, isPickUp);
                // �߰�: ���׳� �ִϸ��̼� �� �ٸ� �ִϸ��̼� if�� �����ϱ�
            }
        }
    }

    // �ִϸ��̼� �ؽ�
    readonly int Hash_IsMove = Animator.StringToHash("IsMove");
    readonly int Hash_IsPickUp = Animator.StringToHash("IsPickUp");
    readonly int Hash_Throw = Animator.StringToHash("Throw");

    /// <summary>
    /// ���� ����ִ� ������Ʈ (������� ������ null)
    /// </summary>
    ReactionObject reaction;

    #endregion


    void Awake()
    {
        controller = GetComponent<Test_99_PlayerController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
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

    #region skill function
    /// <summary>
    /// ������Ʈ�� ��� �޼���
    /// </summary>
    void PickUpObject()
    {
        IsPickUp = true;
        onPickUp?.Invoke();
        reaction.PickUp(handRootTracker.transform);         // ���� ���
        reaction.transform.rotation = Quaternion.identity;  // ������ ȸ���� ���ֱ� = �÷��̾��� ����� ���߱�
    }
    /// <summary>
    /// ������Ʈ ������ �޼���
    /// </summary>
    void ThrowObject()
    {
        if (IsPickUp && reaction != null)
        {
            animator.SetTrigger(Hash_Throw);
            reaction.Throw(throwPower, transform);
            IsPickUp = false;
            reaction = null;
        }
    }

    /// <summary>
    /// ��� �ൿ�� �޼��� (���� Ȯ����)
    /// </summary>
    void DropObject()
    {
        // ���Ű �߼����� Ȯ���ϱ�
        /*if(IsPickUp && reaction != null)
        {
            IsPickUp = false;
            reaction.Drop();
            reaction = null;
        }*/
        if (IsSkillOn && reaction != null)          // ��ų�� ������̸� ��� ���
        {
            IsPickUp = false;
            reaction.Drop();
            reaction = null;
        }
    }

    #endregion
}
