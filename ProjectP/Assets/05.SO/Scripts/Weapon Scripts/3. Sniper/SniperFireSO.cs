using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Weapon/Fire/Sniper")]
public class SniperFireSO : WeaponFireStrategy
{
    // 단일 고속 관통 탄 발사
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
        SniperProjectile proj = bullet.GetComponent<SniperProjectile>();
        int finalDamage = CalculateDamage(data);
        proj.Init(dir, data.projectileSpeed, finalDamage, data.pierceCount);
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
}