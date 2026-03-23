using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("무기 목록")]

    [Tooltip("장착 가능한 무기 리스트")]
    [SerializeField] private WeaponDataSO[] weapons;

    [Tooltip("현재 무기 인덱스")]
    [SerializeField] private int currentIndex = 0;


    [Header("발사 위치")]

    [Tooltip("총구 위치")]
    [SerializeField] private Transform firePoint;


    [Header("외형")]

    [Tooltip("무기 스프라이트 (색 변경용)")]
    [SerializeField] private SpriteRenderer weaponRenderer;


    private WeaponDataSO currentWeapon;
    private float lastAttackTime;

    private void Start()
    {
        SetWeapon(currentIndex);
    }

    private void Update()
    {
        HandleWeaponSwitch();

        if (Mouse.current.leftButton.isPressed)
        {
            TryAttack();
        }
    }

    // 구현 원리 요약:
    // 숫자 입력으로 무기 변경
    private void HandleWeaponSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SetWeapon(0);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SetWeapon(1);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            SetWeapon(2);
    }

    // 구현 원리 요약:
    // 무기 데이터 교체 + 색 변경
    private void SetWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
            return;

        currentIndex = index;
        currentWeapon = weapons[index];

        if (weaponRenderer != null)
        {
            weaponRenderer.color = currentWeapon.weaponColor;
        }
    }

    // 구현 원리 요약:
    // 현재 무기로 공격
    private void TryAttack()
    {
        if (currentWeapon == null)
            return;

        if (Time.time < lastAttackTime + currentWeapon.attackCooldown)
            return;

        lastAttackTime = Time.time;

        currentWeapon.fireStrategy.Fire(firePoint, currentWeapon);
    }
}