using UnityEngine;

public class PlayerWeaponInstaller : MonoBehaviour
{
    [Header("장착 위치")]

    [Tooltip("선택한 무기 프리팹을 붙일 위치")]
    [SerializeField] private Transform weaponMountPoint;


    [Header("초기화 설정")]

    [Tooltip("장착 전에 기존 자식 무기를 제거할지 여부")]
    [SerializeField] private bool clearChildrenBeforeInstall = true;


    private void Start()
    {
        InstallSelectedWeapon();
    }

    public void InstallSelectedWeapon()
    {
        if (weaponMountPoint == null)
        {
            Debug.LogWarning("PlayerWeaponInstaller : weaponMountPoint가 연결되지 않았습니다.");
            return;
        }

        if (!WeaponSelectionState.HasSelection)
        {
            Debug.LogWarning("PlayerWeaponInstaller : 선택된 무기 정보가 없습니다.");
            return;
        }

        WeaponCatalogEntry selectedEntry = WeaponSelectionState.SelectedEntry;

        if (selectedEntry == null || selectedEntry.weaponPrefab == null)
        {
            Debug.LogWarning("PlayerWeaponInstaller : 장착할 무기 프리팹이 없습니다.");
            return;
        }

        if (clearChildrenBeforeInstall)
        {
            ClearMountedChildren();
        }

        GameObject weaponInstance = Instantiate(
            selectedEntry.weaponPrefab,
            weaponMountPoint.position,
            weaponMountPoint.rotation,
            weaponMountPoint
        );

        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;
    }

    private void ClearMountedChildren()
    {
        for (int i = weaponMountPoint.childCount - 1; i >= 0; i--)
        {
            Destroy(weaponMountPoint.GetChild(i).gameObject);
        }
    }
}