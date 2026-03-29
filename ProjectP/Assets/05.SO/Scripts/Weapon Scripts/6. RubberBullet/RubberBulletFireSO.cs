using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Weapon/Fire/RubberBullet")]
public class RubberBulletFireSO : WeaponFireStrategy
{
    // 고무탄 발사 → Projectile에 반사/폭발 로직 위임

    public override void Fire(Transform firePoint, WeaponDataSO data)
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        GameObject bullet = ProjectilePoolManager.Instance.Get
        (
            "RubberBullet",
            firePoint.position,
            Quaternion.identity
        );

        RubberBulletProjectile proj = bullet.GetComponent<RubberBulletProjectile>();
        proj.Init(dir, data);
    }
}