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
    [SerializeField] private WeaponDataSO[] weapons;
    private List<WeaponBlackboard> blackboards;

    [SerializeField] private int currentIndex = 0;

    [Header("무기 장착 위치")]
    [SerializeField] private Transform weaponHolder;
    // 무기 프리팹이 붙는 위치 (플레이어)

    private WeaponBlackboard blackboard;
    private float lastAttackTime;
    private bool isReloading = false;

    // 레이저 전용 상태
    private bool isFiring = false;
    private float ammoTimer = 0f;

    private Camera cam;

    // 프리팹 관련
    private GameObject currentWeaponObj;
    private Transform currentFirePoint;

    // 무기별 탄약 유지
    private Dictionary<WeaponDataSO, int> ammoDict = new Dictionary<WeaponDataSO, int>();

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        InitAmmo();
        SetWeapon(currentIndex);
        foreach (var w in weapons)
        {
            blackboards.Add(new WeaponBlackboard(w));
        }
    }

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
        HandleWeaponSwitch();
        HandleReload();
        HandleAttack();
        RotateWeapon();
    }

    private void HandleAttack()
    {
        if (blackboard == null) return;

        int currentAmmo = ammoDict[blackboard.origin];

        // 레이저
        if (blackboard.origin.fireStrategy is LaserFireSO laser)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                if (currentAmmo <= 0)
                {
                    // 에너지 0 → 레이저 종료 + 재장전 필요 로그
                    if (isFiring)
                    {
                        laser.ResetLaser();
                        isFiring = false;
                    }

                    Debug.Log("에너지 부족 → 재장전 필요");
                    return;
                }

                isFiring = true;

                float drainPerSecond = 5f;
                ammoTimer += drainPerSecond * Time.deltaTime;

                if (ammoTimer >= 1f)
                {
                    int consume = Mathf.FloorToInt(ammoTimer);
                    currentAmmo -= consume;
                    ammoTimer -= consume;

                    if (currentAmmo < 0)
                        currentAmmo = 0;

                    ammoDict[blackboard.origin] = currentAmmo;

                    // 에너지 소모 후 0 되면 로그 출력
                    if (currentAmmo == 0)
                    {
                        Debug.Log("에너지 모두 소모 → 재장전 필요");
                    }
                }

                laser.Fire(currentFirePoint, blackboard);
            }
            else
            {
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
        if (blackboard.origin.fireStrategy is GauntletFireSO)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                TryAttack_Gauntlet();
            }
            return;
        }

        // 일반 무기
        if (Mouse.current.leftButton.isPressed)
        {
            TryAttack();
        }
    }

    private void TryAttack_Gauntlet()
    {
        float cooldown = 1f / blackboard.attackSpeed;

        if (Time.time < lastAttackTime + cooldown) return;

        lastAttackTime = Time.time;

        blackboard.origin.fireStrategy.Fire(currentFirePoint, blackboard);
    }

    private void TryAttack()
    {
        if (blackboard == null || isReloading) return;

        float cooldown = 1f / blackboard.attackSpeed;

        if (Time.time < lastAttackTime + cooldown) return;

        int currentAmmo = ammoDict[blackboard.origin];

        if (currentAmmo <= 0)
        {
            // 탄약 없으면 공격 차단 + 로그 출력
            Debug.Log("탄약 없음 → 재장전 필요");
            return;
        }

        lastAttackTime = Time.time;

        currentAmmo--;
        ammoDict[blackboard.origin] = currentAmmo;

        // 발사 후 탄약 0 되면 로그 출력
        if (currentAmmo == 0)
        {
            Debug.Log("탄창 모두 소모 → 재장전 필요");
        }

        blackboard.origin.fireStrategy.Fire(currentFirePoint, blackboard);
    }

    // 방향 계산 함수
    private Vector2 GetAimDirection()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        return (mousePos - currentFirePoint.position).normalized;
    }

    // 추가된 회전 함수
    private void RotateWeapon()
    {
        if (currentWeaponObj == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - weaponHolder.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
    }

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

        if (blackboard != null && blackboard.origin.fireStrategy is LaserFireSO laser)
        {
            laser.ResetLaser();
        }

        currentIndex = index;
        blackboard = blackboards[index];

        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
        }

        currentWeaponObj = Instantiate(
            blackboard.origin.weaponPrefab,
            weaponHolder
        );

        currentFirePoint = currentWeaponObj.transform.Find("FirePoint");

        if (currentFirePoint == null)
        {
            Debug.LogError("FirePoint 없음 → 프리팹 확인");
        }

        Debug.Log($"무기 변경: {blackboard.origin.weaponName} / 탄약: {ammoDict[blackboard.origin]}");
    }

    private void HandleReload()
    {
        if (blackboard.origin.fireStrategy is GauntletFireSO)
            return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            int currentAmmo = ammoDict[blackboard.origin];

            if (!isReloading && currentAmmo < blackboard.magazineSize)
            {
                Debug.Log("재장전 시작");

                StartCoroutine(Reload());
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        if (blackboard.origin.fireStrategy is LaserFireSO laser)
        {
            if (isFiring)
            {
                laser.ResetLaser();
                isFiring = false;
            }
        }

        yield return new WaitForSeconds(blackboard.reloadTime);

        ammoDict[blackboard.origin] = blackboard.magazineSize;

        isReloading = false;

        Debug.Log("재장전 완료");
    }
}