using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : NPCBase
{
    /// <summary>
    /// ����ġ�� ����
    /// </summary>
    enum State
    {
        Off = 0,    // ����ġ�� ���� ����
        On,         // ����ġ�� ���� ����
    }

    /// <summary>
    /// ����ġ�� ���� ����
    /// </summary>
    State state = State.Off;

    /// <summary>
    /// ����ġ�� ������ ���� ������ �ִ� ���� ������Ʈ
    /// </summary>
    public GameObject target;

    readonly int SwitchOnHash = Animator.StringToHash("SwitchOn");

    DoorSwitch targetDoor;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        if (target != null)
        {
            targetDoor = target.GetComponent<DoorSwitch>(); // target���� �� ã��
        }
        if (targetDoor == null)
        {
            Debug.LogWarning($"{gameObject.name}���� ����� ���� �����ϴ�.");  // ���� ������ ��� ���
        }
        isNPC = false;
        otherObject = true;
        id = 299;
    }

    /// <summary>
    /// ����ġ ���
    /// </summary>
    public void Use()
    {
        if (targetDoor != null)  // ������ ���� �־�� �Ѵ�.
        {
            targetDoor.isLock = false;
            switch (state)
            {
                case State.Off:
                    // ����ġ�� �Ѵ� ��Ȳ
                    targetDoor.OpenDoor();                  // ������
                    animator.SetBool(SwitchOnHash, true);   // ����ġ �ִϸ��̼� ���
                    state = State.On;                       // ���� ����
                    break;
                case State.On:
                    // ����ġ�� ������ ��Ȳ
                    targetDoor.OpenDoor();                  // �� �ݰ�
                    animator.SetBool(SwitchOnHash, false);  // ����ġ �ִϸ��̼� ���
                    state = State.Off;                      // ���� ����
                    break;
            }
        }
    }
}
