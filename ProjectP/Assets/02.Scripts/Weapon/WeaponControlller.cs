using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    // 무기 타입별 입력 방식 분기
    // 건틀릿 → 클릭 1회 공격 (탄창 없음)
    // 총기 → 누르고 있는 동안 연사 (탄창 있음)
    // 레이저 → 누르고 있는 동안 지속 공격 (에너지 소모)

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

    // 레이저 전용 상태
    private bool isFiring = false;
    private float ammoTimer = 0f;

    private Camera cam;

    // 무기별 탄약을 따로 저장해서 무기 변경 시에도 탄 상태 유지
    private Dictionary<WeaponDataSO, int> ammoDict = new Dictionary<WeaponDataSO, int>();

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        InitAmmo();
        SetWeapon(currentIndex);
    }

    // 시작 시 모든 무기의 탄약을 초기값으로 세팅
    private void InitAmmo()
    {
        foreach (var weapon in weapons)
        {
            if (!ammoDict.ContainsKey(weapon))
            {
                ammoDict.Add(weapon, weapon.magazineSize);
            }
        }
    }

    private void Update()
    {
        HandleWeaponSwitch(); // 무기 변경 입력
        RotateWeapon();       // 마우스 방향으로 무기 회전
        HandleReload();       // 재장전 처리
        HandleAttack();       // 공격 처리
    }

    // 공격 처리 (핵심 로직)
    private void HandleAttack()
    {
        if (currentWeapon == null) return;

        int currentAmmo = ammoDict[currentWeapon];

        // 레이저 (지속 공격)
        if (currentWeapon.fireStrategy is LaserFireSO laser)
        {
            // 버튼 누르고 있는 동안 계속 발사 + 에너지 감소
            if (Mouse.current.leftButton.isPressed)
            {
                if (currentAmmo <= 0)
                {
                    // 에너지 없으면 레이저 중지
                    if (isFiring)
                    {
                        laser.ResetLaser();
                        isFiring = false;
                    }

                    Debug.Log("레이저 에너지 없음");
                    return;
                }

                isFiring = true;

                // 초당 일정량씩 에너지 감소
                float drainPerSecond = 5f;
                ammoTimer += drainPerSecond * Time.deltaTime;

                if (ammoTimer >= 1f)
                {
                    int consume = Mathf.FloorToInt(ammoTimer);
                    currentAmmo -= consume;
                    ammoTimer -= consume;

                    if (currentAmmo < 0)
                        currentAmmo = 0;

                    ammoDict[currentWeapon] = currentAmmo;
                }

                laser.Fire(firePoint, currentWeapon);
            }
            else
            {
                // 버튼 떼면 레이저 종료
                if (isFiring)
                {
                    laser.ResetLaser();
                    isFiring = false;
                }

                ammoTimer = 0f;
            }

            return;
        }

        // 건틀릿
        if (currentWeapon.fireStrategy is GauntletFireSO)
        {
            // 구현 원리 요약:
            // 마우스를 누르고 있는 동안 계속 공격 시도 (쿨타임으로 속도 제한)

            if (Mouse.current.leftButton.isPressed)
            {
                TryAttack_Gauntlet();
            }
            return;
        }

        // 일반 총기 (연사)
        if (Mouse.current.leftButton.isPressed)
        {
            TryAttack();
        }
    }

    // 건틀릿 공격
    private void TryAttack_Gauntlet()
    {
        if (currentWeapon == null) return;

        // attackSpeed → 초당 공격 횟수 → 쿨타임으로 변환
        float cooldown = 1f / currentWeapon.attackSpeed;

        if (Time.time < lastAttackTime + cooldown) return;

        lastAttackTime = Time.time;

        // 탄약 없이 바로 공격 실행
        currentWeapon.fireStrategy.Fire(firePoint, currentWeapon);

        Debug.Log("건틀릿 공격!");
    }

    // 일반 무기 공격
    private void TryAttack()
    {
        if (currentWeapon == null || isReloading) return;

        float cooldown = 1f / currentWeapon.attackSpeed;

        if (Time.time < lastAttackTime + cooldown) return;

        int currentAmmo = ammoDict[currentWeapon];

        // 탄약 없으면 공격 불가
        if (currentAmmo <= 0)
        {
            Debug.Log("탄 없음 - R키 재장전");
            return;
        }

        lastAttackTime = Time.time;

        // 구현 원리:
        // 공격 시 탄약 감소
        currentAmmo--;
        ammoDict[currentWeapon] = currentAmmo;

        currentWeapon.fireStrategy.Fire(firePoint, currentWeapon);
    }

    // 무기 회전
    private void RotateWeapon()
    {
        // 마우스 위치 방향으로 무기 회전
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 좌우 반전 처리
        if (weaponRenderer != null)
        {
            weaponRenderer.flipY = (angle > 90f || angle < -90f);
        }
    }

    // 무기 변경
    private void HandleWeaponSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SetWeapon(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SetWeapon(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SetWeapon(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SetWeapon(3);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SetWeapon(4);
        if (Keyboard.current.digit6Key.wasPressedThisFrame) SetWeapon(5);
        if (Keyboard.current.digit7Key.wasPressedThisFrame) SetWeapon(6);
    }

    private void SetWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        // 레이저 사용 중이면 강제 종료
        if (currentWeapon != null && currentWeapon.fireStrategy is LaserFireSO laser)
        {
            laser.ResetLaser();
        }

        currentIndex = index;
        currentWeapon = weapons[index];

        // 무기 색상 변경 (임시 외형)
        if (weaponRenderer != null)
            weaponRenderer.color = currentWeapon.weaponColor;

        Debug.Log($"무기 변경: {currentWeapon.weaponName} / 남은 탄약: {ammoDict[currentWeapon]}");
    }

    // 재장전
    private void HandleReload()
    {
        // 건틀릿은 재장전 없음
        if (currentWeapon.fireStrategy is GauntletFireSO)
            return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            int currentAmmo = ammoDict[currentWeapon];

            // 탄약이 부족할 때만 재장전
            if (!isReloading && currentAmmo < currentWeapon.magazineSize)
            {
                StartCoroutine(Reload());
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("재장전 시작");

        // 레이저 사용 중이면 종료
        if (currentWeapon.fireStrategy is LaserFireSO laser)
        {
            if (isFiring)
            {
                laser.ResetLaser();
                isFiring = false;
            }
        }

        // 재장전 시간 대기
        yield return new WaitForSeconds(currentWeapon.reloadTime);

        // 탄약 풀 충전
        ammoDict[currentWeapon] = currentWeapon.magazineSize;

        Debug.Log("재장전 완료");

        isReloading = false;
    }
}
