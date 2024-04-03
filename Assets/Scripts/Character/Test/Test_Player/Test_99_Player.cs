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
    Rigidbody rigid;
    Animator animator;
    #region PlayerMove

    /// <summary>
    /// �Էµ� �̵� ����
    /// </summary>
    //Vector3 inputDirection = Vector3.zero;

    /// <summary>
    /// �̵� ���� (1 : ����, -1 : ����, 0 : ����)
    /// </summary>
    float moveDirection = 0.0f;

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
    /// ȸ�� ���� (1 : ��ȸ��, -1 : ��ȸ��, 0 : ����)
    /// </summary>
    float rotateDirection = 0.0f;

    /// <summary>
    /// ȸ�� �ӵ�
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// ���� ����
    /// </summary>
    public float jumpPower = 5.0f;

    /// <summary>
    /// ���� ������ �ƴ��� Ȯ�ο� ����
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// ������ �������� Ȯ���ϴ� ������Ƽ (�������� �ƴ� ��)
    /// </summary>
    bool IsJumpAvailable => !isJumping;

    // �ִϸ����Ϳ� �ؽð�
    readonly int IsMoveBackHash = Animator.StringToHash("IsMoveBack");
    readonly int IsJumpHash = Animator.StringToHash("IsJump");
    readonly int IsAttackHash = Animator.StringToHash("IsAttack");
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
    /// ī�޶� ȸ���� �ڽ� Ʈ������
    /// </summary>
    Transform cameraRoot;
    /// <summary>
    /// ī�޶� ȸ���� ������Ƽ
    /// </summary>
    public Transform CameraRoot
    {
        get => cameraRoot;
    }
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
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        controller.onMove += Move;
    }
    void Update()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * moveDirection * transform.forward);
    }

    /// <summary>
    /// Get Player input Values
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="isMove">check press button ( wasd )</param>
    void Move(Vector2 input, bool isMove)
    {
        // �Է� ���� ����
        rotateDirection = input.x;
        moveDirection = input.y;

        // �Է��� ������ ��Ȳ
        if (isMove)
        {
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
}
