using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;
    public Collider short_enemy;

    public GameObject scanIbgect;

    void Start()
    {
        target(false);
    }

    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders != null && colliders.Length > 0)
        {
            float shortestDistance = Vector3.Distance(transform.position, colliders[0].transform.position);
            short_enemy = colliders[0]; // �ϴ� ù ��° ��Ҹ� ���� ����� ������ ����

            foreach (Collider col in colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    short_enemy = col; // �� ����� ���� ã���� short_enemy ������Ʈ
                }
            }

            target(true); // colliders �迭�� ������� ���� ��� target �޼��� ȣ��
        }
        else
        {
            short_enemy = null; // colliders�� ���� �� short_enemy �ʱ�ȭ
            target(false); // colliders �迭�� ����ִ� ��� target �޼��� ȣ��
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void target(bool t)
    {
        if (t)
        {
            // �ֻ��� �θ� GameObject�� ã�Ƽ� scanIbgect�� �Ҵ�
            scanIbgect = FindTopParentWithCollider(short_enemy.gameObject);
        }
        else
        {
            scanIbgect = null;
        }
    }

    // Collider�� ���� GameObject�� �ֻ��� �θ� GameObject�� ��ȯ�ϴ� �޼���
    GameObject FindTopParentWithCollider(GameObject childObject)
    {
        Transform parentTransform = childObject.transform.parent;

        if (parentTransform == null)
        {
            return childObject;
        }

        // �θ� GameObject�� Collider�� ������ ���� GameObject�� ��ȯ
        if (parentTransform.GetComponent<Collider>() != null)
        {
            return childObject;
        }

        // �θ� GameObject�� �θ� GameObject�� ��������� �˻��Ͽ� �ֻ��� �θ� GameObject�� ��ȯ
        return FindTopParentWithCollider(parentTransform.gameObject);
    }
}
 

   

