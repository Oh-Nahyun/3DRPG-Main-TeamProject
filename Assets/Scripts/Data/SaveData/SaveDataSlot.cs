using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SaveDataSlot : MonoBehaviour, IPointerClickHandler
{
    SaveHandler handler;

    /// <summary>
    /// ���� �ε���
    /// </summary>
    int saveIndex;

    /// <summary>
    /// ���� �̸� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI saveName;

    void Awake()
    {
        handler = GetComponentInParent<SaveHandler>();
        saveName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton buttonValue = eventData.button;
        
        if(buttonValue == PointerEventData.InputButton.Left) // ���� Ŭ���ϸ� ���̺�
        {
            handler.onClickSaveSlot?.Invoke(handler.saveIndex);
        }
        
        if(buttonValue == PointerEventData.InputButton.Right)
        {
            handler.onClickLoadSlot?.Invoke(handler.saveIndex);
        }
    }

    /// <summary>
    /// ���̺� ���� �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="index">���� �ε���</param>
    public void SlotInitialize(int index)
    {
        saveIndex = index;
        saveName.text = $"SaveData {saveIndex}";
    }

    /// <summary>
    /// ���̺� �����Ͱ� �����ϴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="isEmtpy">��������� true �ƴϸ� false</param>
    public void CheckSave(bool isEmtpy)
    {
        if(isEmtpy)
        {
            saveName.text = $"SaveData {saveIndex} ";
        }
        else
        {
            saveName.text = $"SaveData {saveIndex} VVVV";
        }
    }
}
