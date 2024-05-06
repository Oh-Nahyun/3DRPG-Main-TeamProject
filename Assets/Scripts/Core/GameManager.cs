using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = FindAnyObjectByType<Player>();
            }
            return player;
        }
    }

    Weapon weapon;
    public Weapon Weapon => weapon;

    CameraManager cameraManager;

    // ItemData
    ItemDataManager itemDataManager;

    /// <summary>
    /// ������ ������ Ŭ���� ������ �ϱ����� ������Ƽ
    /// </summary>
    public ItemDataManager ItemDataManager => itemDataManager;

    MapManager mapManager;

    /// <summary>
    /// mapManager ������ ���� ������Ƽ
    /// </summary>
    public MapManager MapManager => mapManager;

    public CameraManager Cam
    {
        get
        {
            if (cameraManager == null)
                cameraManager = GetComponent<CameraManager>();
            return cameraManager;
        }
    }

    /// <summary>
    /// �ε� ������ Ȯ���ϴ� bool��
    /// </summary>
    public bool isLoading;

    /// <summary>
    /// �̵��� ���� �̸�
    /// </summary>
    string targetSceneName = null;

    /// <summary>
    /// �̵��� ���� �̸��� ���� �� �����ϱ� ���� ������Ƽ ( �̸��� �ٲ�� �ش� ���� TragetScene�� �ǰ� �ε����� ȣ���Ѵ�. )
    /// </summary>
    public string TargetSceneName
    {
        get => targetSceneName;
        set
        {
            if(targetSceneName != value)
            {
                targetSceneName = value;
                ChangeToLoadingScene();
            }
        }
    }

    /// <summary>
    /// �÷��̾� ������Ʈ�� ������ �ı� �Ұ����� ������Ʈ ( �� �̵��� )
    /// </summary>
    public GameObject loadPlayerGameObject;

    /// <summary>
    /// �÷��̾� ����� �κ��丮 Ŭ����
    /// </summary>
    Inventory savedInventory;

    InventorySlot[] savedEquipParts;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        loadPlayerGameObject = new GameObject();
        DontDestroyOnLoad(loadPlayerGameObject);
    }

    protected override void OnInitialize()
    {
        if (isLoading)
            return;

        if(player == null) player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();

        itemDataManager.InitializeItemDataUI();

        mapManager.InitalizeMapUI();

        if (savedInventory != null)
        {
            ItemDataManager.InventoryUI.InitializeInventoryUI(savedInventory);  // �÷��̾� �κ��丮 UI �ʱ�ȭ
            savedInventory = null;                                              // ������ �κ��丮 ������ ����
            player.Inventory.SetOwner(player.gameObject);                       // �κ��丮 ���ʰ� �ʱ�ȭ
            player.EquipPart = savedEquipParts;                                 // �������� ���� ����
            savedEquipParts = null;                                             // �������� ���� ����
        }
    }

    protected override void OnAdditiveInitiallize()
    {
        SpawnPlayerAfterLoadScene();

        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();

        itemDataManager.InitializeItemDataUI();

        mapManager.InitalizeMapUI();
    }

    #region Loading Function
    /// <summary>
    /// ���� ������ �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="SceneName"> ������ �� �̸�</param>
    public void ChangeToTargetScene(string SceneName, GameObject playerObject)
    {
        GameObject obj = Instantiate(playerObject, loadPlayerGameObject.transform); // �÷��̾ �ε� ������Ʈ�� ����
        obj.transform.position = Vector3.zero;                                      // ������Ʈ ��ġ �ʱ�ȭ
        savedInventory = playerObject.GetComponent<Player>().Inventory;             // �κ��丮 ����
        savedEquipParts = playerObject.GetComponent<Player>().EquipPart;            // �������� ���� ����

        loadPlayerGameObject.SetActive(false);

        TargetSceneName = SceneName;
    }

    /// <summary>
    /// �� �ε��� ���� �� �÷��̾� ������ �����ϴ� �Լ�
    /// </summary>
    public void SpawnPlayerAfterLoadScene()
    {
        if (loadPlayerGameObject.transform.childCount < 1)
            return;

        if (!isLoading)
        {
            loadPlayerGameObject.SetActive(true);
            GameObject loadingPlayer = Instantiate(loadPlayerGameObject.transform.GetChild(0).gameObject);  // ���ο� ���� �÷��̾� ����
            loadingPlayer.name = "Player";

            loadingPlayer.transform.position = Vector3.zero;

            Destroy(loadPlayerGameObject.transform.GetChild(0).gameObject); // ����� �÷��̾� ������Ʈ ����

            player = loadingPlayer.GetComponent<Player>();  // �÷��̾� �ʱ�ȭ
            player.GetInventoryData(savedInventory);        // �÷��̾� �κ��丮 ������ �ޱ�
        }
    }

    /// <summary>
    /// ���� �̵��� �� ȣ��Ǵ� �Լ� ( �ε������� �̵� )
    /// </summary>
    void ChangeToLoadingScene()
    {
        SceneManager.LoadScene("02_LoadingScene");
        isLoading = true;
    }
    #endregion

#if UNITY_EDITOR
    public bool isNPC = false;
    public Action onTalkNPC;
    public Action onTalkObj;
    public void StartTalk()
    {
        //onTalk?.Invoke();

        if (!isNPC)
        {
            onTalkNPC?.Invoke();
            Debug.Log("��ȣ�ۿ� Ű ����");
        }
        else
        {
            onTalkObj?.Invoke();
            Debug.Log("������Ʈ�� ��ȭ");
        }
    }

    public Action onNextTalk;
    public void NextTalk()
    {
        onNextTalk?.Invoke();
    }

    public void IsNPCObj()
    {
        isNPC = !isNPC;
    }

    public Action openChase;
    public void OpenChest()
    {
        openChase?.Invoke();
    }
#endif
}
