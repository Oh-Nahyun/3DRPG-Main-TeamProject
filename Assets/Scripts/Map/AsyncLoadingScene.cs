using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    #region AsyncLoad Values
    /// <summary>
    /// �ε� ���� ������ �ҷ��� ���� �� �̸�
    /// </summary>
    public string nextSceneName = "LoadedSampleScene";

    /// <summary>
    /// ����Ƽ �񵿱� ��� ó�� Ŭ����
    /// </summary>
    AsyncOperation async;

    Slider loadingSlider;
    PlayerinputActions inputActions;

    IEnumerator loadingCoroutine;

    float loadRatio;
    public float loadingBarSpeed = 1.0f;

    /// <summary>
    /// �ε� �Ϸ� ���� ( true : �Ϸ�, false : �̿� )
    /// </summary>
    bool loadingDone = false;
    #endregion

    #region Loading Image
    LoadingImageUI loadingImageUI;

    #endregion

    #region LifeCycle Method
    private void Awake()
    {
        inputActions = new PlayerinputActions();

        loadingImageUI = FindAnyObjectByType<LoadingImageUI>();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.Click.performed -= Press;
        inputActions.UI.Disable();        
    }

    private void Start()
    {
        nextSceneName = GameManager.Instance.TargetSceneName;

        loadingSlider = FindAnyObjectByType<Slider>();
        loadingCoroutine = AsyncLoadScene();

        StartCoroutine(loadingCoroutine);
        loadingImageUI.ChangeLoadingImages();
    }

    private void Update()
    {
        // �����̴��� value�� loadRatio�� �� ������ ��� ����
        if(loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }

    #endregion

    #region AsyncLoad Method
    /// <summary>
    /// Ŭ���� �����ϴ� �Լ�
    /// </summary>
    private void Press(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = loadingDone;
        loadingImageUI.FinishLoadingImage();

        GameManager.Instance.isLoading = false;
    }

    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;

        async = SceneManager.LoadSceneAsync(nextSceneName); // �񵿱� �ε� ����
        async.allowSceneActivation = false;                 // �ڵ� �� ��ȯ ��Ȱ��ȭ

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f; // ����� ����

            yield return null;
        }

        yield return new WaitForSeconds((1 - loadingSlider.value / loadingBarSpeed));

        loadingDone = true;        
    }

    #endregion
}
