using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �÷��̾��� ������ �����ϴ� Ŭ����
/// </summary>
public class SaveHandler_Base : MonoBehaviour
{
    // ������Ʈ ================================================================
    CanvasGroup canvasGroup;

    /// <summary>
    /// ���̺� ������ ���Ե�
    /// </summary>
    protected SaveDataSlot[] saveSlots;

    /// <summary>
    /// ���̺� ������ ���� ���� ������Ƽ
    /// </summary>
    public SaveDataSlot[] SaveSlots => saveSlots;

    /// <summary>
    /// ���̺�, �ε� Ȯ��â
    /// </summary>
    SaveCheckUI saveCheckUI;

    // ���Ե����� ===============================================================
    /// <summary>
    /// �� ������
    /// </summary>
    protected int[] SceneDatas;

    /// <summary>
    /// �÷��̾� ������
    /// </summary>
    protected PlayerData[] playerDatas;

    protected Player player;

    /// <summary>
    /// ������ �ִ� ������
    /// </summary>
    const int DATA_SIZE = 5;

    /// <summary>
    /// ������ ���� Ŭ������ �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<int> onClickSaveSlot;

    /// <summary>
    /// ������ ������ Ŭ������ �� �����ϴ� ��������Ʈ
    /// </summary>
    public Action<int> onClickLoadSlot;

    protected virtual void Start()
    {
        SceneDatas = new int[DATA_SIZE];
        playerDatas = new PlayerData[DATA_SIZE];
        saveSlots = new SaveDataSlot[DATA_SIZE];

        Transform child = transform.GetChild(0); // slots
        for (int i = 0; i < saveSlots.Length; i++)
        {
            Transform slot = child.GetChild(i);
            saveSlots[i] = slot.GetComponent<SaveDataSlot>();
            saveSlots[i].InitializeComponent();
            SaveSlots[i].SlotInitialize(i);
        }

        RefreshSaveData();

        player = GameManager.Instance.Player;

        // Check UI =====================================
        child = transform.GetChild(1);
        saveCheckUI = child.GetComponent<SaveCheckUI>();

        saveCheckUI.onSave += SavePlayerData;
        saveCheckUI.onLoad += LoadPlayerData;

        onClickSaveSlot += saveCheckUI.ShowSaveCheck;
        onClickLoadSlot += saveCheckUI.ShowLoadCheck;
    }

    void OnEnable()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();        
    }

    void OnDestroy()
    {
        canvasGroup = null;
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
    /// ����� ������(Json����)�� �ҷ��ͼ� �����ϴ� �Լ� 
    /// </summary>
    void RefreshSaveData()
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
                SaveSlots[i].CheckSave(true, SceneDatas[i]);
            }
            else
            {
                SaveSlots[i].CheckSave(false, SceneDatas[i]);
            }
        }
    }

    /// <summary>
    /// �÷��̾� �����͸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="saveIndex">���̺��� ���� �ε���</param>
    protected virtual void SavePlayerData(int saveIndex)
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

        RefreshSaveData();
        Debug.Log("Player Data convert complete");
    }


    /// <summary>
    /// �÷��̾� ������ �ε�
    /// </summary>
    /// <param name="loadIndex">�ε��� ���� ��ȣ</param>
    /// <returns>�ε忡 ���������� true �ƴϸ� false</returns>
    protected virtual void LoadPlayerData(int loadIndex)
    {
        // ������ ������ �ҷ�����   
        GameManager.Instance.spawnPoint = playerDatas[loadIndex].position; // �÷��̾� ��ġ ���
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
            }
        }

        //CloseSavePanel();

        // �� �ҷ�����
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(SceneDatas[loadIndex])); // ������ �� �ε����� �� ����
        GameManager.Instance.ChangeToTargetScene(sceneName, GameManager.Instance.Player.gameObject);
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
        if (canvasGroup == null) Debug.Log($"�����ϴ� ķ������ NULL�Դϴ�");
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
