using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Weapon/Fire/Shotgun")]
public class ShotgunFireSO : WeaponFireStrategy
{
    // 여러 개 투사체를 퍼짐 각도로 발사하는 구조

    public override void Fire(Transform firePoint, WeaponDataSO data)
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 baseDir = (mousePos - firePoint.position).normalized;

        int pelletCount = Mathf.Max(1, data.pelletCount);
        float spread = data.spreadAngle;

        float startAngle = -spread * 0.5f;

        for (int i = 0; i < pelletCount; i++)
        {
            float t = pelletCount == 1 ? 0.5f : (float)i / (pelletCount - 1);
            float angle = Mathf.Lerp(startAngle, -startAngle, t);

            Vector2 dir = Rotate(baseDir, angle);
            ProjectileSpwanMsg msg = new ProjectileSpwanMsg()
            {
                name = data.projectilePrefab.name,
                pos = firePoint.position,
                rot = Quaternion.identity
            };
            GameObject bullet = PostManager.Instance.Request<ProjectileSpwanMsg, GameObject>(PostMessageKey.ProjectileSpawned, msg);
            Projectile proj = bullet.GetComponent<Projectile>();
            proj.Init(dir, data.projectileSpeed, CalculateDamage(data));
        }
    }

    private int CalculateDamage(WeaponDataSO data)
    {
        float dmg = data.damage;

        // 치명타 적용
        if (Random.value < data.critRate)
        {
            dmg *= data.critMultiplier;
        }

        return Mathf.RoundToInt(dmg);
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