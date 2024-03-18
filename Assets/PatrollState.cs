using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : StateMachineBehaviour
{
    readonly int isPatrolling_Hash = Animator.StringToHash("IsPatrolling");
    readonly int isChasing_Hash = Animator.StringToHash("IsChasing");

    float timer;

    float startChasingRange = 8.0f;

    List<Transform> wayPoints = new List<Transform>();

    NavMeshAgent agent;
    Transform player;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� �±׸� ���� ������Ʈ ã��
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = 1.5f;

        timer = 0.0f;
        GameObject go = GameObject.FindGameObjectWithTag("WayPoints");

        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t); // ��������Ʈ�� ����Ʈ�� ����
        }

        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position); // ���� ��������Ʈ ����
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position); // ���� ��������Ʈ�� ��������� �� ���� ��������Ʈ ����
        }

        timer += Time.deltaTime;
        if (timer > 10)
        {
            animator.SetBool(isPatrolling_Hash, false); // ���� �ð� �Ŀ� �ȴ� �ִϸ��̼� ����
        }
        float distance = Vector3.Distance(player.position, animator.transform.position); // �ڽŰ� �÷��̾� �±׸� ���� ������Ʈ ã��
        if (distance < startChasingRange)
        {
            // // �ڽŰ� �÷��̾��� �Ÿ��� �����Ÿ� �����̸�
            animator.SetBool(isChasing_Hash, true); // �޸��� �ִϸ��̼� ����
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position); // ��������Ʈ ����
    }

}
