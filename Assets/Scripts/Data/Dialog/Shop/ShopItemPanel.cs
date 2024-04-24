using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Image itemImage;
    TextMeshProUGUI itemNameText;
    TextMeshProUGUI itemStockText;
    TextMeshProUGUI itemPriceText;

    public ItemData itemData;
    ShopItemData shopItemData;

    Inventory inventory;

    Test_EquipCharacter player;

    public Color inStockColor = Color.white;
    public Color noStockColor = Color.red;

    /// <summary>
    /// �������� ���� ���
    /// </summary>
    public int itemStock = 10;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        itemImage = child.GetComponent<Image>();

        child = transform.GetChild(1);
        itemNameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        itemStockText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        child = child.transform.GetChild(1);
        itemPriceText = child.GetComponent<TextMeshProUGUI>();
        shopItemData = FindAnyObjectByType<ShopItemData>();

        player = FindAnyObjectByType<Test_EquipCharacter>();
    }

    private void Start()
    {
        if (itemData != null)
        {
            itemImage.sprite = itemData.itemIcon;
            itemNameText.text = itemData.itemName;
            itemStockText.text = itemStock.ToString();
            itemPriceText.text = itemData.price.ToString();
        }
    }

    private void Update()
    {
        setShopItemText();
    }

    /// <summary>
    /// ���콺�� ������ �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {   
        if (shopItemData != null)
        {
            // �ش� �г��� ItemData�� �ǳ���
            shopItemData.GetItemData(itemData);
        }
    }

    /// <summary>
    /// ���콺�� ������ �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (shopItemData != null)
        {
            shopItemData.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventory.Gold >= itemData.price)
        {
            if (itemStock > 0)
            {
                inventory.AddSlotItem((uint)itemData.itemCode);
                itemStock--;
                itemStockText.text = itemStock.ToString();

                inventory.SubCoin(itemData.price);
            }
            else
            {
                Debug.Log($"{itemData.itemName} ��� ����");
            }
        }
        else
        {
            Debug.Log("�ܾ��� ���ڸ�");
        }
    }

    private void setShopItemText()
    {
        if (inventory == null)
        {
            inventory = player.Inventory;
        }

        if (inventory.Gold > itemData.price)
        {
            itemNameText.color = inStockColor;
            itemPriceText.color = inStockColor;
        }
        else
        {
            itemNameText.color = noStockColor;
            itemPriceText.color = noStockColor;
        }

        if (itemStock > 0)
        {
            itemStockText.color = inStockColor;
        }
        else
        {
            itemNameText.color = noStockColor;
            itemStockText.color = noStockColor;
        }
    }
}
