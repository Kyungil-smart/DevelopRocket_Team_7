using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct WeaponImageMap
{
    public WeaponType weaponType;
    public Sprite sprite;
}

public class MainUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private TextMeshProUGUI playerLvText;
    [SerializeField] private TextMeshProUGUI curMaxAmmoText;
    [SerializeField] private TextMeshProUGUI dashCntText;
    [SerializeField] private Image curWeaponImg;
    [SerializeField] private List<WeaponImageMap> wMap;
    [SerializeField] private GameObject _victoryWindow;
    [SerializeField] private GameObject _loseWindow;

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<WeaponType>(PostMessageKey.SelectWeapon, SetCurrentWeapon);
        PostManager.Instance.Subscribe<int>(PostMessageKey.MainUIPlayerHp, OnPlayerHpChange);
        PostManager.Instance.Subscribe<int>(PostMessageKey.MainUIPlayerLv, OnPlayerLvChange);
        PostManager.Instance.Subscribe<int>(PostMessageKey.MainUIDashCount, OnDashCountChange);
        PostManager.Instance.Subscribe<string>(PostMessageKey.MainUICurAmmo, OnCurMaxAmmoChange);
        PostManager.Instance.Subscribe<bool>(PostMessageKey.MainUIGameResult, OnPopupGameResult);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<WeaponType>(PostMessageKey.SelectWeapon, SetCurrentWeapon);
        PostManager.Instance.Unsubscribe<int>(PostMessageKey.MainUIPlayerHp, OnPlayerHpChange);
        PostManager.Instance.Unsubscribe<int>(PostMessageKey.MainUIPlayerLv, OnPlayerLvChange);
        PostManager.Instance.Unsubscribe<int>(PostMessageKey.MainUIDashCount, OnDashCountChange);
        PostManager.Instance.Unsubscribe<string>(PostMessageKey.MainUICurAmmo, OnCurMaxAmmoChange);
        PostManager.Instance.Unsubscribe<bool>(PostMessageKey.MainUIGameResult, OnPopupGameResult);
    }
    
    private void SetCurrentWeapon(WeaponType weaponType)
    {
        foreach (var w in wMap)
        {
            if (w.weaponType == weaponType)
            {
                curWeaponImg.sprite = w.sprite;
                curWeaponImg.enabled = true;
                return;
            }
        }

        Debug.Log("No Match Weapon");
    }

    private void OnPlayerHpChange(int value) => playerHpText.text = $"X {value}";
    private void OnPlayerLvChange(int value) => playerLvText.text = $"Lv: {value}";
    private void OnDashCountChange(int value) => dashCntText.text = $"X {value}";
    private void OnCurMaxAmmoChange(string curMaxAmmo) => curMaxAmmoText.text = curMaxAmmo;

    private void OnPopupGameResult(bool result)
    {
        Time.timeScale = 0f;
        if (result) _victoryWindow.SetActive(true);
        else _loseWindow.SetActive(true);
    }

    public void OnGoToTitle()
    {
        Time.timeScale = 1;
        SceneChanger.Instance.ChangeScene("TitleScene");
    }
    
}

