using System.Collections;
using UnityEngine;

public class MainMenuLoadingFlow : MonoBehaviour
{
    [Header("로딩 설정")]

    [Tooltip("0%에서 100%까지 연출 시간(초)")]
    [Min(1)]
    [SerializeField] private int loadingDuration = 3;


    [Header("화면 참조")]

    [Tooltip("패널 표시 전환을 담당하는 뷰")]
    [SerializeField] private LoadingScreenView screenView;

    [Tooltip("로딩 게이지 표시를 담당하는 뷰")]
    [SerializeField] private LoadingProgressView progressView;

    [Tooltip("확인 버튼 표시를 담당하는 뷰")]
    [SerializeField] private LoadingConfirmButtonView confirmButtonView;


    private bool isLoading;
    private bool isCompleted;
    private Coroutine loadingCoroutine;


    private void Awake()
    {
        Initialize();
    }


    private void Initialize()
    {
        isLoading = false;
        isCompleted = false;

        if (screenView != null)
        {
            screenView.Initialize();
        }

        if (progressView != null)
        {
            progressView.Initialize();
        }

        if (confirmButtonView != null)
        {
            confirmButtonView.Initialize();
            confirmButtonView.SetState(false);
        }
    }


    public void BeginLoading()
    {
        if (isLoading)
        {
            return;
        }

        if (screenView == null || progressView == null || confirmButtonView == null)
        {
            Debug.LogWarning("로딩에 필요한 View 연결되지 않았습니다.");
            return;
        }

        isLoading = true;
        isCompleted = false;

        screenView.ShowLoading();
        progressView.SetProgress(0);
        confirmButtonView.SetState(false);

        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }

        loadingCoroutine = StartCoroutine(LoadingRoutine());
    }


    private IEnumerator LoadingRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.deltaTime;

            float normalized = Mathf.Clamp01(elapsedTime / loadingDuration);
            int percent = Mathf.RoundToInt(normalized * 100f);

            progressView.SetProgress(percent);

            yield return null;
        }

        progressView.SetProgress(100);

        isLoading = false;
        isCompleted = true;
        confirmButtonView.SetState(true);
        loadingCoroutine = null;
    }


    public void OnClickConfirm()
    {
        // 씬 매니저로 게임뷰로 넘어가도록 수정하기
        if (!isCompleted)
        {
            return;
        }
        SceneChanger.Instance.ChangeScene("InGameScene");
    }
}