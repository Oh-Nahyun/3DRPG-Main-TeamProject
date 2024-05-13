using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_01_Save : TestBase
{
    // 1. ��ư�� ������ ���� ���� ���� ���� ����
    // 2. 3����ư�� ������ ���� ����� ���� Load
    // 2.1 �κ��丮�� 1ĭ�ΰ� Emtpy ����
    int[] SceneDatas;
    PlayerData[] playerDatas;

    public Player player;

    /// <summary>
    /// ������ �ִ� ������
    /// </summary>
    const int DATA_SIZE = 5;

    /// <summary>
    /// ������ ���̺� ĭ �ε��� ��ȣ
    /// </summary>
    public int saveIndex = 0;

    /// <summary>
    /// �ε��� ���̺� ĭ �ε�����ȣ
    /// </summary>
    public int loadIndex = 0;

    public Transform traget;

    private void Start()
    {
        SceneDatas = new int[DATA_SIZE];
        playerDatas = new PlayerData[DATA_SIZE];

        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        SetDefaultData();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        SavePlayerData();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        LoadPlayerData(loadIndex);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[4]);
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[8]);
        Factory.Instance.GetItemObjects(GameManager.Instance.ItemDataManager[9], 3, traget.position, true);
    }

    // Save Scripts

    void SetDefaultData()
    {
        for(int i = 0; i < DATA_SIZE; i++)
        {
            SceneDatas[i] = 0;
            playerDatas[i] = new PlayerData(Vector3.zero, Vector3.zero, null);
        }
    }

    void SavePlayerData()
    {
        SaveData data = new SaveData(); // ����� Ŭ���� �ν��Ͻ� ����
        // ����� ��ü�� ������ ����
        // Scene ��ȣ ����
        SceneDatas[saveIndex] = SceneManager.GetActiveScene().buildIndex;
        data.SceneNumber = SceneDatas;

        // Player ���� ����
        Vector3 curPos = player.gameObject.transform.position;
        Vector3 curRot = player.gameObject.transform.eulerAngles;
        Inventory curInven = player.Inventory;

        //data.playerInfos = new List<PlayerData>[DATA_SIZE]; // ������ ������ �ʱ�ȭ
        data.playerInfos = new PlayerData[DATA_SIZE]; // ������ ������ �ʱ�ȭ
        PlayerData playerData = new PlayerData(curPos, curRot, curInven); // ������ �����Ͱ�
        playerDatas[saveIndex] = playerData;    // �÷��̾� �����Ͱ� ����

        // ����� Ŭ���� �ν��Ͻ��� ���� ����� �� ����
        for (int i = 0; i < DATA_SIZE; i++)
        {
            data.playerInfos[i] = playerDatas[i];
        }

        //data.playerInfos[saveIndex].Insert(saveIndex, playerDatas[saveIndex]); // SaveData Ŭ������ ����

        // save Data file
        string jsonText = JsonUtility.ToJson(data, true); // json ���� ���ڿ��� ����
        string path = $"{Application.dataPath}/Save/";
        if(!System.IO.Directory.Exists(path))
        {
            // path ������ ����
            System.IO.Directory.CreateDirectory(path); // ���� ����
        }

        string fullPath = $"{path}Save.json";               // ���� ��� �����
        System.IO.File.WriteAllText(fullPath, jsonText);    // ���Ϸ� ����

        Debug.Log("Player Data convert complete");
    }


    /// <summary>
    /// �÷��̾� ������ �ε�
    /// </summary>
    /// <param name="loadIndex">�ε��� ���� ��ȣ</param>
    /// <returns>�ε忡 ���������� true �ƴϸ� false</returns>
    bool LoadPlayerData(int loadIndex)
    {
        bool result = false;

        // Json ���� �ҷ�����
        string path = $"{Application.dataPath}/Save/";
        if(System.IO.Directory.Exists(path))
        {
            string fullPath = $"{path}Save.json";
            if(System.IO.File.Exists(fullPath))
            {
                string json = System.IO.File.ReadAllText(fullPath);

                SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

                SceneDatas = loadedData.SceneNumber;
                playerDatas = loadedData.playerInfos;

                result = true;
            }
        }

        // ������ ������ �ҷ�����   
        player.transform.position = playerDatas[loadIndex].position;                // �÷��̾� ��ġ ���
        player.transform.rotation = Quaternion.Euler(playerDatas[loadIndex].rotation);

        Inventory inventory = player.Inventory; // ������ �÷��̾� �κ��丮 �ҷ�����
        for (int i = 0; i < inventory.SlotSize; i++)
        {
            if (playerDatas[loadIndex].itemDataClass[i].count == 0) // ������ ������ ������ ����
            {
                continue;
            }
            else // �������� �����ϸ� ������ �߰�
            {
                uint itemCode = (uint)playerDatas[loadIndex].itemDataClass[i].itemCode; // ������ �ڵ�
                int itemCount = playerDatas[loadIndex].itemDataClass[i].count;            // ������ ����

                player.Inventory.AddSlotItem(itemCode, itemCount, (uint)i);
                //inventory[(uint)i].AssignItem(itemCode, itemCount, out int over);
            }
        }

        // �� �ҷ�����
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(SceneDatas[loadIndex])); // ������ �� �ε����� �� ����
        GameManager.Instance.ChangeToTargetScene(sceneName, player.gameObject);

        return result;
    }
}