using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_01_Save : TestBase
{
    // 1. ��ư�� ������ ���� ���� ���� ���� ����
    int[] SceneDatas;
    //PlayerData[] playerDatas;
    List<PlayerData> playerDatas;

    Player player;

    const int DATA_SIZE = 5;

    public int saveIndex;

    private void Start()
    {
        SceneDatas = new int[DATA_SIZE];
        playerDatas = new List<PlayerData>(DATA_SIZE);

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


    // Save Scripts

    void SetDefaultData()
    {
        for(int i = 0; i < DATA_SIZE; i++)
        {
            SceneDatas[i] = 0;
            //playerDatas[i] = new PlayerData(Vector3.zero, Vector3.zero, null);
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
        Inventory curInven = player.PlayerInventory;

        data.playerInfos = new List<PlayerData>(DATA_SIZE);         // SaveData Ŭ������ LIst �ʱ�ȭ
        SetSaveData(saveIndex, curPos, curRot, curInven);       
        data.playerInfos.Insert(saveIndex, playerDatas[saveIndex]); // SaveData Ŭ������ ����

        // save Data file
        string jsonText = JsonUtility.ToJson(data); // json ���� ���ڿ��� ����
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

    bool LoadPlayerData()
    {
        return false;
    }

    void UpdateData()
    {

    }

    /// <summary>
    /// �÷��̾� �����͸� �����ϰ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="dataIndex">������ �ε���</param>
    /// <param name="pos">�÷��̾� ��ġ</param>
    /// <param name="rot">�÷��̾� ȸ���� ( ���Ϸ� )</param>
    /// <param name="inven">�÷��̾� �κ��丮</param>
    void SetSaveData(int dataIndex, Vector3 pos, Vector3 rot, Inventory inven)
    {
        PlayerData data = new PlayerData(pos, rot, inven);  // ������ �÷��̾� ������ ���� 
        playerDatas.Insert(dataIndex, data);                // ������ ����

        Debug.Log("�÷��̾� ������ ���� �Ϸ�");
    }
}