using UnityEngine;

public class LoadingScreenView : MonoBehaviour
{
    [Header("패널 참조")]

    [Tooltip("처음에 보이는 타이틀 패널")]
    [SerializeField] private GameObject titlePanel;

    [Tooltip("로딩 중에 보이는 로딩 패널")]
    [SerializeField] private GameObject loadingPanel;

    [Tooltip("로딩 완료 후 열릴 무기 선택 패널")]
    [SerializeField] private GameObject weaponSelectPanel;


    public void Initialize()
    {
        if (titlePanel != null)
        {
            titlePanel.SetActive(true);
        }

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        if (weaponSelectPanel != null)
        {
            weaponSelectPanel.SetActive(false);
        }
    }


    public void ShowLoading()
    {
        if (titlePanel != null)
        {
            titlePanel.SetActive(false);
        }

        if (weaponSelectPanel != null)
        {
            weaponSelectPanel.SetActive(false);
        }

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
    }


    public void ShowWeaponSelect()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        if (weaponSelectPanel != null)
        {
            weaponSelectPanel.SetActive(true);
        }
    }
}