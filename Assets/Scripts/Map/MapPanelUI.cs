using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Map �г� UI�� �����ϴ� Ŭ����
/// </summary>
public class MapPanelUI : MonoBehaviour
{
    /// <summary>
    /// Map�� ���� ī�޶�
    /// </summary>
    public Camera mapCamera;

    LargeMapUI mapUI;

    public GameObject mapPingPrefab;
    public GameObject highlightPingPrefab;

    GameObject highlightObject;


    private void Awake()
    {
        mapUI = GetComponentInChildren<LargeMapUI>();

        mapUI.onClick += OnClickInput;

        //
        highlightObject = Instantiate(highlightPingPrefab, transform);
        highlightObject.SetActive(false);

        mapUI.onPointerInMark += CheckMark;
        mapUI.onPointerExitMark += ExitMark;
    }

    /// <summary>
    /// ��ũ������ ���� ������Ʈ�� ������ ���ϴ� �Լ�
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private RaycastHit GetObjectScreenToWorld(Vector3 vector)
    {
        Ray ray = mapCamera.ScreenPointToRay(vector);   // ray
        RaycastHit hit;                                 // rayHit ����

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Map Object"))) // Map Object Ž��
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5f);
        }

        return hit;
    }

    /// <summary>
    /// �ʿ� Ŭ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="vector"></param>
    private void OnClickInput(Vector2 vector)
    {
        RaycastHit hit = GetObjectScreenToWorld(vector);
        Vector3 instantiateVector = hit.point;
        instantiateVector.y = 0;
        Instantiate(mapPingPrefab, instantiateVector, Quaternion.identity);  // PointObject
    }

    /// <summary>
    /// �� �ȿ��� Mark�� �����Ͱ� ������ ����Ǵ� �Լ�
    /// </summary>
    /// <param name="pointObject">���� ������Ʈ</param>
    private void CheckMark(Vector2 pointVector)
    {
        RaycastHit hit = GetObjectScreenToWorld(pointVector);

        GameObject pointObject = hit.transform.gameObject;

        MapPointMark mark = hit.transform.gameObject?.GetComponent<MapPointMark>(); // ���� ������Ʈ�� Mark ������Ʈ���� Ȯ��
        if (mark != null)
        {
            highlightObject.SetActive(true);
            highlightObject.transform.localPosition = pointObject.transform.position;
            Debug.Log("Map Mark ����");
        }
        else
        {
            highlightObject.SetActive(false);
        }
    }

    private void ExitMark(Vector2 pointVector)
    {

    }
}