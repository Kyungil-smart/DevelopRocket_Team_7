using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    public void OnClickCloseSetting() => gameObject.SetActive(false);

    public void OnChangeToKorean() => 
        PostManager.Instance.Post(PostMessageKey.ChangeLanguage, LanguageType.Korean);
    
    public void OnChangeToEnglish() => 
        PostManager.Instance.Post(PostMessageKey.ChangeLanguage, LanguageType.English);
}
