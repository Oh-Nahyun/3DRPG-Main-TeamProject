using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySortUI : MonoBehaviour
{
    TMP_Dropdown dropDown;
    Button checkBtn;
    //Button AsceningBtn;

    uint sortValue = 0;
    bool isAcending = false;

    /// <summary>
    /// ������ ������ �� �� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<uint, bool> onSortItem;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        dropDown = child.GetComponent<TMP_Dropdown>();

        dropDown.onValueChanged.AddListener((int value) =>
        {   // dropDown���� ������ ���� ����
            sortValue = (uint)value; 
        });

        child = transform.GetChild(1);
        checkBtn = child.GetComponent<Button>();
        checkBtn.onClick.AddListener(() =>
        {
            onSortItem?.Invoke(sortValue, isAcending);
        });
    }
}
