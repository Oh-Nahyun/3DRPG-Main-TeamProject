using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : RecycleObject
{
    Boss boss;
    float speed = 30.0f;

    private void Start()
    {
        boss = FindAnyObjectByType<Boss>();
        GameObject bossObject = boss.gameObject;

        if (boss != null)
        {
            transform.rotation = Quaternion.Euler(0f, bossObject.transform.eulerAngles.y,0f);
        }

    }

    private void FixedUpdate()
    {
        transform.Translate(speed * Vector3.forward * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("감지");
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            IBattler target = other.GetComponent<IBattler>();
            if (target != null)
            {
                Debug.Log("파이어볼 공격 맞음");
                boss.Attack(target, false);
            }
        }
    }
}
