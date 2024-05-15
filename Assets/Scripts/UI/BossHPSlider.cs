using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSlider : MonoBehaviour
{
    public Slider hpSlider; // Inspector���� ������ �����̴�
    public Boss boss;       // Inspector���� ������ Boss ������Ʈ

    const float sliderReduceValue = 25f; // �����̴� ���� ��ġ

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

        if(hpSlider.value > boss.HP)
        {
            if(!boss.IsAlive) // ������ ��������� ü�¹� �����
            {
                gameObject.SetActive(false);
            }

            hpSlider.value -= Time.deltaTime * sliderReduceValue;
        }
    }

    /// <summary>
    /// ü�¹� �г� Ȱ��ȭ �Լ�
    /// </summary>
    public void ShowPanel()
    {
        canvasGroup.alpha = 1.0f;
    }
}