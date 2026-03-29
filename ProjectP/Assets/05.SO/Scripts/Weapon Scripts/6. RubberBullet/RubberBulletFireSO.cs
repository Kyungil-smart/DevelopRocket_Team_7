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

        ProjectileSpwanMsg msg = new ProjectileSpwanMsg()
        {
            name = data.projectilePrefab.name,
            pos = firePoint.position,
            rot = Quaternion.identity
        };
        GameObject bullet = PostManager.Instance.Request<ProjectileSpwanMsg, GameObject>(PostMessageKey.ProjectileSpawned, msg);
        RubberBulletProjectile proj = bullet.GetComponent<RubberBulletProjectile>();
        proj.Init(dir, data);
    }
}