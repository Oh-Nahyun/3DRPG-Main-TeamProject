using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    SwordSkeleton skeleton;
    NightmareDragon nightmareDragon;

    BoxCollider attackCollider;
    

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>();   // �÷��̾� ã��
        nightmareDragon = GetComponentInParent<NightmareDragon>();
        attackCollider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// ���� �浹������ �Ѱ� ���� �Լ�
    /// </summary>
    /// <param name="isEnable"></param>
    public void BladeVolumeEnable(bool isEnable)
    {
        attackCollider.enabled = isEnable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackPoint"))
        {
            // �� ����
            IBattler target = other.GetComponent<IBattler>();
            if (target != null)
            {
                if(nightmareDragon != null)
                {
                    target.Defence(nightmareDragon.AttackPower);
                }
                else
                {
                    skeleton.Attack(target, false);        
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
