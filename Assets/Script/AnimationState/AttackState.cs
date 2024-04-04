using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    float endChasingRange = 3.5f;


    readonly int isAttacking_Hash = Animator.StringToHash("IsAttacking");

    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� �±׸� ���� ������Ʈ ã��
        
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ��Ÿ� ��Ƽ� �Ÿ��� ��������� �÷��̾� �ٶ󺸰� ����
        float distance = Vector3.Distance(player.position, animator.transform.position); // �ڽŰ� �÷��̾��� �Ÿ� ���ϱ�
        if(distance > 0.1f)
        {
            animator.transform.LookAt(player);
        }
        if (distance > endChasingRange) // �ڽŰ� �÷��̾��� �Ÿ��� �����Ÿ� �̻��̸�
        {
            animator.SetBool(isAttacking_Hash, false); // �޸��� �ִϸ��̼� ����
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
