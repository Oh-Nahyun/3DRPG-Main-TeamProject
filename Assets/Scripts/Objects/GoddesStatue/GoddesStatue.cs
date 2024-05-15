using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoddesStatue : MonoBehaviour
{

    /// <summary>
    /// ƽ�� ȸ���� �� ( ĳ���� �ִ� ä�� )
    /// </summary>
    public float tickRegen = 10f;

    /// <summary>
    /// ƽ ���͹�
    /// </summary>
    public float inverval = 0.2f;

    /// <summary>
    /// ȸ�� ƽ ����
    /// </summary>
    public uint tickCount = 100;

    private void Start()
    {
        tickRegen = GameManager.Instance.Player.MaxHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            IHealth health = GameManager.Instance.Player as IHealth;
            if (health != null)
            {
                tickRegen = GameManager.Instance.Player.MaxHP;
                health.HealthRegenerateByTick(tickRegen * 0.1f, inverval, tickCount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tickRegen = 0;
        }
    }



}
