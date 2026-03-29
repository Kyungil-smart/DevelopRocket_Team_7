using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Weapon/Fire/Rifle")]
public class RifleFireSO : WeaponFireStrategy
{
    // 투사체 개수만큼 반복 발사

    public override void Fire(Transform firePoint, WeaponDataSO data)
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        for (int i = 0; i < data.projectileCount; i++)
        {
            GameObject bullet = ProjectilePoolManager.Instance.Get
            (
                "RifleBullet",
                firePoint.position,
                Quaternion.identity
            );

            Projectile proj = bullet.GetComponent<Projectile>();
            proj.Init(dir, data.projectileSpeed, CalculateDamage(data));
        }
    }

    private int CalculateDamage(WeaponDataSO data)
    {
        float finalDamage = data.damage;

        // 치명타 계산
        if (Random.value < data.critRate)
        {
            finalDamage *= data.critMultiplier;
        }

        return Mathf.RoundToInt(finalDamage);
    }
}