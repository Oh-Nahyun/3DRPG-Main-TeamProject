using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : RecycleObject
{
    SwordSkeleton skeleton;
    NightmareDragon nightmareDragon;
    

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>();   // �÷��̾� ã��
        nightmareDragon = GetComponentInParent<NightmareDragon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackPoint"))
        {
            // �� ����
            Player target = GameManager.Instance.Player;
            if (target != null)
            {
                if(nightmareDragon != null)
                {
                    target.Defence(nightmareDragon.AttackPower);
                }
                else
                {
                    target.Defence(skeleton.AttackPower);        // ���� ��󿡰� ������ ����
                }
            }
        }
        //else if (other.CompareTag("Player"))
        //{
        //    // ���� ����
        //    Player target = GameManager.Instance.Player;
        //    if (target != null)
        //    {
        //        target.Defence(skeleton.AttackPower);        // ���� ��󿡰� ������ ����
        //    }
        //}
    }

    
}
