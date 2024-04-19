using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    /// <summary>
    /// ���� ����
    /// </summary>
    public float radius = 0f;
    /// <summary>
    /// ã�� �ݶ��̴��� ���̾�
    /// </summary>
    public LayerMask layer;
    /// <summary>
    /// ������ ��� �ݶ��̴�
    /// </summary>
    public Collider[] colliders;
    /// <summary>
    /// ���� ����� �ݶ��̴�
    /// </summary>
    public Collider short_enemy;
    /// <summary>
    /// ���� ����� ������Ʈ
    /// </summary>
    public GameObject scanIbgect;
    /// <summary>
    /// ��ȣ�ۿ� �ؽ�Ʈ�� �����ִ� ������Ʈ UI
    /// </summary>
    Transform tagTextTransform;
    /// <summary>
    /// ��ȣ�ۿ� �ؽ�Ʈ
    /// </summary>
    TextMeshPro tagText;

    private void Awake()
    {
        tagText = GetComponentInChildren<TextMeshPro>();
        //tagTextTransform = transform.GetChild(0).transform;
        tagTextTransform = GetComponentInChildren<Transform>();
    }

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

    void target(bool t)
    {
        if (t)
        {
            // �ֻ��� �θ� GameObject�� ã�Ƽ� scanIbgect�� �Ҵ�
            scanIbgect = FindTopParentWithCollider(short_enemy.gameObject);
            if (scanIbgect != null)
            {
                if (scanIbgect.tag != null)
                {
                    setTagText(scanIbgect);
                    tagTextTransform.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            scanIbgect = null;
            tagTextTransform.gameObject.SetActive(false);
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

    /// <summary>
    /// ���� ����� ������Ʈ�� ��ȣ�ۿ� �ؽ�Ʈ�� ����ϴ� �Լ�
    /// </summary>
    /// <param name="obj">���� ����� ������Ʈ</param>
    private void setTagText(GameObject obj)
    {
        switch (obj.tag)
        {
            case "NPC" :
                tagText.SetText("���ϱ�");
                break;
            case "Item":
                tagText.SetText("�ݱ�");
                break;
            case "Chest":
                tagText.SetText("����");
                break;
            case "Warp":
                tagText.SetText("�̵�");
                break;
            default:
                tagText.SetText("");
                break;
        }
  
        

    }

    /// <summary>
    /// Ž�� ������ �����ִ� �����
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}