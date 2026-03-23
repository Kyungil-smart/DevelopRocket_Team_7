// UTF-8
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// [구현 원리 요약]
/// 여러 투사체 각각에 무기 공격력을 전달하여 퍼지게 발사
/// </summary>
[CreateAssetMenu(menuName = "Weapon/Fire/Shotgun")]
public class ShotgunFire : WeaponFireStrategy
{
    [Header("샷건 설정")]

    [Tooltip("발사 탄 수")]
    public int pelletCount = 5;

    [Tooltip("퍼짐 각도")]
    public float spreadAngle = 30f;


    public override void Fire(Transform firePoint, WeaponDataSO data)
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 baseDir = (mousePos - firePoint.position).normalized;

        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = startAngle + (spreadAngle / (pelletCount - 1)) * i;

            Vector2 dir = Rotate(baseDir, angle);

            GameObject bullet = Instantiate(
                data.projectilePrefab,
                firePoint.position,
                Quaternion.identity
            );

            Projectile proj = bullet.GetComponent<Projectile>();
            proj.Init(dir, data.projectileSpeed, data.damage);
        }
    }

    private Vector2 Rotate(Vector2 dir, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;

        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        float x = dir.x * cos - dir.y * sin;
        float y = dir.x * sin + dir.y * cos;

        return new Vector2(x, y).normalized;
    }
}