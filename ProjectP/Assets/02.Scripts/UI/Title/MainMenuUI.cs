using System.Collections;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("설정 패널")]
    [Tooltip("환경 설정 창 오브젝트")] [SerializeField] private GameObject settingPanel;

    [Header("로딩")]
    [Tooltip("메인 메뉴에서 사용할 로딩 스크립트")] [SerializeField] private MainMenuLoadingFlow loadingFlow;
    
    [Header("Title BGM")]
    [SerializeField] private AudioClip audioClip;
    private Coroutine audioCoroutine;


    private void OnEnable()
    {
        if (audioCoroutine == null)
        {
            StartCoroutine(AudioStartCoroutine());
            audioCoroutine = null;
        }
    }

    private void OnDisable()
    {
        if (audioCoroutine != null) StopCoroutine(audioCoroutine);
    }

    private IEnumerator AudioStartCoroutine()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            AudioManager.Instance.OnBgmPlay(audioClip);
            yield return new WaitForSeconds(audioClip.length + 1f);
        }
    }

    public void OnClickStart()
    {
        if (loadingFlow == null)
        {
            Debug.LogWarning("MainMenuLoadingFlow가 연결되지 않았습니다.");
            return;
        }

        loadingFlow.BeginLoading();
    }


    public void OnClickOpenSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("SettingPanel이 연결되지 않았습니다.");
        }
    }


    public void OnClickCloseSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SettingPanel이 연결되지 않았습니다.");
        }
    }


    public void OnClickQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}