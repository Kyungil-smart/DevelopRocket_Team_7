using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Weapon/Fire/Laser")]
public class LaserFireSO : WeaponFireStrategy
{
    // 구현 원리 요약:
    // 차징 시간 누적 → 단계 도달 시 발사 시작 → 유지하면서 단계 상승

    private float chargeTimer = 0f;
    private int chargeLevel = 0; // 0부터 시작 (중요)

    private LaserBeam currentLaser;

    public override void Fire(Transform firePoint, WeaponDataSO data)
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        // 차징 누적
        chargeTimer += Time.deltaTime;

        int newLevel = Mathf.FloorToInt(chargeTimer / data.chargeTime);

        // 최대 단계 제한
        newLevel = Mathf.Clamp(newLevel, 0, data.maxChargeLevel);

        // 단계 상승 체크
        if (newLevel != chargeLevel)
        {
            chargeLevel = newLevel;
            Debug.Log("차징 단계 → " + chargeLevel);
        }

        // 1단계 이상부터 발사 시작
        if (chargeLevel <= 0)
        {
            return; // 아직 발사 안함 (차징 중)
        }

        // 레이저 생성 (1회만)
        if (currentLaser == null)
        {
            GameObject obj = new GameObject("LaserBeam");
            currentLaser = obj.AddComponent<LaserBeam>();
        }

        // 단계별 성능 증가
        float finalDPS = data.laserDPS * chargeLevel;
        float finalRange = data.laserRange + (chargeLevel * 2f);

        currentLaser.Init(finalDPS, chargeLevel);
        currentLaser.Fire(firePoint.position, dir, finalRange);
    }

    public void ResetLaser()
    {
        chargeTimer = 0f;
        chargeLevel = 0;

        if (currentLaser != null)
        {
            GameObject.Destroy(currentLaser.gameObject);
            currentLaser = null;
        }

        Debug.Log("레이저 초기화");
    }
}