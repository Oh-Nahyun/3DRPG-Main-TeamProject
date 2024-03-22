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
        target(true); // ���� �� ���� ����
    }

    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
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
        }
        else
        {
            short_enemy = null; // colliders�� ���� �� short_enemy �ʱ�ȭ
        }

        target(short_enemy != null); // short_enemy�� null�� �ƴ� ���� ����
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
    /*
    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;
    public Collider short_enemy;

    public GameObject scanIbgect;

    void Start()
    {
        // isTarget ���� true�� �����Ͽ� target �޼��尡 ����ǵ��� ��
        target(true);
    }

    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            foreach (Collider col in colliders)
            {
                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);

                if (short_distance > short_distance2)
                {
                    short_distance = short_distance2;
                }
                short_enemy = col;
            }
        }
        else
        {
            // colliders �迭�� ��������� scanIbgect�� null�� ����
            scanIbgect = null;
        }

        // isTarget ���� colliders �迭�� ���̿� ���� ����
        target(colliders.Length > 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void target(bool t)
    {
        if (t && short_enemy != null)
        {
            scanIbgect = short_enemy.gameObject;
        }
        else
        {
            scanIbgect = null;
        }
    }
    public GameObject FindTopParentWithCollider(GameObject childObject)
    {
        // ���� GameObject�� null�̸� null ��ȯ
        if (childObject == null)
        {
            return null;
        }

        // ���� GameObject�� Collider�� ������ ���� GameObject�� ��ȯ
        if (childObject.GetComponent<Collider>() != null)
        {
            return childObject;
        }

        // ���� GameObject�� �θ� GameObject�� �˻�
        Transform parentTransform = childObject.transform.parent;

        // �θ� GameObject�� null�̸� ���� GameObject�� ��ȯ
        if (parentTransform == null)
        {
            return childObject;
        }

        // �θ� GameObject�� �θ� GameObject�� ��������� �˻��Ͽ� �ֻ��� �θ� GameObject�� ��ȯ
        return FindTopParentWithCollider(parentTransform.gameObject);
    }

}*/

   

