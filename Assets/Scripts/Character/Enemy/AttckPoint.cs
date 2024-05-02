using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : RecycleObject
{
    SwordSkeleton skeleton;
    NightmareDragon NightmareDragon;

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>();   // �÷��̾� ã��
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BodyPoint"))
        {
            // �� ����
            IBattler target = other.GetComponent<IBattler>();
            if (target != null)
            {
                skeleton.Attack(target, false);

            }
        }
        else if (other.CompareTag("WeakPoint"))
        {
            // ���� ����
            IBattler target = other.GetComponent<IBattler>();
            if (target != null)
            {
                skeleton.Attack(target, true);
            }
        }
    }
}
