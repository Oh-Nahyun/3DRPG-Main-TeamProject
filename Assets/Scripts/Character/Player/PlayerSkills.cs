using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using static System.Collections.Specialized.BitVector32;

/// <summary>
/// �÷��̾��� ��ų���븸 �޴� ��ũ��Ʈ
/// </summary>
public class PlayerSkills : MonoBehaviour
{
    // components
    PlayerSkillController skillController;

    ReactionObject currentOnSkill;
    public ReactionObject CurrentOnSkill => currentOnSkill;

    RemoteBomb remoteBomb;
    RemoteBombCube remoteBombCube;
    MagnetCatch magnetCatch;

    Animator animator;

    /// <summary>
    /// �÷��̾� ��ų��� �� ������Ʈ ���� ���� ��ġ ������ Ʈ������ (�÷��̾�� ������ ȸ������ ���� = ������ ����)
    /// </summary>
    HandRootTracker handRootTracker;

    Transform handRootTrackerTransform;
    public Transform HandRoot => handRootTrackerTransform;

    /// <summary>
    /// ������Ʈ�� ��� ������ �ڽ� Ʈ������
    /// </summary>
    Transform pickUpRoot;

    /// <summary>
    /// ���� ����ִ� ������Ʈ (������� ������ null)
    /// </summary>
    ReactionObject reaction;

    // Delegates
    public Action onSKillAction;
    public Action useSkillAction;
    public Action offSkillAction;

    // properties
    SkillName currentSkill = SkillName.RemoteBomb;

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
                //
                skillController.onSkillSelect?.Invoke(selectSkill);         // ���� ���õ� ��ų�� �˸�
            }
        }
    }

    /// <summary>
    /// ������ ���� �� �ִ� ����(����)
    /// </summary>
    public float pickUpHeightRange = 0.5f;

    /// <summary>
    /// ��ü�� ������ ��
    /// </summary>
    public float throwPower = 5.0f;

    /// <summary>
    /// ������ ���� �� �ִ� ����(������)
    /// </summary>
    public float liftRadius = 0.5f;

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

    /// <summary>
    /// ���� ������� ��ų�� �ִ��� Ȯ�� (true: ��ų �����)
    /// </summary>
    bool IsSkillOn => CurrentOnSkill != null;

    // Hashes
    readonly int Hash_IsPickUp = Animator.StringToHash("IsPickUp");
    readonly int Hash_Throw = Animator.StringToHash("Throw");

    // ===

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


    // ===

    void Awake()
    {
        skillController = GetComponent<PlayerSkillController>();
        animator = GetComponent<Animator>();

        if (skillController == null)
        {
            Debug.LogError("skillController�� �������� �ʽ��ϴ�.");
        }
        else
        {
            skillController.onSkillSelect += ConnectSkill; // onSkillSelect�� ���� ��ũ��Ʈ ������ ó����

            //skillController.onSkillActive += () => onSKillAction?.Invoke(); // 
            skillController.onSkillActive += OnSkill; // 
            //skillController.rightClick += () => useSkillAction?.Invoke();
            skillController.onCancel += CancelSkill;

            //onSKillAction += OnSkill;

            animator = GetComponent<Animator>();                          // �ִϸ��̼��� �ڽ� Ʈ�������� �𵨿��� ó��

            HandRoot handRoot = transform.GetComponentInChildren<HandRoot>();       // �÷��̾� �� ��ġ�� ã�� �����Ƽ� ��ũ��Ʈ �־ ã��
            handRootTracker = transform.GetComponentInChildren<HandRootTracker>();  // �÷��̾� �� ��ġ�� �����ϴ� Ʈ������ => ����� ������Ʈ�� �ڽ����� ���� �� ������ �÷��̾��� �������� ���߱� ����
            handRootTrackerTransform = handRoot.transform; // 24.04.05

            pickUpRoot = transform.GetChild(2);

            //cameraRoot = transform.GetComponentInChildren<CameraRootMover>().transform;

            rightClick += PickUpObjectDetect;       // ��Ŭ�� = ���� ���
            onThrow += ThrowObject;                 // ������
            onCancel += DropObject;                // ���

            onPickUp += () => handRootTracker.OnTracking(handRoot.transform);   // ������ ��� ����ġ������ ����
            onSkill += () => handRootTracker.OnTracking(handRoot.transform);    // ��ų ���� ����ġ������ ����
            onCancel += handRootTracker.OffTracking;



            skillController.onRemoteBomb += OnRemoteBomb;  // skill 1
            skillController.onRemoteBomb_Cube += OnRemoteBomb_Cube;// skill 2
            skillController.onMagnetCatch += OnMagnetCatch;// skill 3
            skillController.onIceMaker += OnIceMaker;// skill 4
            skillController.onTimeLock += OnTimeLock; // skill 5
            skillController.onThrow += ThrowObject;
}
    }

    void OnSkill()
    {
        // ������ ��ų���� ����
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                if (remoteBomb == null)     // ��������ź�� ���� ��ȯ�Ǿ� ���� ������
                {
                    //Debug.Log("���� : ������ ��ź");
                    remoteBomb = Factory.Instance.GetRemoteBomb(); // ���丮���� ��������ź ������ �� ������ ��ź ������ ����
                    currentOnSkill = remoteBomb;                        // ���� ������� ��ų�� ��������ź
                }
                else
                {
                    CancelSkill();          // ��������ź�� ��ȯ�Ǿ� ������ �����鼭 ��ų ����
                }

                break;
            case SkillName.RemoteBomb_Cube:
                if (remoteBombCube == null)
                {
                    //Debug.Log("���� : ������ ��ź ť��");
                    remoteBombCube = Factory.Instance.GetRemoteBombCube();
                    currentOnSkill = remoteBombCube;
                }
                else
                {
                    CancelSkill();
                }
                break;
            case SkillName.MagnetCatch:
                if (magnetCatch == null)
                {
                    //Debug.Log("���� : ���׳� ĳġ");
                    magnetCatch = Factory.Instance.GetMagnetCatch();
                    currentOnSkill = magnetCatch;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }

        ConnectSkill(currentSkill);

        if (currentOnSkill != null)
        {
            currentOnSkill.PickUp(HandRoot);

            //currentOnSkill.transform.SetParent(HandRoot);
            //currentOnSkill.transform.position = HandRoot.position;
            //currentOnSkill.transform.forward = player.transform.forward;
        }

        onSKillAction?.Invoke();
    }

    void ConnectSkill(SkillName skiilName)
    {
        // ���� ��ų ������ ����
        currentSkill = skiilName;

        // ��ų ����� ��������Ʈ ���� ����
        onSKillAction = null;
        useSkillAction = null;
        offSkillAction = null;

        // ������ ��ų�� ���� �ߵ� ���̸� ��ų ����(����, �����, ����) ��������Ʈ ����
        // ������ ��ų�� ���������� ��ų ���� (�� ��ų�� ������ null)
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                currentOnSkill = remoteBomb;
                if (remoteBomb != null)
                {
                    onSKillAction = remoteBomb.OnSkill;
                    useSkillAction = remoteBomb.UseSkill;
                    offSkillAction = remoteBomb.OffSkill;
                }

                break;
            case SkillName.RemoteBomb_Cube:
                currentOnSkill = remoteBombCube;
                if (remoteBombCube != null)
                {
                    onSKillAction = remoteBombCube.OnSkill;
                    useSkillAction = remoteBombCube.UseSkill;
                    offSkillAction = remoteBombCube.OffSkill;
                }
                break;
            case SkillName.MagnetCatch:
                currentOnSkill = magnetCatch;
                if (magnetCatch != null)
                {
                    onSKillAction = magnetCatch.OnSkill;
                    useSkillAction = magnetCatch.UseSkill;
                    offSkillAction = magnetCatch.OffSkill;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }

    }

    /// <summary>
    /// �� �� �ִ� ������Ʈ �ľ��ϴ� �޼���
    /// </summary>
    void PickUpObjectDetect()
    {
        if (!IsPickUp)      // �� ���̸�
        {
            Vector3 heightPoint = pickUpRoot.position;
            heightPoint.y += pickUpHeightRange;
            Collider[] hit = Physics.OverlapCapsule(pickUpRoot.position, heightPoint, liftRadius);  // �Ⱦ� ���� �ľ��ؼ� üũ�� ��

            for (int i = 0; i < hit.Length; i++)        // ���� ���� ��� ��ü ��
            {
                reaction = hit[i].transform.GetComponent<ReactionObject>();
                if (reaction != null && reaction.IsThrowable)   // �� �� �ִ� ù��° ������Ʈ�� ��� ����
                {
                    PickUpObject();
                    break;
                }
            }
        }
        else if (IsPickUp && reaction != null)      // �̹� ������ ��� �ִ� ���
        {
            bool onSkill = reaction is Skill;
            if (onSkill)                            // ��ų�̸�
            {
                switch (SelectSkill)
                {
                    case SkillName.RemoteBomb:      // ��������ź�� ����߸���
                    case SkillName.RemoteBomb_Cube:
                        IsPickUp = false;
                        reaction.Drop();
                        reaction = null;
                        break;
                }
            }
            else                                    // ��ų�� �ƴϸ� ��ü ����߸���
            {
                IsPickUp = false;
                reaction.Drop();
                reaction = null;
            }
        }
        // ��ȣ�ۿ� Ű ����� �� �ൿ �߼����� Ȯ���ϱ�
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

    void CancelSkill()
    {
        offSkillAction?.Invoke();
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                remoteBomb = null;
                currentOnSkill = null;
                break;
            case SkillName.RemoteBomb_Cube:
                remoteBombCube = null;
                currentOnSkill = null;
                break;
            case SkillName.MagnetCatch:
                magnetCatch = null;
                currentOnSkill = null;
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
    }

    private void OnTimeLock()
    {
        SelectSkill = SkillName.TimeLock;
    }

    private void OnIceMaker()
    {
        SelectSkill = SkillName.IceMaker;
    }

    private void OnMagnetCatch()
    {
        SelectSkill = SkillName.MagnetCatch;
    }

    private void OnRemoteBomb_Cube()
    {
        SelectSkill = SkillName.RemoteBomb_Cube;
    }

    private void OnRemoteBomb()
    {
        SelectSkill = SkillName.RemoteBomb;
    }
}
