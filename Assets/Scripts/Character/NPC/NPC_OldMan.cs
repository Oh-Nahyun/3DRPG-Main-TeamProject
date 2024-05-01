using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_OldMan : NPCBase
{
    private bool isStanding = false;
    private bool isGesture = false;


    //readonly int Talk_Hash = Animator.StringToHash("IsTalk");
    readonly int Standing_Hash = Animator.StringToHash("IsStanding");
    readonly int Gesture_Hash = Animator.StringToHash("IsGesture");

    protected override void Awake()
    {
        base.Awake();
        isNPC = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        selectNextTalk();
        SetAnimation(); // �ִϸ��̼� ���� �޼��� ȣ��
        base.Update();
        SetAnimationBasedOnID(id);
    }



    void selectNextTalk()
    {
        if (id == 1000)
        {
            nextTaklSelect = true;
        }
        else if (id > 1010 && id < 1014)
        {
            nextTaklSelect = true;
        }
        else if (id == 1100)
        {
            nextTaklSelect = true;
        }
        else
        {
            nextTaklSelect = false; 
        }
    }

    void SetAnimation()
    {
        if (!isTalk)
        {
            isStanding = false;
            isGesture = false;
        }

        animator.SetBool(Talk_Hash, isTalk);
        animator.SetBool(Standing_Hash, isStanding);
        animator.SetBool(Gesture_Hash, isGesture);

    }

    void SetAnimationBasedOnID(int id)
    {
        string[] talkData = textBoxManager.GetTalkData(id);

        if (talkData != null && talkData.Length > 0)
        {
            switch (id)
            {
                case 1000:
                    isStanding = true;
                    isGesture = false;
                    break;
                case 1011:     
                    if (!isTalk)
                    {
                        questManager.GetQuestTalkIndex(10, false);
                        GameManager.Instance.NextTalk();
                    }
                    break;
                case 1012:
                case 1013:
                    // �������� ������ ������ �� �ְ� ����ó �ִϸ��̼��� ��Ȱ��ȭ.
                    isStanding = true;
                    isGesture = false;
                    break;
                case 1100:
                    // �������� ���� ��ȭ�� ������ �� �ְ� ����ó �ִϸ��̼��� ���.
                    isStanding = true;
                    isGesture = true;
                    break;
                default:
                    // ������ ��쿡�� ���ϴ� �ִϸ��̼��� ��Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ.
                    isStanding = false;
                    isGesture = false;
                    break;
            }
        }
        else
        {
            Debug.LogError("��ȭ �����͸� ã�� �� ���� ID: " + id);
        }
        SetAnimation(); // �ִϸ��̼� ���� �޼��� ȣ��
    }





}
