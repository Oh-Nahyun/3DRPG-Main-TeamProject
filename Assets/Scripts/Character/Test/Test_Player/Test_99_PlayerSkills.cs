using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �÷��̾��� ��ų���븸 �޴� ��ũ��Ʈ
/// </summary>
public class Test_99_PlayerSkills : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    void Awake()
    {
        playerInputAction = new PlayerinputActions();
    }

    void OnEnable()
    {
        //playerInputAction.Skill.OnSkill.performed += onsl
        playerInputAction.Enable();
    }

    void OnDisable()
    {
        playerInputAction.Player.Disable();
    }
}
