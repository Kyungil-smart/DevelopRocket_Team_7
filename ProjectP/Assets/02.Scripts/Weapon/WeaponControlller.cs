using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    // 구현 원리 요약:
    // 마우스 방향을 기준으로 무기 전체를 회전시키고
    // 탄이 없을 경우 R키 입력으로만 재장전하도록 처리

    [Header("무기 목록")]
    [Tooltip("장착 가능한 무기 리스트")]
    [SerializeField] private WeaponDataSO[] weapons;

    [Tooltip("현재 무기 인덱스")]
    [SerializeField] private int currentIndex = 0;


    [Header("발사 위치")]
    [Tooltip("총구 위치")]
    [SerializeField] private Transform firePoint;


    [Header("외형")]
    [Tooltip("무기 스프라이트")]
    [SerializeField] private SpriteRenderer weaponRenderer;


    private WeaponDataSO currentWeapon;
    private float lastAttackTime;
    private bool isReloading = false;

    private Camera cam;


    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        SetWeapon(currentIndex);
    }

    private void Update()
    {
        HandleWeaponSwitch();

        RotateWeapon();

        // R키 입력 시 재장전 실행
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (!isReloading && currentWeapon.currentAmmo < currentWeapon.magazineSize)
            {
                StartCoroutine(Reload());
            }
        }

        if (Mouse.current.leftButton.isPressed)
        {
            TryAttack();
        }
    }


    private void RotateWeapon()
    {
        // 마우스 위치 가져오기
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        // 방향 계산
        Vector2 dir = mousePos - transform.position;

        // 각도 계산
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 무기 전체 회전
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 스프라이트 뒤집기 처리
        if (weaponRenderer != null)
        {
            if (angle > 90f || angle < -90f)
                weaponRenderer.flipY = true;
            else
                weaponRenderer.flipY = false;
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

        // 탄 없음 → 공격 불가
        if (currentWeapon.currentAmmo <= 0)
        {
            Debug.Log("탄 없음 - R키로 재장전");
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