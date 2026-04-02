using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectSlotUI : MonoBehaviour
{
    [Header("슬롯 버튼")]

    [Tooltip("슬롯 클릭을 받는 버튼")]
    [SerializeField] private Button slotButton;


    [Header("슬롯 표시")]

    [Tooltip("슬롯 안 무기 이미지")]
    [SerializeField] private Image iconImage;

    [Tooltip("슬롯 아래 무기 이름 텍스트")]
    [SerializeField] private TextMeshProUGUI nameText;


    private WeaponCatalogEntry currentEntry;
    private WeaponSelectPanelUI panelUI;


    public void Setup(WeaponCatalogEntry entry, WeaponSelectPanelUI ownerPanel)
    {
        currentEntry = entry;
        panelUI = ownerPanel;

        if (iconImage != null)
        {
            iconImage.sprite = entry.weaponIcon;
            iconImage.enabled = entry.weaponIcon != null;
            iconImage.color = Color.white;
        }

        if (nameText != null)
        {
            nameText.text = entry.weaponName;
        }

        if (slotButton != null)
        {
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(HandleClickSlot);
        }
    }

    private void HandleClickSlot()
    {
        if (panelUI == null || currentEntry == null)
        {
            return;
        }

        panelUI.SelectSlot(currentEntry);
    }
}