using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponSelectPanelUI : MonoBehaviour
{
    [Header("카탈로그")]
    [Tooltip("무기 선택 화면에서 사용할 카탈로그 SO")]
    [SerializeField] private WeaponCatalogSO weaponCatalog;
    
    [Header("왼쪽 미리보기")]
    [Tooltip("왼쪽 미리보기 영역 스크립트")]
    [SerializeField] private Image previewUIImg;
    [SerializeField] private TextLoader previewUIText;

    [Header("버튼")]
    [Tooltip("최종 확정 버튼")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private List<Button> weaponButtons;

    private WeaponType _selectdWeapon = WeaponType.None;

    private void Awake()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(HandleClickConfirm);
        }
    }

    public void ShowPreview(int index)
    {
        WeaponCatalogEntry wSo = weaponCatalog.GetEntry(index);
        if (wSo.weaponType == _selectdWeapon)
        {
            previewUIImg.sprite = wSo.weaponIcon;
            previewUIImg.enabled = true;
            previewUIText.SetTextId(wSo.locTxtNum);
        }
    }

    // 버튼 연결
    public void OnSelectWeapon(int index)
    {
        _selectdWeapon = (WeaponType)index;
        ShowPreview(index);
    }
    
    private void HandleClickConfirm()
    {
        if (_selectdWeapon == WeaponType.None)
        {
            Debug.LogWarning("WeaponSelectPanelUI : 선택된 무기가 없습니다.");
            return;
        }
        
        PostManager.Instance.Post(PostMessageKey.SelectWeapon, _selectdWeapon);
        gameObject.SetActive(false);
    }
}