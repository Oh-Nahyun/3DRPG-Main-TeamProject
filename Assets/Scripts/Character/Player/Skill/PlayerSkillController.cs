using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// 플레이어 스킬입력
/// </summary>
public class PlayerSkillController : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    IEnumerator[] specialKeyPress;

    void Awake()
    {
        playerInputAction = new PlayerinputActions();

        int specialKeyCount = Enum.GetValues(typeof(PlayerSkills.SpecialKey)).Length;
        onSpecialKey = new Action[specialKeyCount];
        specialKeyPress = new IEnumerator[specialKeyCount];
        for (int i = 0; i < specialKeyCount; i++)
        {
            specialKeyPress[i] = SpecialKeyPress((PlayerSkills.SpecialKey)i);
        }
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

        playerInputAction.Skill.SpecialKeyDown.performed += OnSpecialKeyDownPress;
        playerInputAction.Skill.SpecialKeyDown.canceled += OnSpecialKeyDownRelease;
        playerInputAction.Skill.SpecialKeyUp.performed += OnSpecialKeyUpPress;
        playerInputAction.Skill.SpecialKeyUp.canceled += OnSpecialKeyUpRelease;
        playerInputAction.Skill.SpecialKeyLeft.performed += OnSpecialKeyLeftPress;
        playerInputAction.Skill.SpecialKeyLeft.canceled += OnSpecialKeyLeftPress;
        playerInputAction.Skill.SpecialKeyRight.performed += OnSpecialKeyRightPress;
        playerInputAction.Skill.SpecialKeyRight.canceled += OnSpecialKeyRightPress;

    }

    void OnDisable()
    {
        // Player Skills
        playerInputAction.Skill.SpecialKeyRight.canceled -= OnSpecialKeyRightPress;
        playerInputAction.Skill.SpecialKeyRight.performed -= OnSpecialKeyRightPress;
        playerInputAction.Skill.SpecialKeyLeft.canceled -= OnSpecialKeyLeftPress;
        playerInputAction.Skill.SpecialKeyLeft.performed -= OnSpecialKeyLeftPress;
        playerInputAction.Skill.SpecialKeyUp.canceled -= OnSpecialKeyUpRelease;
        playerInputAction.Skill.SpecialKeyUp.performed -= OnSpecialKeyUpPress;
        playerInputAction.Skill.SpecialKeyDown.canceled -= OnSpecialKeyDownRelease;
        playerInputAction.Skill.SpecialKeyDown.performed -= OnSpecialKeyDownPress;

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
    public Action<SkillName> onSkillChange;
    public Action onThrow;
    public Action onCancel;
    public Action[] onSpecialKey;

    /// <summary>
    /// 선택된 스킬이 바뀌었음을 알리는 델리게이트 (F1:리모컨폭탄 F2:리모컨폭탄큐브 F3:마그넷캐치 F4:아이스메이커 F5:타임록)
    /// </summary>
    public Action<SkillName> onSkillSelect;

    /// <summary>
    /// 우클릭: 상호작용
    /// </summary>
    public Action rightClick;

    #region Player behavior
    private void OnSkill(InputAction.CallbackContext _)
    {
        onSkillActive?.Invoke();
    }
    private void OnSkill1(InputAction.CallbackContext _)
    {
        onSkillChange?.Invoke(SkillName.RemoteBomb);
    }
    private void OnSkill2(InputAction.CallbackContext _)
    {
        onSkillChange?.Invoke(SkillName.RemoteBomb_Cube);
    }
    private void OnSkill3(InputAction.CallbackContext _)
    {
        onSkillChange?.Invoke(SkillName.MagnetCatch);
    }
    private void OnSkill4(InputAction.CallbackContext _)
    {
        onSkillChange?.Invoke(SkillName.IceMaker);
    }
    private void OnSkill5(InputAction.CallbackContext context)
    {
        onSkillChange?.Invoke(SkillName.TimeLock);
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

    bool isSpecialKeyDown = false;

    private void OnSpecialKeyDownPress(CallbackContext context)
    {
        StopCoroutine(specialKeyPress[(int)PlayerSkills.SpecialKey.NumPad8_Up]);

        StartCoroutine(specialKeyPress[(int)PlayerSkills.SpecialKey.NumPad5_Down]);
    }
    private void OnSpecialKeyDownRelease(CallbackContext context)
    {

        StopCoroutine(specialKeyPress[(int)PlayerSkills.SpecialKey.NumPad5_Down]);
        onSpecialKey[(int)PlayerSkills.SpecialKey.None]?.Invoke();
    }
    private void OnSpecialKeyUpPress(CallbackContext context)
    {
        StopCoroutine(specialKeyPress[(int)PlayerSkills.SpecialKey.NumPad5_Down]);

        StartCoroutine(specialKeyPress[(int)PlayerSkills.SpecialKey.NumPad8_Up]);

    }
    private void OnSpecialKeyUpRelease(CallbackContext context)
    {
        StopCoroutine(specialKeyPress[(int)PlayerSkills.SpecialKey.NumPad8_Up]);
        onSpecialKey[(int)PlayerSkills.SpecialKey.None]?.Invoke();
    }
    private void OnSpecialKeyRightPress(CallbackContext context)
    {
        isSpecialKeyDown = context.performed;
        if(isSpecialKeyDown)
            onSpecialKey[(int)PlayerSkills.SpecialKey.NumPad6_Right]?.Invoke();
    }

    private void OnSpecialKeyLeftPress(CallbackContext context)
    {
        isSpecialKeyDown = context.performed;
        if(isSpecialKeyDown)
            onSpecialKey[(int)PlayerSkills.SpecialKey.NumPad4_Left]?.Invoke();
    }

    IEnumerator SpecialKeyPress(PlayerSkills.SpecialKey key)
    {
        while (true)
        {
            if (!isSpecialKeyDown)
            {
                onSpecialKey[(int)key]?.Invoke();
            }
            yield return null;
        }
    }

    #endregion
}
