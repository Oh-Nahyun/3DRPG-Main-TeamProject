using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    /// <summary>
    /// Map Object ���̾ �ִ� ������Ʈ�� Material
    /// </summary>
    Renderer mapPointMaterial;

    /// <summary>
    /// MapPointMaterial�� ������ �����ϱ� ���� y��ǥ��
    /// </summary>
    float position_Y => transform.position.y;

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

    void Start()
    {
        Color mapColor = MapManager.Instance.SetColor(position_Y);

        mapPointMaterial.material.color = mapColor;
    }
}
