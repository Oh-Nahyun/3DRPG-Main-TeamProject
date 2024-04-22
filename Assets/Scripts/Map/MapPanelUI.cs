using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.PointerEventData;

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

    /// <summary>
    /// �� �� ������
    /// </summary>
    public GameObject mapPingPrefab;

    /// <summary>
    /// Mark ������Ʈ�� �������� UI ������Ʈ ������
    /// </summary>
    public GameObject highlightPingPrefab;

    /// <summary>
    /// Mark ������Ʈ�� �������� UI ������Ʈ
    /// </summary>
    GameObject highlightObject;

    Vector3 startDragVector = Vector3.zero;
    Vector3 onDragingVector = Vector3.zero;


    private void Awake()
    {
        // Map UI �ʱ�ȭ
        mapUI = GetComponentInChildren<LargeMapUI>();

        mapUI.onClick += OnClickInput;

        highlightObject = Instantiate(highlightPingPrefab, transform);
        highlightObject.SetActive(false);

        mapUI.onPointerInMark += OnCheckMark;
        mapUI.onPointerDragBegin += OnDragEnter;
        mapUI.onPointerDraging += OnDraging;
        mapUI.onPointerDragEnd += OnDragEnd;
    }

    private void OnDragEnd(Vector2 vector)
    {
        onDragingVector = new Vector3(vector.x, 0, vector.y);
        Vector3 result = startDragVector - onDragingVector;

        // ���������� �̵��� 
        //MapManager.Instance.MapCamera.transform.position += result;
    }

    private void OnDraging(Vector2 vector)
    {
        onDragingVector = new Vector3(vector.x, 0, vector.y);
        Vector3 result = startDragVector - onDragingVector;

        MapManager.Instance.MapCamera.transform.position += result * Time.deltaTime;
    }

    private void OnDragEnter(Vector2 vector)
    {
        startDragVector = new Vector3(vector.x, 0, vector.y);
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
    private void OnClickInput(InputButton button, Vector2 vector)
    {
        RaycastHit hit = GetObjectScreenToWorld(vector);
        Vector3 instantiateVector = hit.point;
        instantiateVector.y = 0;

        if(button == InputButton.Left)
        {
            Instantiate(mapPingPrefab, instantiateVector, Quaternion.identity);  // PointObject
        }
        else if(button == InputButton.Right)
        {
            MapPointMark mark = hit.transform.gameObject?.GetComponent<MapPointMark>(); // ���� ������Ʈ�� Mark ������Ʈ���� Ȯ��

            if (mark != null)
            {
                mark.ShowMarkInfo();
            }
        }
    }

    /// <summary>
    /// �� �ȿ��� Mark�� �����Ͱ� ������ ����Ǵ� �Լ�
    /// </summary>
    /// <param name="pointObject">���� ������Ʈ</param>
    private void OnCheckMark(Vector2 pointVector)
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
}