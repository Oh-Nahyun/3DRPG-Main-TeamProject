using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowVCam : MonoBehaviour
{
    /// <summary>
    /// ī�޶� ���� �ӵ�
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// ī�޶� Zoom ����
    /// </summary>
    readonly Vector3 zoomIn = new Vector3(0.25f, 0.0f, 2.0f);
    readonly Vector3 zoomOut = new Vector3(0.0f, 0.25f, -2.0f);

    // ������Ʈ��
    Weapon weapon;
    CinemachineVirtualCamera vcam;
    Cinemachine3rdPersonFollow follow;

    private void Awake()
    {
        weapon = FindAnyObjectByType<Weapon>();
        vcam = GetComponent<CinemachineVirtualCamera>();
        follow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Update()
    {
        ChangeCameraZoom();
    }

    void ChangeCameraZoom()
    {
        if (weapon.IsBowEquip && weapon.IsArrowEquip) // ĳ���Ͱ� Ȱ�� ����ϰ� �ְ� ȭ���� �����ϰ� �ִ� ���
        {
            ///// CanZoom�� true�� ����
            ///// ���콺 ���� ��ư�� ������ ������ �ǰ�, ZoomIn�� true�� �Ǿ���Ѵ�. (Ȱ������ ����.)
            ///// ���콺 ���� ��ư�� �����ٰ� ���� �ܾƿ��� �ǰ�, ZoomIn�� false�� �Ǿ���Ѵ�. (ȭ���� ���.)

            // �÷��̾ ���콺 ���� ��ư�� ������ �ִ� ���
            if (Input.GetMouseButtonDown(0))
            {
                // vcam.transform.rotation = Quaternion.LookRotation(player.transform.forward, Vector3.up); // Ȱ������ ��� ��, ĳ���Ϳ� ī�޶� ���� ���� �ٶ󺸱�
                StopAllCoroutines();
                StartCoroutine(Timer(true));
                weapon.IsZoomIn = true;
                weapon.LoadArrowAfter();
                Debug.Log("Camera Zoom-In");
            }

            // �÷��̾ ���콺 ���� ��ư���� �� ���
            if (Input.GetMouseButtonUp(0))
            {
                StopAllCoroutines();
                StartCoroutine(Timer(false));
                weapon.IsZoomIn = false;
                weapon.LoadArrowAfter();
                Debug.Log("Camera Zoom-Out");
            }
        }
        else
        {
            follow.ShoulderOffset = zoomOut;
            weapon.IsZoomIn = false;
        }
    }

    IEnumerator Timer(bool IsZoom)
    {
        float timeElapsed = 0.0f;
        while (true)
        {
            timeElapsed += speed * Time.deltaTime;

            if (IsZoom)
                follow.ShoulderOffset = Vector3.Lerp(zoomOut, zoomIn, timeElapsed);
            else
                follow.ShoulderOffset = Vector3.Lerp(zoomIn, zoomOut, timeElapsed);

            yield return null;
        }
    }
}