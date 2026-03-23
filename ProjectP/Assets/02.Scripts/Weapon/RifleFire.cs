// UTF-8
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// [구현 원리 요약]
/// 무기 데이터의 공격력을 투사체에 전달하여 발사
/// </summary>
[CreateAssetMenu(menuName = "Weapon/Fire/Rifle")]
public class RifleFire : WeaponFireStrategy
{
    public override void Fire(Transform firePoint, WeaponDataSO data)
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        GameObject bullet = Instantiate(
            data.projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile proj = bullet.GetComponent<Projectile>();
        proj.Init(dir, data.projectileSpeed, data.damage);
    }
}