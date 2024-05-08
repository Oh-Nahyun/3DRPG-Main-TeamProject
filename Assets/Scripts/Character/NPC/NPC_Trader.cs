using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trader : NPCBase
{
    ShopInfo shop;
    TextBox textBox;
    Player player;
    Inventory playerInventory;
    SellPanelUI sellPanelUI;

    public string selectButtonText1_1;
    public string selectButtonText1_2;
    public string selectButtonText1_3;

    public bool isSellClose = true;


    protected override void Awake()
    {
        shop = FindAnyObjectByType<ShopInfo>();
        textBox = FindAnyObjectByType<TextBox>();
        player = FindAnyObjectByType<Player>();
        sellPanelUI = FindAnyObjectByType<SellPanelUI>();

        base.Awake();
        isNPC = true;    
    }

    protected override void Start()
    {
        base.Start();
        playerInventory = player.PlayerInventory;
        GameManager.Instance.ItemDataManager.SellPanelUI.GetTarget(playerInventory);
        sellPanelUI.onCloseButton += sellPanelUIClose;
    }

    protected override void Update()
    {
        base.Update();
        openShopinfo();
    }

    /// <summary>
    /// ���ΰ� ��ȭ�� ����� �Լ�
    /// </summary>
    public void openShopinfo()
    {
        if(id == 4011)
        {
            // ���� ����
            shop.gameObject.SetActive(true);
            shop.CanvasGroup.alpha = 1;
            if (!textBox.TalkingEnd)
            {
                id = 4010;
            }
        }else if (id == 4012)
        {
            GameManager.Instance.ItemDataManager.SellPanelUI.OpenSellUI();
            // �Ǹ� ����
            if (!textBox.TalkingEnd || !isSellClose)
            {
                GameManager.Instance.ItemDataManager.SellPanelUI.CloseSellUI();
                id = 4010;
                isSellClose = true;
            }
        }
        else
        {
            // ������
            if (!textBox.TalkingEnd)
            {
                id = 4010;
            }
        }
    }

    void sellPanelUIClose()
    {
        isSellClose = false;
    }


}
