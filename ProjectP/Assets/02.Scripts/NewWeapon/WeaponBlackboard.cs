
namespace NewWeaponSystem
{
    public class WeaponBlackboard
    {
        // SO Origin Data
        public CommonWeaponData origin;
    
        public float attackSpeed;
        public int damage;
        public int magazineSize;
        public float reloadTime;
        public float critRate;
        public float critMultiplier;
        public int currentAmmo;

        public WeaponBlackboard(CommonWeaponData origin)
        {
            this.origin = origin;
            attackSpeed = origin.attackSpeed;
            damage = origin.damage;
            magazineSize = origin.magazineSize;
            reloadTime = origin.reloadTime;
            critRate = origin.critRate;
            critMultiplier = origin.critMultiplier;
            currentAmmo = origin.magazineSize;
        }
    }
}
