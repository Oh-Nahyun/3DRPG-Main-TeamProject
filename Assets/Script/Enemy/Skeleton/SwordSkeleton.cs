using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordSkeleton : EnemyBase
{
    // NavMeshAgent ������Ʈ
    private NavMeshAgent navMeshAgent;

    // Animator ������Ʈ
    private Animator animator;

    // �߰��� ���
    private Transform target;

    // ���� �������� ����
    private bool canAttack = true;

    // Idle ���� ���� �ð�
    public float idleDuration = 5f;

    // �ִϸ��̼� ���� �ؽ��ڵ�
    private readonly int isPatrolling_Hash = Animator.StringToHash("IsPatrolling");
    private readonly int isChasing_Hash = Animator.StringToHash("IsChasing");
    private readonly int isAttacking_Hash = Animator.StringToHash("IsAttacking");
    private readonly int isDamaged_Hash = Animator.StringToHash("IsDamaged");

    // SwordPoint ������Ʈ
    private GameObject swordPoint;

    // SwordPoint ������Ʈ�� �ݶ��̴�
    private Collider swordCollider;

    // onWeaponBladeEnabe ��������Ʈ ����
    public Action<bool> onWeaponBladeEnabe;

    // Waypoints�� ������ ����Ʈ
    private List<Transform> waypointsList = new List<Transform>();

    // �߰� ��Ÿ� ������ üũ�ϴ� �޼���
    private bool IsPlayerInRange()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= chaseRange;
        }
        return false;
    }

    // ���� �������� üũ�ϴ� �޼���
    private bool CanAttack()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= attackRange;
        }
        return false;
    }

    // ���� ���� �޼���
    private void Attack()
    {
        if (canAttack && CanAttack())
        {
            // ���� �ִϸ��̼� ����
            animator.SetBool(isAttacking_Hash, true);

            // ���� ���̵� Ȱ��ȭ
            WeaponBladeEnable();

            // ���� ��ٿ� ����
            canAttack = false;
            Invoke("ResetAttack", attackCooldown);

            // TODO: ���� ó��
        }
    }

    // ���� ��ٿ� ���� �޼���
    private void ResetAttack()
    {
        canAttack = true;
    }

    // �÷��̾� ���� �޼���
    private void ChasePlayer()
    {
        if (target != null)
        {
            // �÷��̾� ���� �� ����
            navMeshAgent.SetDestination(target.position);
            if (IsPlayerInRange())
            {
                // ���� ���·� ����
                animator.SetBool(isChasing_Hash, true);

                // ���� ���¿��� ���� ������ ��� ���� ����
                Attack();
            }
        }
    }

    // Awake �޼��带 ����Ͽ� �ʱ�ȭ
    void Awake()
    {
        // NavMeshAgent ������Ʈ ��������
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Animator ������Ʈ ��������
        animator = GetComponent<Animator>();

        // ü�� �ʱ�ȭ
        CurrentHealth = maxHealth;

        // ���� ��� ����
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // SwordPoint ������Ʈ ã��
        swordPoint = transform.Find("SwordPoint").gameObject;
        // SwordPoint ������Ʈ�� �ݶ��̴� ã��
        swordCollider = swordPoint.GetComponent<Collider>();

        // Waypoints�� ã�� ����Ʈ�� ����
        Transform waypointsParent = transform.GetChild(3); // 3��° �ڽ�
        foreach (Transform waypoint in waypointsParent)
        {
            waypointsList.Add(waypoint);
        }

        // Idle ���� ���� �� Walk �ִϸ��̼����� ����
        Invoke("StartPatrolling", idleDuration);
    }

    // Walk �ִϸ��̼����� �����Ͽ� ��ȸ ����
    private void StartPatrolling()
    {
        // Walk �ִϸ��̼� ����
        animator.SetBool(isPatrolling_Hash, true);
        navMeshAgent.speed = patrollingSpeed;
        // TODO: Waypoints�� ��ȸ�ϸ� �̵�
    }

    // Update �޼��带 ����Ͽ� �����Ӹ��� ����
    void Update()
    {
        // �÷��̾� ���� �� ����
        if (IsPlayerInRange())
        {
            // ���� ���·� ����
            ChasePlayer();
        }
        else
        {
            // �߰� ��Ÿ��� ����� Walk �ִϸ��̼����� �����Ͽ� ��ȸ ����
            animator.SetBool(isChasing_Hash, false);
            animator.SetBool(isPatrolling_Hash, true);
            navMeshAgent.speed = patrollingSpeed;
            // TODO: Waypoints�� ��ȸ�ϸ� �̵�
        }
    }

    // �÷��̾��� ������ �޾��� �� ȣ��Ǵ� �޼���
    public void TakeDamage(float damageAmount)
    {
        // ������ �޾��� �� ������ ����
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            // ü���� 0 ������ �� Die �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ
            Die();
        }
        else
        {
            // Damage �ִϸ��̼� ����
            animator.SetTrigger(isDamaged_Hash);
            // TODO: Damage �ִϸ��̼� ����
        }
    }

    // ���� ó�� �޼���
    private void Die()
    {
        // Die �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ
        animator.SetBool(isPatrolling_Hash, false);
        animator.SetBool(isChasing_Hash, false);
        animator.SetTrigger("Die");
        // TODO: Die �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ
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
}
