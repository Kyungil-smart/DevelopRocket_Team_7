using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectPreviewUI : MonoBehaviour
{
    [Header("미리보기 이미지")]

    [Tooltip("왼쪽 큰 무기 이미지")]
    [SerializeField] private Image previewImage;

    [Header("미리보기 텍스트")]

    [Tooltip("왼쪽 아래 설명 텍스트")]
    [SerializeField] private TextMeshProUGUI descriptionText;


    [Header("기본 문구")]

    [Tooltip("아직 무기를 선택하지 않았을 때 표시할 문구")]
    [TextArea(2, 5)]
    [SerializeField] private string defaultDescription = "무기를 선택하면 이 영역에 무기 설명이 표시됩니다.";


    public void ShowEmpty()
    {
        if (previewImage != null)
        {
            previewImage.sprite = null;
            previewImage.enabled = false;
            previewImage.color = Color.white;
        }

        if (descriptionText != null)
        {
            descriptionText.text = defaultDescription;
        }
    }

    public void ShowWeapon(WeaponCatalogEntry entry)
    {
        if (entry == null)
        {
            ShowEmpty();
            return;
        }

        if (previewImage != null)
        {
            previewImage.sprite = entry.weaponIcon;
            previewImage.enabled = entry.weaponIcon != null;
            previewImage.color = Color.white;
        }

        if (descriptionText != null)
        {
            descriptionText.text = string.IsNullOrWhiteSpace(entry.description)
                ? "설명 데이터가 아직 등록되지 않았습니다."
                : entry.description;
        }
    }
}