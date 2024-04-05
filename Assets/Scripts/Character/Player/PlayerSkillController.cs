using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// �÷��̾� ��ų�Է�
/// </summary>
public class PlayerSkillController : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    void Awake()
    {
        playerInputAction = new PlayerinputActions();
    }

    void OnEnable()
    {
        playerInputAction.Skill.Enable();
        // player Skills
        playerInputAction.Skill.OnSkill.performed += OnSkill;
        playerInputAction.Skill.Skill1.performed += OnSkill1;
        playerInputAction.Skill.Skill2.performed += OnSkill2;
        playerInputAction.Skill.Skill3.performed += OnSkill3;
        playerInputAction.Skill.Skill4.performed += OnSkill4;
        playerInputAction.Skill.Skill5.performed += OnSkill5;

        playerInputAction.Skill.Throw.performed += OnThrow;
        playerInputAction.Skill.Cancel.performed += OnCancel;
        playerInputAction.Skill.RightClick.performed += OnRightClick;
    }

    void OnDisable()
    {
        // Player Skills
        playerInputAction.Skill.RightClick.performed -= OnRightClick;
        playerInputAction.Skill.Cancel.performed -= OnCancel;
        playerInputAction.Skill.Throw.performed -= OnThrow;

        playerInputAction.Skill.Skill5.performed -= OnSkill5;
        playerInputAction.Skill.Skill4.performed -= OnSkill4;
        playerInputAction.Skill.Skill3.performed -= OnSkill3;
        playerInputAction.Skill.Skill2.performed -= OnSkill2;
        playerInputAction.Skill.Skill1.performed -= OnSkill1;
        playerInputAction.Skill.OnSkill.performed -= OnSkill;
        playerInputAction.Skill.Disable();
    }


    // delegates
    public Action onSkillActive; // onSkill
    public Action onRemoteBomb; // skill 1
    public Action onRemoteBomb_Cube; // skill 2
    public Action onMagnetCatch; // skill 3
    public Action onIceMaker; // skill 4
    public Action onTimeLock; // skill 5
    public Action onThrow; 
    public Action onCancel;

    /// <summary>
    /// ���õ� ��ų�� �ٲ������ �˸��� ��������Ʈ (F1:��������ź F2:��������źť�� F3:���׳�ĳġ F4:���̽�����Ŀ F5:Ÿ�ӷ�)
    /// </summary>
    public Action<SkillName> onSkillSelect;

    /// <summary>
    /// ��Ŭ��: ��ȣ�ۿ�
    /// </summary>
    public Action rightClick;

    #region Player behavior
    private void OnSkill(InputAction.CallbackContext _)
    {
        onSkillActive?.Invoke();
    }
    private void OnSkill1(InputAction.CallbackContext _)
    {
        onRemoteBomb?.Invoke();
    }
    private void OnSkill2(InputAction.CallbackContext _)
    {
        onRemoteBomb_Cube?.Invoke();
    }
    private void OnSkill3(InputAction.CallbackContext _)
    {
        onMagnetCatch?.Invoke();
    }
    private void OnSkill4(InputAction.CallbackContext _)
    {
        onIceMaker?.Invoke();
    }
    private void OnSkill5(InputAction.CallbackContext context)
    {
        onTimeLock?.Invoke();
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        onThrow?.Invoke();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        onCancel?.Invoke();
    }

    private void OnRightClick(InputAction.CallbackContext _)
    {
        rightClick?.Invoke();
    }

    #endregion
}
