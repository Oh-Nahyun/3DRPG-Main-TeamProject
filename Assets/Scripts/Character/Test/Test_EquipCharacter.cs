using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �κ��丮�� �׽�Ʈ ĳ���� ��ũ��Ʈ
/// </summary>
public class Test_EquipCharacter : MonoBehaviour, IEquipTarget, IHealth
{   
    public float hp;
    public float HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHP);
            onHealthChange?.Invoke(hp);
        }
    }

    public float maxHP = 5;
    public float MaxHP => maxHP;

    /// <summary>
    /// ü���� ����� �� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// ĳ���Ͱ� ����ִ��� Ȯ���ϴ� ������Ƽ ( 0 �ʰ� : true,)
    /// </summary>
    public bool IsAlive => HP > 0;

    /// <summary>
    /// ĳ���Ͱ� ����ϸ� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// ������ ������ ��ġ ( equipPart ������� �ʱ�ȭ �ؾ���)
    /// </summary>
    [Tooltip("Equip Part�� �����ϰ� ��ġ�� ��")]
    public Transform[] partPosition;

    /// <summary>
    /// ������ ������ �����۵�
    /// </summary>
    private InventorySlot[] equipPart;

    public InventorySlot[] EquipPart
    {
        get => equipPart;
        set
        {
            equipPart = value;
        }
    }

    Inventory inventory;
    PlayerinputActions input;

    Interaction interaction;

    int partCount = Enum.GetNames(typeof(EquipPart)).Length;

    void Awake()
    {
        input = new PlayerinputActions();   // ��ǲ ��ü ����
        interaction = GetComponent<Interaction>();
    }

    void Start()
    {
        inventory = new Inventory(this.gameObject, 16); // �κ� �ʱ�ȭ
        GameManager.Instance.ItemDataManager.InventoryUI.InitializeInventoryUI(inventory); // �κ� UI �ʱ�ȭ

        EquipPart = new InventorySlot[partCount]; // EquipPart �迭 �ʱ�ȭ

        HP = MaxHP; // ü�� �ʱ�ȭ

#if UNITY_EDITOR
        Test_AddItem();
#endif
    }

    void OnEnable()
    {
        input.Player.Enable();
        input.Player.Open_Inventory.performed += OnOpenInventory;
        input.Player.Get_Item.performed += OnGetItem;
        input.Player.Get_Item.canceled += OnGetItem;
    }

    /// <summary>
    /// �������� ȹ���ϴ� ��ǲ ( F Key )
    /// </summary>
    private void OnGetItem(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("FŰ ����");

            if(interaction.short_enemy != null) // ������ ������ ������Ʈ�� �����Ѵ�.
            {
                GameObject itemObject = interaction.short_enemy.gameObject;       // ���� ����� ������Ʈ 
                if(itemObject.TryGetComponent(out ItemDataObject itemDataObject)) // �ش� ������Ʈ�� ItemDataObject Ŭ������ �����ϸ� true
                {
                    itemDataObject.AdditemToInventory(inventory);                    
                }
                else
                {
                    Debug.Log($"������Ʈ ���� ItemDataObject Ŭ������ �������� �ʽ��ϴ�.");
                }
            }
            else
            {
                Debug.Log($"������ ������ ������Ʈ�� �������� �ʽ��ϴ�.");
            }
        }
    }

    void OnDisable()
    {
        input.Player.Get_Item.canceled -= OnGetItem;
        input.Player.Get_Item.performed -= OnGetItem;
        input.Player.Open_Inventory.performed -= OnOpenInventory;
        input.Player.Disable();
    }

    private void OnOpenInventory(InputAction.CallbackContext _)
    {
        GameManager.Instance.ItemDataManager.InventoryUI.ShowInventory();

        GameManager.Instance.ItemDataManager.CharaterRenderCameraPoint.transform.eulerAngles = new Vector3(0, 180f, 0); //
    }

    /// <summary>
    /// ĳ���� ������ ������ �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="equipment">��� ������</param>
    /// <param name="part">������ ����</param>
    public void CharacterEquipItem(GameObject equipment, EquipPart part, InventorySlot slot)
    {
        if (EquipPart[(int)part] != null) // ������ �������� ������
        {
            // false
            CharacterUnequipItem(part); // �����ߴ� ������ �ı�

            Instantiate(equipment, partPosition[(int)part]); // ������ ������Ʈ ����
            EquipPart[(int)part] = slot;    // ���������� ������ ���� ����
        }
        else // ������ �������� ������
        {
            EquipPart[(int)part] = slot;
            Instantiate(equipment, partPosition[(int)part]); // ������ ������Ʈ ����
        }
    }

    /// <summary>
    /// ĳ���� ������ �������� �� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="part"></param>
    public void CharacterUnequipItem(EquipPart part)
    {
        Destroy(partPosition[(int)part].GetChild(0).gameObject);    // ������ ������Ʈ �ı�
    }

    /// <summary>
    /// ����� ����Ǵ� �Լ�
    /// </summary>
    public void Die()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// ü��ȸ�� �� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="totalRegen">�� ȸ����</param>
    /// <param name="duration">ȸ�� �ֱ� �ð�</param>
    public void HealthRegenerate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegen_Coroutine(totalRegen, duration));
    }

    /// <summary>
    /// ü�� ȸ�� �ڷ�ƾ
    /// </summary>
    /// <param name="totalRegen">���� ȸ���� ü�·�</param>
    /// <param name="Duration">ü��ȸ���ϴ� �ð�</param>
    /// <returns></returns>
    IEnumerator HealthRegen_Coroutine(float totalRegen, float Duration)
    {
        float timeElapsed = 0f;
        while(timeElapsed < Duration)
        {
            timeElapsed += Time.deltaTime;
            HP += (totalRegen / Duration) * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// ƽ�� ü�� ȸ���� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="tickRegen">ƽ�� ȸ����</param>
    /// <param name="tickInterval">ȸ�� �ֱ�</param>
    /// <param name="totalTickCount">���� ƽ ��</param>
    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        StartCoroutine(HealthRegenByTick_Coroutine(tickRegen, tickInterval, totalTickCount));
    }

    /// <summary>
    /// ƽ�� ü�� ȸ�� �ڷ�ƾ
    /// </summary>
    /// <param name="tickRegen">ƽ�� ȸ����</param>
    /// <param name="tickInterval">ȸ�� �ֱ�</param>
    /// <param name="totalTickCount">���� ƽ ��</param>
    /// <returns></returns>
    IEnumerator HealthRegenByTick_Coroutine(float tickRegen, float tickInterval, uint totalTickCount)
    {
        for (int i = 0; i < totalTickCount; i++)
        {
            float timeElapsed = 0f;
            while (timeElapsed < tickInterval)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            HP += tickRegen;
        }
    }

#if UNITY_EDITOR
    void Test_AddItem()
    {
        inventory.AddSlotItem((uint)ItemCode.Hammer);
        inventory.AddSlotItem((uint)ItemCode.Sword);
        inventory.AddSlotItem((uint)ItemCode.HP_portion,3);
        inventory.AddSlotItem((uint)ItemCode.Coin);
    }
#endif
}