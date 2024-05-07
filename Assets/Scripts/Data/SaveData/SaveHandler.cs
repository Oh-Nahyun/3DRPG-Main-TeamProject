using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// �÷��̾��� ������ �����ϴ� Ŭ����
/// </summary>
public class SaveHandler : MonoBehaviour
{
    CanvasGroup canvasGroup;

    /// <summary>
    /// ���̺� ������ ���Ե�
    /// </summary>
    SaveDataSlot[] saveSlots;

    /// <summary>
    /// ���̺� ������ ���� ���� ������Ƽ
    /// </summary>
    public SaveDataSlot[] SaveSlots => saveSlots;

    /// <summary>
    /// �� ������
    /// </summary>
    int[] SceneDatas;

    /// <summary>
    /// �÷��̾� ������
    /// </summary>
    PlayerData[] playerDatas;

    Player player;

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

    // Delegates
    public Action<int> onClickSaveSlot;
    public Action<int> onClickLoadSlot;

    private void Start()
    {
        SceneDatas = new int[DATA_SIZE];
        playerDatas = new PlayerData[DATA_SIZE];
        saveSlots = new SaveDataSlot[DATA_SIZE];
        for(int i = 0; i < saveSlots.Length; i++)
        {
            Transform child = transform.GetChild(i);
            saveSlots[i] = child.GetComponent<SaveDataSlot>();
            SaveSlots[i].SlotInitialize(i);
        }

        onClickSaveSlot += SavePlayerData;
        onClickLoadSlot += LoadPlayerData;

        canvasGroup = GetComponent<CanvasGroup>();

        LoadJsonFile();
        player = GameManager.Instance.Player;
    }

    /// <summary>
    /// ���̺� �����͸� Default������ �ǵ����� �Լ�
    /// </summary>
    void SetDefaultData()
    {
        for (int i = 0; i < DATA_SIZE; i++)
        {
            SceneDatas[i] = 0;
            playerDatas[i] = new PlayerData(Vector3.zero, Vector3.zero, null);
        }
    }

    /// <summary>
    /// ����� ������(Json����)�� �ҷ����� �Լ� 
    /// </summary>
    void LoadJsonFile()
    {
        // Json ���� �ҷ�����
        string path = $"{Application.dataPath}/Save/";
        if (System.IO.Directory.Exists(path))   // Save �𷺷�Ƽ�� �����ϸ� 
        {
            string fullPath = $"{path}Save.json";
            if (System.IO.File.Exists(fullPath))    // json ������ �����ϸ� �ҷ�����
            {
                string json = System.IO.File.ReadAllText(fullPath);

                SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

                SceneDatas = loadedData.SceneNumber;
                playerDatas = loadedData.playerInfos;
            }
        }

        for(int i = 0; i < SaveSlots.Length; i++)
        {
            if (SceneDatas[i] == 0) // �� �����Ͱ� ���� == ���̺� �����Ͱ� �������� �ʴ´�.
            {
                SaveSlots[i].CheckSave(true);
            }
            else
            {
                SaveSlots[i].CheckSave(false);
            }
        }
    }

    /// <summary>
    /// �÷��̾� �����͸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="saveIndex">���̺��� ���� �ε���</param>
    void SavePlayerData(int saveIndex)
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
        if (!System.IO.Directory.Exists(path))
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
    void LoadPlayerData(int loadIndex)
    {
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

        CloseSavePanel();

        // �� �ҷ�����
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(SceneDatas[loadIndex])); // ������ �� �ε����� �� ����
        GameManager.Instance.ChangeToTargetScene(sceneName, player.gameObject);
    }

    /// <summary>
    /// �г��� �����ִ� �Լ�
    /// </summary>
    public void ShowSavePanel()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// �г��� ����� �Լ�
    /// </summary>
    public void CloseSavePanel()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
