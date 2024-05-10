using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Boss : MonoBehaviour, IBattler, IHealth
{
    protected enum BossState
    {
        Wait = 0,
        Bite,
        Breath,
        ClowLR,
        ClowL,
        ClowR,
        FireBall,
        Move,
        GroundDodge,
        GroundDash,
        Dead
    }

    BossState state = BossState.Wait;

    protected BossState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case BossState.Wait:
                        isActive = false;
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        animator.SetTrigger("Idle");
                        break;
                    case BossState.Bite:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("Bite");
                        break;
                    case BossState.Breath:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("Breath");
                        break;
                    case BossState.ClowLR:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("ClowLR");
                        break;
                    case BossState.ClowL:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("ClowL");
                        break;
                    case BossState.ClowR:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("ClowR");
                        break;
                    case BossState.FireBall:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("FireBall");
                        break;
                    case BossState.Move:
                        isActive = false;
                        agent.isStopped = false;
                        StartCoroutine(MoveRandomDirection());
                        break;
                    case BossState.GroundDodge:
                        isActive = false;
                        agent.isStopped = false;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("GroundDodge");
                        break;
                    case BossState.GroundDash:
                        isActive = false;
                        agent.isStopped = false;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("GroundDash");
                        OnDash();
                        break;
                    case BossState.Dead:
                        isActive = false;
                        agent.isStopped = true;
                        break;
                }
            }
        }

    }

    public GameObject fireBallPrefab;
    BossInputActions inputActions;
    Rigidbody rb;
    ClowAttackArea clowAttackArea;
    BiteAttackArea biteAttackArea;
    Animator animator;
    ParticleSystem breathParticle;
    Player player;
    NavMeshAgent agent;
    Rigidbody rigid;
    Action onStateUpdate;

    Vector3 fireBallSpawnPosition;
    Vector3 difference;
    float sqrDistance;

    float moveDuration = 1.1f;
    public float speed = 2.0f;
    public float approachDuration = 1.2f;
    public float stopDistance = 1.5f;
    bool isActive = false;

    /// <summary>
    /// HP Ȯ�� �� ������ ������Ƽ
    /// </summary>
    protected float hp = 50.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(State != BossState.Dead)
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHealthChange?.Invoke(hp / MaxHP);
        }
    }

    /// <summary>
    /// �ִ� HPȮ�ο� ������Ƽ
    /// </summary>
    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// HP�� ����� ������ ����� ��������Ʈ(float:����)�� ������Ƽ
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// ������ Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public bool IsAlive => hp > 0;

    /// <summary>
    /// ��� ó���� �Լ�(�޼��� method)
    /// </summary>
    public void Die()
    {
        Debug.Log($"{gameObject.name} ���");
    }

    /// <summary>
    /// ����� �˸��� ���� ��������Ʈ�� ������Ƽ
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// ü���� ���������� �������� �ִ� �Լ�. �ʴ� totalRegen/duration ��ŭ ȸ��
    /// </summary>
    /// <param name="totalRegen">��ü ȸ����</param>
    /// <param name="duration">��ü ȸ���Ǵµ� �ɸ��� �ð�</param>
    public void HealthRegenerate(float totalRegen, float durationm)
    {

    }

    /// <summary>
    /// ü���� ƽ������ ȸ������ �ִ� �Լ�. 
    /// ��ü ȸ���� = tickRegen * totalTickCount. ��ü ȸ�� �ð� = tickInterval * totalTickCount
    /// </summary>
    /// <param name="tickRegen">ƽ �� ȸ����</param>
    /// <param name="tickInterval">ƽ ���� �ð� ����</param>
    /// <param name="totalTickCount">��ü ƽ ��</param>
    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {

    }

    /// <summary>
    /// ���ݷ� Ȯ�ο� ������Ƽ
    /// </summary>
    public float attackPower = 10.0f;
    public float AttackPower => attackPower;

    /// <summary>
    /// ���� Ȯ�ο� ������Ƽ
    /// </summary>
    public float defencePower => 3.0f;
    public float DefencePower => defencePower;

    /// <summary>
    /// �¾��� �� ����� ��������Ʈ(int:������ ���� ������)
    /// </summary>
    public Action<int> onHit { get; set; }

    /// <summary>
    /// �⺻ ���� �Լ�
    /// </summary>
    /// <param name="target">���� ������ ���</param>
    /// <param name="isWeakPoint">�������� �ƴ��� Ȯ�ο�(true�̸� ����, false�̸� �����ƴ�</param>
    public void Attack(IBattler target, bool isWeakPoint = false)
    {

    }

    /// <summary>
    /// �⺻ ��� �Լ�
    /// </summary>
    /// <param name="damage">���� ���� ������</param>
    public void Defence(float damage)
    {

    }

    IEnumerator MoveRandomDirection()
    {
        agent.isStopped = false;
        float endTime = Time.time + moveDuration;

        // ���� ���� ����
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        Vector3 direction = directions[UnityEngine.Random.Range(0, directions.Length)];

        if (direction == Vector3.forward)
        {
            animator.SetTrigger("WalkF");
        }
        else if (direction == Vector3.back)
        {
            animator.SetTrigger("WalkB");
        }
        else if (direction == Vector3.left)
        {
            animator.SetTrigger("WalkL");
        }
        else
        {
            animator.SetTrigger("WalkR");
        }

        while (Time.time < endTime)
        {
            transform.LookAt(player.transform.position);
            agent.Move(direction * Time.deltaTime * agent.speed);
            yield return null;
        }
        agent.isStopped = true;
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        clowAttackArea = GetComponentInChildren<ClowAttackArea>(true);
        biteAttackArea = GetComponentInChildren<BiteAttackArea>(true);
        breathParticle = GetComponentInChildren<ParticleSystem>(true);
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        animator.SetTrigger("Roar");
    }

    public void OnFireBall()
    {
        //Instantiate(fireBallPrefab, fireBallSpawnPosition, transform.rotation);
        Factory.Instance.GetFireBall(fireBallSpawnPosition);
    }

    public void OnClowArea()
    {
        if (clowAttackArea != null)
        {
            clowAttackArea.Activate();
        }
    }

    public void OffClowArea()
    {
        if (clowAttackArea != null)
        {
            clowAttackArea.Deactivate();
        }
    }

    public void OnBiteArea()
    {
        if (biteAttackArea != null)
        {
            biteAttackArea.Activate();
        }
    }

    public void OffBiteArea()
    {
        if (biteAttackArea != null)
        {
            biteAttackArea.Deactivate();
        }
    }

    public void OnBreathArea()
    {
        breathParticle.Play();
    }

    public void OffBreathArea()
    {
        breathParticle.Stop();
    }

    public void OnDash()
    {
        StartCoroutine(MoveTowardsPlayer());
    }

    public void OnActive()
    {
        isActive = true;
    }

    public void OnDodge()
    {
        animator.SetTrigger("GroundDodge");
    }

    IEnumerator MoveTowardsPlayer()
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        // �÷��̾� ���������� ���� ���
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // ���� ��ǥ ��ġ�� �÷��̾� ��ġ���� stopDistance��ŭ ������ ����
        Vector3 targetPosition = player.transform.position - directionToPlayer * stopDistance;

        while (Time.time < startTime + approachDuration)
        {
            float t = (Time.time - startTime) / approachDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition; // ��Ȯ�� ��ǥ ��ġ�� ����
    }

    private void Update()
    {
        fireBallSpawnPosition = transform.TransformPoint(new Vector3(-0.1f, 1.35f, 1.6f));
        difference = player.transform.position - transform.position;
        sqrDistance = difference.sqrMagnitude;

        if(isActive == true)
        {
            DecideStateBasedOnDistance();
        }
    }
    void DecideStateBasedOnDistance()
    {
        if (sqrDistance > 100) // �Ÿ��� 10 �̻� (10^2 = 100)
        {
            ChooseNextState(BossState.Move, BossState.GroundDash, BossState.FireBall);
        }
        else if (sqrDistance > 16 && sqrDistance <= 100) // �Ÿ��� 4���� 10 ���� (4^2 = 16, 10^2 = 100)
        {
            ChooseNextState(BossState.Move, BossState.GroundDash, BossState.Breath, BossState.FireBall, BossState.GroundDodge);
        }
        else // �Ÿ��� 4 �̸� (4^2 = 16)
        {
            ChooseNextState(BossState.ClowLR, BossState.ClowL, BossState.ClowR, BossState.Bite, BossState.GroundDodge);
        }
    }

    void ChooseNextState(params BossState[] states)
    {
        State = states[UnityEngine.Random.Range(0, states.Length)];
    }
}
