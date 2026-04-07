using UnityEngine;
using UnityEngine.InputSystem;

public class SettingPanelController : MonoBehaviour
{
    [Header("Button Sound")]
    [SerializeField] private AudioClip buttonSound;
    
    [Header("Button Sound")]
    [SerializeField] private InputActionAsset _inputAsset;
    private InputAction _input;
    
    public void OnClickCloseSetting()
    {
        AudioManager.Instance.OnSfxPlayOnShot(buttonSound);
        gameObject.SetActive(false);
    }

    public void OnChangeToKorean()
    {
        AudioManager.Instance.OnSfxPlayOnShot(buttonSound);    
        PostManager.Instance.Post(PostMessageKey.ChangeLanguage, LanguageType.Korean);
    }
    
    public void OnChangeToEnglish()
    {
        AudioManager.Instance.OnSfxPlayOnShot(buttonSound);   
        PostManager.Instance.Post(PostMessageKey.ChangeLanguage, LanguageType.English);
    }
}
