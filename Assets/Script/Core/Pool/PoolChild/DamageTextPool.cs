using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageTextPool : ObjectPool<DamageText>
{
    /// <summary>
    /// Ǯ���� ������� �ʴ� ������Ʈ�� �ϳ� ���� �� ���� �ϴ� �Լ�
    /// </summary>
    /// <param name="damage">���� ������</param>
    /// <param name="position">������ ��ġ</param>
    /// <returns>Ǯ���� ���� ������Ʈ(Ȱ��ȭ��)</returns>
    public GameObject GetObject(int damage, Vector3? position)
    {
        DamageText damageText = GetObject(position);
        damageText.SetDamage(damage);

        return damageText.gameObject;
    }
}
