using UnityEngine;

public class LoadingScreenView : MonoBehaviour
{
    [Header("패널 참조")]

    [Tooltip("처음에 보이는 타이틀 패널")]
    [SerializeField] private GameObject titlePanel;

    [Tooltip("로딩 중에 보이는 로딩 패널")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject loadingImgKR;
    [SerializeField] private GameObject loadingImgEN;
    
    private LanguageType languageType = LanguageType.Korean;

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<LanguageType>(PostMessageKey.ChangeLanguage, ChangeLanguage);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<LanguageType>(PostMessageKey.ChangeLanguage, ChangeLanguage);
    }

    private void ChangeLanguage(LanguageType languageType)
    {
        this.languageType = languageType;
    }
    
    public void Initialize()
    {
        if (titlePanel != null) titlePanel.SetActive(true);
        if (loadingPanel != null) loadingPanel.SetActive(false);
        if (loadingImgEN != null) loadingImgEN.SetActive(false);
        if (loadingImgKR != null) loadingImgKR.SetActive(false);
    }

    public void ShowLoading()
    {
        if (loadingPanel != null) loadingPanel.SetActive(true);
        if (languageType == LanguageType.English)
        {
            if (loadingImgEN != null) loadingImgEN.SetActive(true);
        }
        else
        {
            if (loadingImgKR != null) loadingImgKR.SetActive(true);
        } 
    }
}