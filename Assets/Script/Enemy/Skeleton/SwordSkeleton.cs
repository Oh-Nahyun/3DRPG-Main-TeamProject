using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkeleton : EnemyBase
{
    public Action<float> onDamage;

    readonly int die_Hash = Animator.StringToHash("Die");
    readonly int damage_Hash = Animator.StringToHash("Damage");

    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            // �״� �ִϸ��̼�
            animator.SetTrigger(die_Hash);
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            // �´� �ִϸ��̼�
            animator.SetTrigger(damage_Hash);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onDamage?.Invoke(damage);
        }
    }
}
