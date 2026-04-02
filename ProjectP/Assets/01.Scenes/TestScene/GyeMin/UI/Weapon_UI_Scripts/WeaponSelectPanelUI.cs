using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponSelectPanelUI : MonoBehaviour
{
    [Header("카탈로그")]

    [Tooltip("무기 선택 화면에서 사용할 카탈로그 SO")]
    [SerializeField] private WeaponCatalogSO weaponCatalog;


    [Header("슬롯 목록")]

    [Tooltip("오른쪽에 배치된 무기 슬롯 UI 목록")]
    [SerializeField] private List<WeaponSelectSlotUI> slotUIs = new List<WeaponSelectSlotUI>();


    [Header("왼쪽 미리보기")]

    [Tooltip("왼쪽 미리보기 영역 스크립트")]
    [SerializeField] private WeaponSelectPreviewUI previewUI;


    [Header("버튼")]

    [Tooltip("최종 확정 버튼")]
    [SerializeField] private Button confirmButton;

    [Tooltip("닫기 버튼")]
    [SerializeField] private Button closeButton;


    [Header("패널 전환")]

    [Tooltip("닫기 시 되돌아갈 화면 뷰")]
    [SerializeField] private LoadingScreenView screenView;


    [Header("씬 이동")]

    [Tooltip("최종 확정 후 이동할 게임 씬 이름")]
    [SerializeField] private string gameSceneName = "GameScene";


    private void Awake()
    {
        BindButtons();
    }

    private void OnEnable()
    {
        OpenPanel();
    }

    private void OpenPanel()
    {
        BuildSlots();
        ClearSelectionState();
        RefreshConfirmButton();
    }

    private void BindButtons()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(HandleClickConfirm);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(HandleClickClose);
        }
    }

    private void BuildSlots()
    {
        if (weaponCatalog == null)
        {
            Debug.LogWarning("WeaponSelectPanelUI : WeaponCatalogSO가 연결되지 않았습니다.");
            return;
        }

        for (int i = 0; i < slotUIs.Count; i++)
        {
            WeaponSelectSlotUI slotUI = slotUIs[i];

            if (slotUI == null)
            {
                continue;
            }

            WeaponCatalogEntry entry = weaponCatalog.GetEntry(i);

            if (entry == null)
            {
                slotUI.gameObject.SetActive(false);
                continue;
            }

            slotUI.gameObject.SetActive(true);
            slotUI.Setup(entry, this);
        }
    }

    public void SelectSlot(WeaponCatalogEntry entry)
    {
        if (entry == null)
        {
            return;
        }

        WeaponSelectionState.SetSelection(entry);

        if (previewUI != null)
        {
            previewUI.ShowWeapon(entry);
        }

        RefreshConfirmButton();
    }

    private void ClearSelectionState()
    {
        WeaponSelectionState.Clear();

        if (previewUI != null)
        {
            previewUI.ShowEmpty();
        }
    }

    private void RefreshConfirmButton()
    {
        if (confirmButton != null)
        {
            confirmButton.interactable = WeaponSelectionState.HasSelection;
        }
    }

    private void HandleClickConfirm()
    {
        if (!WeaponSelectionState.HasSelection)
        {
            Debug.LogWarning("WeaponSelectPanelUI : 선택된 무기가 없습니다.");
            return;
        }

        WeaponCatalogEntry selectedEntry = WeaponSelectionState.SelectedEntry;

        if (selectedEntry == null || selectedEntry.weaponPrefab == null)
        {
            Debug.LogWarning("WeaponSelectPanelUI : 선택한 무기의 프리팹이 비어 있습니다.");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    private void HandleClickClose()
    {
        ClearSelectionState();

        if (screenView != null)
        {
            screenView.Initialize();
            return;
        }

        gameObject.SetActive(false);
    }
}