using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SwordSkeleton : RecycleObject, IBattler, IHealth
{
    /// <summary>
    /// ���� ���� �� �ִ� ������ ����
    /// </summary>
    protected enum EnemyState
    {
        Wait = 0,   // ���
        Patrol,     // ����
        Chase,      // ����
        Attack,     // ����
        Dead        // ���
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    EnemyState state = EnemyState.Patrol;

    /// <summary>
    /// ���¸� �����ϰ� Ȯ���ϴ� ������Ƽ
    /// </summary>
    protected EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)  // ���¿� ������ �� �� �ϵ� ó��
                {
                    case EnemyState.Wait:
                        // ���� �ð� ���
                        agent.isStopped = true;         // agent ����
                        agent.velocity = Vector3.zero;  // agent�� �����ִ� ��� ����
                        animator.SetTrigger("Idle");    // �ִϸ��̼� ����
                        WaitTimer = waitTime;           // ��ٷ��� �ϴ� �ð� �ʱ�ȭ
                        onStateUpdate = Update_Wait;    // ��� ���¿� ������Ʈ �Լ� ����
                        break;
                    case EnemyState.Patrol:
                        // Debug.Log("��Ʈ�� ����");
                        agent.isStopped = false;        // agent �ٽ� �ѱ�
                        agent.speed = walkSpeed;
                        agent.SetDestination(waypoints.NextTarget);  // ������ ����(��������Ʈ ����)
                        animator.SetTrigger("Patrol");
                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        agent.isStopped = false;
                        agent.speed = runSpeed;
                        animator.SetTrigger("Chase");
                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        attackCoolTime = attackInterval;
                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        animator.SetTrigger("Die");
                        onStateUpdate = Update_Dead;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// ��� ���·� ���� �� ��ٸ��� �ð�
    /// </summary>
    public float waitTime = 1.0f;

    /// <summary>
    /// ��� �ð� ������(��� ����)
    /// </summary>
    float waitTimer = 1.0f;

    /// <summary>
    /// ������ �ð� ó���� ������Ƽ
    /// </summary>
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waitTimer < 0.0f)
            {
                State = EnemyState.Patrol;
            }
        }
    }

    /// <summary>
    /// �ȴ�(����) �ӵ�
    /// </summary>
    public float walkSpeed = 2.0f;

    /// <summary>
    /// �ٴ�(�߰�)�ӵ�
    /// </summary>
    public float runSpeed = 4.0f;

    /// <summary>
    /// ���� ������ ��������Ʈ(public������ privateó�� ����� ��)
    /// </summary>
    public Waypoints waypoints;

    /// <summary>
    /// ���Ÿ� �þ� ����
    /// </summary>
    public float farSightRange = 10.0f;

    /// <summary>
    /// ���Ÿ� �þ߰��� ����
    /// </summary>
    public float sightHalfAngle = 50.0f;

    /// <summary>
    /// �ٰŸ� �þ� ����
    /// </summary>
    public float nearSightRange = 1.5f;

    /// <summary>
    /// ���� ����� Ʈ������
    /// </summary>
    protected Transform chaseTarget = null;

    /// <summary>
    /// ���� ���
    /// </summary>
    protected IBattler attackTarget = null;

    /// <summary>
    /// ���ݷ�(������ �ν����Ϳ��� �����ϱ� ���� public���� ���� ����)
    /// </summary>
    public float attackPower = 10.0f;
    public float AttackPower => attackPower;

    /// <summary>
    /// ����(������ �ν����Ϳ��� �����ϱ� ���� public���� ���� ����)
    /// </summary>
    public float defencePower = 3.0f;
    public float DefencePower => defencePower;

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    public float attackInterval = 1.0f;

    /// <summary>
    /// �����ִ� ���� ��Ÿ��
    /// </summary>
    float attackCoolTime = 0.0f;

    /// <summary>
    /// HP
    /// </summary>
    protected float hp = 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if (State != EnemyState.Dead && hp <= 0)    // �ѹ��� �ױ�뵵
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHealthChange?.Invoke(hp / MaxHP);
        }
    }

    /// <summary>
    /// �ִ� HP(������ �ν����Ϳ��� �����ϱ� ���� public���� ���� ����)
    /// </summary>
    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// HP ����� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// ��Ҵ��� �׾����� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public bool IsAlive => hp > 0;

    /// <summary>
    /// �� ĳ���Ͱ� �׾��� �� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// �� ĳ���Ͱ� �¾����� ����Ǵ� ��������Ʈ(int : ������ ���� ������)
    /// </summary>
    public Action<int> onHit { get; set; }

    // onWeaponBladeEnabe ��������Ʈ ����
    public Action<bool> onWeaponBladeEnabe;

    /// <summary>
    /// ���º� ������Ʈ �Լ��� ����� ��������Ʈ(�Լ� �����)
    /// </summary>
    Action onStateUpdate;

    // ������Ʈ��
    Animator animator;
    NavMeshAgent agent;
    // ���� �ؾߵ�(���κ� �Ӹ��κ� ���� �и�)
    CapsuleCollider bodyCollider;   // ���� �κ� �ݶ��̴�
    SphereCollider headCollider;    // �Ӹ� �κ� �ݶ��̴�
    BoxCollider swordCollider;      // ���� �κ� �ݶ��̴�

    Rigidbody rigid;
    EnemyHealthBar hpBar;           // ���� HP��

    // �б� ����
    readonly Vector3 EffectResetPosition = new(0.0f, 0.01f, 0.0f);

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();

        GameObject bodyPoint = GameObject.Find("BodyPoint").gameObject;
        bodyCollider = bodyPoint.GetComponent<CapsuleCollider>();

        GameObject headPoint = GameObject.Find("HeadPoint").gameObject;
        headCollider = headPoint.GetComponent<SphereCollider>();

        GameObject swordPoint = GameObject.Find("SwordPoint").gameObject;
        swordCollider = swordPoint.GetComponent<BoxCollider>();


        Transform child = transform.GetChild(3);
        hpBar = child.GetComponent<EnemyHealthBar>();

        child = transform.GetChild(4);
        AttackArea attackArea = child.GetComponent<AttackArea>();

        
        attackArea.onPlayerIn += (target) =>
        {
            // �÷��̾ ���� ���¿���
            if (State == EnemyState.Chase)   // ���� �����̸�
            {
                attackTarget = target;      // ���� ��� �����ϰ�
                State = EnemyState.Attack;  // ���� ���·� ��ȯ
            }
        };
        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target)            // ���� ����� ��������
            {
                attackTarget = null;                // ���� ����� ����
                if (State != EnemyState.Dead)        // ���� �ʾҴٸ�
                {
                    State = EnemyState.Chase;       // ���� ���¸� �ǵ�����
                }
            }
        };
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        agent.speed = walkSpeed;            // �̵� �ӵ� ����
        State = EnemyState.Wait;            // �⺻ ���� ����
        animator.ResetTrigger("Idle");      // Wait ���·� �����ϸ鼭 Stop Ʈ���Ű� ���� ���� �����ϱ� ���� �ʿ�
        rigid.isKinematic = true;           // Ű�׸�ƽ�� ���� ������ ����ǰ� �����
        rigid.drag = Mathf.Infinity;        // ���Ѵ�� �Ǿ� �ִ� �������� ���缭 ������ �� �ְ� �ϱ�
        HP = maxHP;                         // HP �ִ��
    }

    protected override void OnDisable()
    {
        bodyCollider.enabled = true;        // �ö��̴� Ȱ��ȭ
        hpBar.gameObject.SetActive(true);   // HP�� �ٽ� ���̰� �����
        agent.enabled = true;               // agent�� Ȱ��ȭ �Ǿ� ������ �׻� �׺�޽� ���� ����

        base.OnDisable();
    }

    void Update()
    {
        onStateUpdate();
    }

    /// <summary>
    /// Wait ���¿� ������Ʈ �Լ�
    /// </summary>
    void Update_Wait()
    {
        if (SearchPlayer())
        {
            State = EnemyState.Chase;
        }
        else
        {
            WaitTimer -= Time.deltaTime;    // ��ٸ��� �ð� ����(0�̵Ǹ� Patrol�� ����)

            // ���� �������� �ٶ󺸰� �����
            Quaternion look = Quaternion.LookRotation(waypoints.NextTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 2);
        }
    }

    /// <summary>
    /// Patrol ���¿� ������Ʈ �Լ�
    /// </summary>
    void Update_Patrol()
    {
        if (SearchPlayer())
        {
            State = EnemyState.Chase;
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance) // �����ϸ�
            {
                waypoints.StepNextWaypoint();   // ��������Ʈ�� ���� ������ �����ϵ��� ����
                State = EnemyState.Wait;        // ��� ���·� ��ȯ
            }
        }
    }

    void Update_Chase()
    {
        if (SearchPlayer())
        {
            agent.SetDestination(chaseTarget.position);
        }
        else
        {
            State = EnemyState.Wait;
        }
    }

    void Update_Attack()
    {
        attackCoolTime -= Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f);
        if (attackCoolTime < 0)
        {
            Attack(attackTarget);
        }
    }

    void Update_Dead()
    {
    }

    /// <summary>
    /// �þ� �����ȿ� �÷��̾ �ִ��� ������ ã�� �Լ�
    /// </summary>
    /// <returns>ã������ true, ��ã������ false</returns>
    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        // ���� �ݰ�(=farSightRange)�ȿ� �ִ� �÷��̾� ���̾ �ִ� ������Ʈ ���� ã��
        Collider[] colliders = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            // ���� �ݰ�(=farSightRange)�ȿ� �÷��̾ �ִ�.
            Vector3 playerPos = colliders[0].transform.position;    // 0���� ������ �÷��̾��(�÷��̾�� 1���̴ϱ�)
            Vector3 toPlayerDir = playerPos - transform.position;   // ��->�÷��̾�� ���� ���� ����
            if (toPlayerDir.sqrMagnitude < nearSightRange * nearSightRange)  // �÷��̾�� nearSightRange���� ���ʿ� �ִ�.
            {
                // ��������(=nearSightRange) �����̴�.
                chaseTarget = colliders[0].transform;
                result = true;
            }
            else
            {
                // �������� ���̴� => �þ߰� Ȯ��
                if (IsInSightAngle(toPlayerDir))     // �þ߰� ������ Ȯ��
                {
                    if (IsSightClear(toPlayerDir))   // ���� �÷��̾� ���̿� �þ߸� ������ ������Ʈ�� �ִ��� Ȯ��
                    {
                        chaseTarget = colliders[0].transform;
                        result = true;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// �þ߰�(-sightHalfAngle ~ +sightHalfAngle)�ȿ� �÷��̾ �ִ��� ������ Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="toTargetDirection">������ ������� ���ϴ� ���� ����</param>
    /// <returns>�þ߰� �ȿ� ������ true, ������ false</returns>
    bool IsInSightAngle(Vector3 toTargetDirection)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDirection);  // ���� ������� ���� �ٶ󺸴� ������� ������ ���� ����
        return sightHalfAngle > angle;
    }

    /// <summary>
    /// ���� �ٸ� ������Ʈ�� ���� ���������� �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="toTargetDirection">������ ������� ���ϴ� ���� ����</param>
    /// <returns>true�� �������� �ʴ´�. false�� ��������.</returns>
    bool IsSightClear(Vector3 toTargetDirection)
    {
        bool result = false;
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDirection); // ���� ����(�� ���� ������ ���� ����)
        if (Physics.Raycast(ray, out RaycastHit hitInfo, farSightRange))
        {
            if (hitInfo.collider.CompareTag("Player"))   // ó�� �浹�� ���� �÷��̾���
            {
                result = true;                          // �߰��� ������ ��ü�� ���ٴ� �Ҹ�
            }
        }

        return result;
    }

    /// <summary>
    /// ����ó���� �Լ�
    /// </summary>
    /// <param name="target">���� ���</param>
    public void Attack(IBattler target)
    {
        animator.SetTrigger("Attack");      // �ִϸ��̼� ����
        target.Defence(AttackPower);        // ���� ��쿡�� ������ ����
        attackCoolTime = attackInterval;    // ��Ÿ�� �ʱ�ȭ
    }

    /// <summary>
    /// ��� ó���� �� �Լ�
    /// </summary>
    /// <param name="damage">���� �ޤ��� ���� �����</param>
    public void Defence(float damage)
    {
        if (IsAlive) // ������� ���� ���̤Ӹ� ����
        {
            animator.SetTrigger("Hit");                 // �ִϸ��̼� ����

            float final = Mathf.Max(0, damage - DefencePower);  // ���� ������ ����ؼ�
            HP -= final;
            onHit?.Invoke(Mathf.RoundToInt(final));
            //Debug.Log($"���� �¾Ҵ�. ���� HP = {HP}");     
        }
    }

    /// <summary>
    /// ��� ó���� �Լ�
    /// </summary>
    public void Die()
    {
        //Debug.Log("���");
        State = EnemyState.Dead;        // 
        StartCoroutine(DeadSquence());  // ��� ���� ����
        onDie?.Invoke();                // �׾��ٰ� �˸� ������
    }

    /// <summary>
    /// ��� ����� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DeadSquence()
    {
        // �ö��̴� ��Ȱ��ȭ
        bodyCollider.enabled = false;

        // HP�� �Ⱥ��̰� �����
        hpBar.gameObject.SetActive(false);

        // ��� �ִϸ��̼� ���������� ���
        yield return new WaitForSeconds(2.5f);  // ��� �ִϸ��̼� �ð�(2.167��) -> 2.5�ʷ� ó��

        // �ٴ����� ���� �ɱ� ����
        agent.enabled = false;                  // agent�� Ȱ��ȭ �Ǿ� ������ �׻� �׺�޽� ���� ����
        rigid.isKinematic = false;              // Ű�׸�ƽ�� ���� ������ ����ǰ� �����
        rigid.drag = 10.0f;                     // ���Ѵ�� �Ǿ� �ִ� �������� ���缭 ������ �� �ְ� �ϱ�

        // ����� �ٴھƷ��� ������������ ���
        yield return new WaitForSeconds(2.0f);  // 2�ʸ� �� ������ ���̴�.

        // �� Ǯ�� �ǵ�����
        gameObject.SetActive(false);    // ��� �� Ǯ�� �ǵ�����
    }

    // ���� ���̵� Ȱ��ȭ �޼���
    private void WeaponBladeEnable()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }

        // onWeaponBladeEnabe ��������Ʈ ȣ��
        onWeaponBladeEnabe?.Invoke(true);
    }

    // ���� ���̵� ��Ȱ��ȭ �޼���
    private void WeaponBladeDisable()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }

        // onWeaponBladeEnabe ��������Ʈ ȣ��
        onWeaponBladeEnabe?.Invoke(false);
    }

    public void HealthRegenerate(float totalRegen, float duration)
    {
        
    }

    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        
    }

#if UNITY_EDITOR


    private void OnDrawGizmos()
    {
        bool playerShow = SearchPlayer();
        Handles.color = playerShow ? Color.red : Color.green;

        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f); // �߽ɼ� �׸���

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);            // �߽ɼ� ȸ����Ű��
        Handles.DrawLine(transform.position, transform.position + q1 * forward);        // �� �߱�

        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);
        Handles.DrawLine(transform.position, transform.position + q2 * forward);

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2, farSightRange, 2.0f);   // ȣ �׸���

        Handles.DrawWireDisc(transform.position, transform.up, nearSightRange);         // �ٰŸ� ���� �׸���
    }
    // �÷��̾� �߰ݽ� �����Ͼ
    // �ִϸ����� Ʈ���� ���� �ٲٱ�(��Ȳ�� �˸°�)
    // ���� ���¿� �߰� �����϶� �̵��ӵ� �ٲٱ�
    // ���� �Ӹ� �κ� �ݶ��̴� ������ ������ �ٸ��� �ޱ�
#endif
}

