using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    readonly int isChasing_Hash = Animator.StringToHash("IsChasing");
    readonly int isAttacking_Hash = Animator.StringToHash("IsAttacking");

    float startAttackRange = 3.5f;
    float endChasingRange = 15.0f;

    NavMeshAgent agent;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� �±׸� ���� ������Ʈ ã��
        agent.speed = 3.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position); // �ڽŰ� �÷��̾��� �Ÿ� ���ϱ�
        if (distance > endChasingRange) // �ڽŰ� �÷��̾��� �Ÿ��� �����Ÿ� �̻��̸�
        {
            animator.SetBool(isChasing_Hash, false); // �޸��� �ִϸ��̼� ����
        }

        if (distance < startAttackRange) // �ڽŰ� �÷��̾��� �Ÿ��� startAttackRange �����̸�
        {
            animator.SetBool(isAttacking_Hash, true); // ���� �ִϸ��̼� ����
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
