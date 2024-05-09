using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSlider : MonoBehaviour
{
    public Slider hpSlider; // Inspector���� ������ �����̴�
    public Boss boss;       // Inspector���� ������ Boss ������Ʈ

    void Start()
    {
        hpSlider = GetComponent<Slider>();
        boss = FindObjectOfType<Boss>();

        // ���� ���� �� �����̴� �ִ밪�� ����
        hpSlider.maxValue = boss.MaxHP;
        hpSlider.value = boss.HP;
    }

    void Update()
    {
        // �� �����Ӹ��� Boss�� HP�� �����̴��� �ݿ�
        hpSlider.value = boss.HP;
    }
}
