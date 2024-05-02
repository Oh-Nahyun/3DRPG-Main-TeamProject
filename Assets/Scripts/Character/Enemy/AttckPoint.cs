using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : RecycleObject
{
    SwordSkeleton skeleton;
    NightmareDragon NightmareDragon;

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>(true);   // �÷��̾� ã��
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

    ///// <summary>
    ///// ���� �ݶ��̴� �Ѵ� �Լ�
    ///// </summary>
    //private void WeaponBladeEnable()
    //{
    //    if (swordCollider != null)
    //    {
    //        swordCollider.enabled = true;
    //    }

    //    // onWeaponBladeEnabe �Ѷ�� ��ȣ������
    //    onWeaponBladeEnabe?.Invoke(true);
    //}

    ///// <summary>
    ///// ���� �ݶ��̴� ���� �Լ�
    ///// </summary>
    //private void WeaponBladeDisable()
    //{
    //    if (swordCollider != null)
    //    {
    //        swordCollider.enabled = false;
    //    }

    //    // onWeaponBladeEnabe ����� ��ȣ������
    //    onWeaponBladeEnabe?.Invoke(false);
    //}
}
