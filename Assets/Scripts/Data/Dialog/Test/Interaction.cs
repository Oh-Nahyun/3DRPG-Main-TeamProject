using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    /// <summary>
    /// 감지 범위
    /// </summary>
    public float radius = 0f;
    /// <summary>
    /// 찾을 콜라이더의 레이어
    /// </summary>
    public LayerMask layer;
    /// <summary>
    /// 감지된 모든 콜라이더
    /// </summary>
    public Collider[] colliders;
    /// <summary>
    /// 가장 가까운 콜라이더
    /// </summary>
    public Collider short_enemy;
    /// <summary>
    /// 가장 가까운 오브젝트
    /// </summary>
    public GameObject scanIbgect;
    /// <summary>
    /// 상호작용 텍스트를 나타내는 UI
    /// </summary>
    TextInteraction textInteraction;

    private void Awake()
    {
        textInteraction = FindAnyObjectByType<TextInteraction>();
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
            short_enemy = colliders[0]; // 일단 첫 번째 요소를 가장 가까운 것으로 설정

            foreach (Collider col in colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    short_enemy = col; // 더 가까운 것을 찾으면 short_enemy 업데이트
                }
            }
            target(true); // colliders 배열이 비어있지 않은 경우 target 메서드 호출
        }
        else
        {
            short_enemy = null; // colliders가 없을 때 short_enemy 초기화
            target(false); // colliders 배열이 비어있는 경우 target 메서드 호출
        }
    }

    void target(bool t)
    {
        if (t)
        {
            // 최상위 부모 GameObject를 찾아서 scanIbgect에 할당
            scanIbgect = FindTopParentWithCollider(short_enemy.gameObject);
            if (scanIbgect != null)
            {
                if (scanIbgect.tag != null)
                {
                    textInteraction.SetTagText(scanIbgect);
                    textInteraction.TextActive(t);
                }
            }
        }
        else
        {
            scanIbgect = null;
            textInteraction.TextActive(t);
        }
    }

    // Collider를 가진 GameObject의 최상위 부모 GameObject를 반환하는 메서드
    GameObject FindTopParentWithCollider(GameObject childObject)
    {
        Transform parentTransform = childObject.transform.parent;

        if (parentTransform == null)
        {
            return childObject;
        }

        /*
        if (parentTransform.GetComponent<Collider>() != null)
        {
            return childObject;
        }*/

        if (parentTransform.gameObject.layer != layer)
        {
            return parentTransform.gameObject;
        }

        // 부모 GameObject의 부모 GameObject를 재귀적으로 검색하여 최상위 부모 GameObject를 반환
        return FindTopParentWithCollider(parentTransform.gameObject);
    }

    /// <summary>
    /// 탐지 범위를 보여주는 기즈모
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}