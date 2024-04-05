using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// �÷��̾� ��ǲ�� �޴� ��ũ��Ʈ
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    // movment delegate
    public Action<Vector2, bool> onMove;
    public Action onMoveModeChagne;
    public Action<Vector2, bool> onLook;
    public Action onSlide;
    public Action<bool> onJump;
    public Action onSkillModeChange;

    // behavior delegate


    void Awake()
    {
        playerInputAction = new PlayerinputActions();
    }

    void OnEnable()
    {
        playerInputAction.Player.Enable();
        // Player Movement
        playerInputAction.Player.Move.performed += OnMoveInput;
        playerInputAction.Player.Move.canceled += OnMoveInput;
        playerInputAction.Player.LookAround.performed += OnLookInput;
        playerInputAction.Player.LookAround.canceled += OnLookInput;
        playerInputAction.Player.Jump.performed += OnJumpInput;
        playerInputAction.Player.Slide.performed += OnSlideInput;
        playerInputAction.Player.MoveModeChange.performed += OnMoveModeChangeInput;

        playerInputAction.Player.ActiveSkillMode.performed += OnSkillModeChange;
    }

    void OnDisable()
    {
        // Player Movement
        playerInputAction.Player.ActiveSkillMode.performed -= OnSkillModeChange;

        playerInputAction.Player.MoveModeChange.performed -= OnMoveModeChangeInput;
        playerInputAction.Player.Slide.performed -= OnSlideInput;
        playerInputAction.Player.Jump.performed -= OnJumpInput;
        playerInputAction.Player.LookAround.canceled -= OnLookInput;
        playerInputAction.Player.LookAround.performed -= OnLookInput;
        playerInputAction.Player.Move.canceled -= OnMoveInput;
        playerInputAction.Player.Move.performed -= OnMoveInput;

        playerInputAction.Player.Disable();
    }

    #region Player Movement Input

    private void OnSkillModeChange(CallbackContext context)
    {
        bool isActiveSelf = playerInputAction.Skill.enabled;
        if (!isActiveSelf)
        {
            playerInputAction.Skill.Enable();
            playerInputAction.Weapon.Disable();
        }
        else
        {
            playerInputAction.Skill.Disable();
            playerInputAction.Weapon.Enable();
        }

        Debug.Log($"��ų ��� Ȱ��ȭ ���� : {playerInputAction.Skill.enabled}");
    }

    /// <summary>
    /// �̵� ó�� �Լ�
    /// </summary>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context.ReadValue<Vector2>(), !context.canceled);
    }

    /// <summary>
    /// �̵� ��� ���� �Լ�
    /// </summary>
    private void OnMoveModeChangeInput(CallbackContext _)
    {
        onMoveModeChagne?.Invoke();
    }

    /// <summary>
    /// ī�޶� ȸ�� �Է� �Լ�
    /// </summary>
    private void OnLookInput(InputAction.CallbackContext context)
    {
        onLook?.Invoke(context.ReadValue<Vector2>(), context.performed);
    }

    /// <summary>
    /// ȸ�� ó�� �Լ�
    /// </summary>
    private void OnSlideInput(InputAction.CallbackContext context)
    {
        onSlide?.Invoke();
    }

    /// <summary>
    /// ���� ó�� �Լ�
    /// </summary>
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        onJump?.Invoke(context.performed);
    }
    #endregion

    /// <summary>
    /// �Է� ó�� �Ұ� ó�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    public IEnumerator StopInput()
    {
        playerInputAction.Player.Disable();          // Player �׼Ǹ� ��Ȱ��ȭ
        yield return new WaitForSeconds(4.0f);
        playerInputAction.Player.Enable();           // Player �׼Ǹ� Ȱ��ȭ
    }
}