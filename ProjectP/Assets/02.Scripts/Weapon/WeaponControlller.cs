using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [Header("무기 목록")]
    [SerializeField] private WeaponDataSO[] weapons;
    [SerializeField] private int currentIndex = 0;

    [Header("발사 위치")]
    [SerializeField] private Transform firePoint;

    [Header("외형")]
    [SerializeField] private SpriteRenderer weaponRenderer;

    private WeaponDataSO currentWeapon;
    private float lastAttackTime;
    private bool isReloading = false;

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

    private void HandleWeaponSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SetWeapon(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SetWeapon(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SetWeapon(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SetWeapon(3);
    }

    private void SetWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        currentIndex = index;
        currentWeapon = weapons[index];

        currentWeapon.Init();

        if (weaponRenderer != null)
            weaponRenderer.color = currentWeapon.weaponColor;
    }

    private void TryAttack()
    {
        if (currentWeapon == null || isReloading) return;

        // 공격속도 → 쿨타임 변환
        float cooldown = 1f / currentWeapon.attackSpeed;

        if (Time.time < lastAttackTime + cooldown) return;

        // 탄 없음 → 재장전
        if (currentWeapon.currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        lastAttackTime = Time.time;
        currentWeapon.currentAmmo--;

        currentWeapon.fireStrategy.Fire(firePoint, currentWeapon);
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("재장전 시작");

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.currentAmmo = currentWeapon.magazineSize;

        Debug.Log("재장전 완료");

        isReloading = false;
    }
}