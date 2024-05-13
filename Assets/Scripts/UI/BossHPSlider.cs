using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSlider : MonoBehaviour
{
    public Slider hpSlider; // Inspector���� ������ �����̴�
    public Boss boss;       // Inspector���� ������ Boss ������Ʈ

    CanvasGroup canvasGroup;

    void Start()
    {
        hpSlider = GetComponent<Slider>();
        boss = FindObjectOfType<Boss>();
        canvasGroup = GetComponent<CanvasGroup>();

        // ���� ���� �� �����̴� �ִ밪�� ����
        hpSlider.maxValue = boss.MaxHP;
        hpSlider.value = boss.HP;
    }

    void Update()
    {
        // �� �����Ӹ��� Boss�� HP�� �����̴��� �ݿ�
        hpSlider.value = boss.HP;
    }

    /// <summary>
    /// ü�¹� �г� Ȱ��ȭ �Լ�
    /// </summary>
    public void ShowPanel()
    {
        canvasGroup.alpha = 1.0f;
    }
}