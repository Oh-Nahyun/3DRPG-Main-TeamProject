using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    #region AsyncLoad Values
    /// <summary>
    /// 로딩 씬이 끝나고 불려진 다음 씬 이름
    /// </summary>
    public string nextSceneName = "LoadedSampleScene";

    /// <summary>
    /// 유니티 비동기 명령 처리 클래스
    /// </summary>
    AsyncOperation async;

    Slider loadingSlider;
    TextMeshProUGUI loadingDoneText;
    PlayerinputActions inputActions;

    IEnumerator loadingCoroutine;

    float loadRatio;
    public float loadingBarSpeed = 1.0f;

    /// <summary>
    /// 로딩 완료 여부 ( true : 완료, false : 미완 )
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
        loadingDoneText = FindAnyObjectByType<TextMeshProUGUI>();
        loadingDoneText.gameObject.SetActive(false);
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
        // 슬라이더의 value가 loadRatio가 될 때까지 계속 증가
        if (loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }

    #endregion

    #region AsyncLoad Method
    /// <summary>
    /// 클릭시 실행하는 함수
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

        async = SceneManager.LoadSceneAsync(nextSceneName); // 비동기 로딩 시작
        async.allowSceneActivation = false;                 // 자동 씬 변환 비활성화

        while (loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f; // 진행률 갱신

            yield return null;
        }

        yield return new WaitForSeconds((1 - loadingSlider.value / loadingBarSpeed));

        loadingDoneText.gameObject.SetActive(true);
        loadingDone = true;
    }

    #endregion
}
