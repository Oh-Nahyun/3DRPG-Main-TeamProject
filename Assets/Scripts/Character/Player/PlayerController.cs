using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// 플레이어 인풋만 받는 스크립트
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    // movment delegate
    public Action<Vector2, bool> onMove;
    public Action onMoveModeChagne;
    public Action<Vector2, bool> onLook;
    public Action<bool> onJump;
    public Action<bool> onSlide;
    public Action onSkillModeChange;

    // behavior delegate
    public Action onInteraction;

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

        // Player Inventory
        playerInputAction.Player.Open_Inventory.performed += OnOpenInventory;
        playerInputAction.Player.Get_Item.performed += OnGetItem;

        //playerInputAction.Player.ActiveSkillMode.performed += OnSkillModeChange;
    }

    void OnDisable()
    {
        //playerInputAction.Player.ActiveSkillMode.performed -= OnSkillModeChange;

        // Player Inventory
        playerInputAction.Player.Open_Inventory.performed -= OnOpenInventory;
        playerInputAction.Player.Get_Item.performed -= OnGetItem;

        // Player Movement
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

    /*private void OnSkillModeChange(CallbackContext context)
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

        Debug.Log($"스킬 모드 활성화 여부 : {playerInputAction.Skill.enabled}");
    }*/

    /// <summary>
    /// 이동 처리 함수
    /// </summary>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context.ReadValue<Vector2>(), !context.canceled);
    }

    /// <summary>
    /// 이동 모드 변경 함수
    /// </summary>
    private void OnMoveModeChangeInput(CallbackContext _)
    {
        onMoveModeChagne?.Invoke();
    }

    /// <summary>
    /// 카메라 회전 입력 함수
    /// </summary>
    private void OnLookInput(InputAction.CallbackContext context)
    {
        onLook?.Invoke(context.ReadValue<Vector2>(), context.performed);
    }

    /// <summary>
    /// 점프 처리 함수
    /// </summary>
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        onJump?.Invoke(context.performed);
    }

    /// <summary>
    /// 회피 처리 함수
    /// </summary>
    private void OnSlideInput(InputAction.CallbackContext context)
    {
        onSlide?.Invoke(context.performed);
    }
    #endregion

    #region Player Inventory

    /// <summary>
    /// 인벤토리 열 때 실행되는 인풋 함수
    /// </summary>
    private void OnOpenInventory(InputAction.CallbackContext _)
    {
        GameManager.Instance.ItemDataManager.InventoryUI.ShowInventory();

        GameManager.Instance.ItemDataManager.CharaterRenderCameraPoint.transform.eulerAngles = new Vector3(0, 180f, 0); // RenderTexture 플레이어 위치 초기화
    }

    /// <summary>
    /// 아이템을 획득하는 인풋 ( F Key )
    /// </summary>
    private void OnGetItem(InputAction.CallbackContext context)
    {
        onInteraction?.Invoke();
    }
    #endregion

    /// <summary>
    /// 입력 처리 불가 처리 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator StopInput()
    {
        playerInputAction.Player.Disable();          // Player 액션맵 비활성화
        yield return new WaitForSeconds(4.0f);
        playerInputAction.Player.Enable();           // Player 액션맵 활성화
    }
}