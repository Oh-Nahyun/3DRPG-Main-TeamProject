using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Large Map�� ���� �׸� ������Ʈ�� ������ Ŭ����
/// </summary>
public class MapObject : MonoBehaviour
{
    /// <summary>
    /// Map Object ���̾ �ִ� ������Ʈ�� Material
    /// </summary>
    Renderer mapPointMaterial;

    /// <summary>
    /// MapPointMaterial�� ������ �����ϱ� ���� y��ǥ��
    /// </summary>
    float position_Y = 0f;

    bool isColored = false;

    /// <summary>
    /// �ʿ� ǥ���� ������Ʈ
    /// </summary>
    [Tooltip("Layer�� �ݵ�� Map Object�� ���־�� �Ѵ�.")]
    public GameObject mapObject;

    private void Awake()
    {
        if(mapObject == null)
        {
            Debug.LogWarning($"{gameObject.name}�� mapObject�� ����ֽ��ϴ�.");
        }
        else
        {
            mapPointMaterial = mapObject.GetComponent<Renderer>();
        }
    }

    private void Update()
    {
        Scan();
    }

    void Scan()
    {
        if (isColored) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            //Debug.Log($"{gameObject.name}.y : {hit.point}");
            Debug.DrawRay(transform.position, transform.forward * 1000f, Color.red);
            position_Y = hit.point.y;

            Color mapColor = MapManager.Instance.SetColor(position_Y);

            if (mapPointMaterial == null)
            {
                mapPointMaterial = mapObject.GetComponent<Renderer>();
            }

            mapPointMaterial.material.color = mapColor;
            isColored = true;
        }
    }
}
