using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponControllerTest : MonoBehaviour
{
    // 무기 프리팹을 생성/삭제해서 실제 무기를 교체한다

    [Header("무기 프리팹 목록")]

    [Tooltip("장착 가능한 무기 프리팹")]
    [SerializeField] private GameObject[] weaponPrefabs;

    [Tooltip("현재 무기 위치")]
    [SerializeField] private Transform weaponHolder;

    private GameObject currentWeaponObj;
    private Weapon currentWeapon;

    private int currentIndex = 0;

    private void Start()
    {
        EquipWeapon(0);
    }

    private void Update()
    {
        // 공격
        if (Mouse.current.leftButton.isPressed)
        {
            currentWeapon?.TryAttack();
        }

        // 무기 변경
        if (Keyboard.current.digit1Key.wasPressedThisFrame) EquipWeapon(0);

    }

    private void EquipWeapon(int index)
    {
        currentIndex = index;

        // 기존 무기 제거
        if (currentWeaponObj != null)
            Destroy(currentWeaponObj);

        // 새 무기 생성
        currentWeaponObj = Instantiate(
            weaponPrefabs[index],
            weaponHolder
        );

        currentWeapon = currentWeaponObj.GetComponent<Weapon>();
    }
}