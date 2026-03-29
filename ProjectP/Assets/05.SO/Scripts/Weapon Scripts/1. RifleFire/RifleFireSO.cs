using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Weapon/Fire/Rifle")]
public class RifleFireSO : WeaponFireStrategy
{
    // 투사체 개수만큼 반복 발사

    public override void Fire(Transform firePoint, WeaponBlackboard data)
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        ProjectileSpwanMsg msg = new ProjectileSpwanMsg()
        {
            name = data.origin.projectilePrefab.name,
            pos = firePoint.position,
            rot = Quaternion.identity
        };
        
        for (int i = 0; i < data.origin.projectileCount; i++)
        {
            GameObject bullet = PostManager.Instance.Request<ProjectileSpwanMsg, GameObject>(PostMessageKey.ProjectileSpawned, msg);
            Projectile proj = bullet.GetComponent<Projectile>();
            proj.Init(dir, data.origin.projectileSpeed, CalculateDamage(data));
        }
    }

    private int CalculateDamage(WeaponBlackboard data)
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