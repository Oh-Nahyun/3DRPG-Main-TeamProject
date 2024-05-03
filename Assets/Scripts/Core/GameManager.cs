using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    Weapon weapon;
    public Weapon Weapon => weapon;

    CameraManager cameraManager;

    // ItemData
    ItemDataManager itemDataManager;

    /// <summary>
    /// ������ ������ Ŭ���� ������ ���� ������Ƽ
    /// </summary>
    public ItemDataManager ItemDataManager => itemDataManager;

    public CameraManager Cam
    {
        get
        {
            if (cameraManager == null)
                cameraManager = GetComponent<CameraManager>();
            return cameraManager;
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
    }

#if UNITY_EDITOR
    public Action onTalkNPC;
    public Action onNextTalk;
    public void StartTalk()
    {
        onTalkNPC?.Invoke();
        Debug.Log("��ȣ�ۿ� Ű ����");
    }

    public void NextTalk()
    {
        onNextTalk?.Invoke();
    }

#endif
}
