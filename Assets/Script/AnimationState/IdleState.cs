using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    readonly int isPatrolling_Hash = Animator.StringToHash("IsPatrolling");
    readonly int isChasing_Hash = Animator.StringToHash("IsChasing");

    float timer;

    float startChasingRange = 8.0f;

    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0.0f;
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� �±׸� ���� ������Ʈ ã��
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 5) // �ð� ������ �ȴ� �ִϸ��̼� ����
        {
            animator.SetBool(isPatrolling_Hash, true);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position); // �ڽŰ� �÷��̾��� �Ÿ� ���ϱ�
        if(distance < startChasingRange) // �ڽŰ� �÷��̾��� �Ÿ��� �����Ÿ� �����̸�
        {
            animator.SetBool(isChasing_Hash, true); // �޸��� �ִϸ��̼� ����
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
