using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Explosion : TestBase
{
    public GameObject target;

    Vector3 dir = Vector3.zero;

    public Transform exPos;

    Rigidbody rb;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        rb = target.GetComponent<Rigidbody>();
        dir = target.transform.position - exPos.position;

        rb.AddForce (dir * 5, ForceMode.Impulse);
    }
    // �����տ� �ݶ��̴� �ְ�(�ȶ�����������)
    // ������ �ٵ� Ű�׸�ƽ �ѱ�(�߷� �ޱ�����)
    // Darg�� ���� �ϱ� (�߷� ������ ����)
    // freeze rotation(�����ϱ�)
}
