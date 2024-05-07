using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// ���� ����
    /// </summary>
    TextMeshProUGUI saveDesc;

    /// <summary>
    /// ������ �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    public void InitializeComponent()
    {
        handler = GetComponentInParent<SaveHandler>();
        Transform child = transform.GetChild(1);
        saveName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        saveDesc = child.GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton buttonValue = eventData.button;
        
        if(buttonValue == PointerEventData.InputButton.Left) // ���� Ŭ���ϸ� ���̺�
        {
            Debug.Log($"{saveIndex}���� �����");
            handler.onClickSaveSlot?.Invoke(saveIndex);
        }
        
        if(buttonValue == PointerEventData.InputButton.Right)
        {
            Debug.Log($"{saveIndex}�� �ε���");
            handler.onClickLoadSlot?.Invoke(saveIndex);
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
    public void CheckSave(bool isEmtpy, int sceneNumber)
    {
        if(isEmtpy)
        {
            saveName.text = $"SaveData {saveIndex} ";
            saveDesc.text = $"Empty";
        }
        else
        {
            saveName.text = $"SaveData {saveIndex}";
            saveDesc.text = $"{SceneManager.GetSceneByBuildIndex(sceneNumber).name}";
        }
    }
}
