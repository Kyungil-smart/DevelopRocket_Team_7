
namespace NewWeaponSystem
{
    public struct StatusUIMsg
    {
        public int textId;
        public float attackSpeed;
        public int damage;
        public float reloadTime;
        public float critRate;
        public float critMultiplier;
    }
    
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
        private int _currentAmmo;

        public int CurrentAmmo { get => _currentAmmo; }

        public WeaponBlackboard(CommonWeaponData origin)
        {
            this.origin = origin;
            attackSpeed = origin.attackSpeed;
            damage = origin.damage;
            magazineSize = origin.magazineSize;
            reloadTime = origin.reloadTime;
            critRate = origin.critRate;
            critMultiplier = origin.critMultiplier;
            _currentAmmo = origin.magazineSize;
        }
        
        public void WasteAmmo(int count)
        {
            _currentAmmo -= count;
            PostManager.Instance.Post(PostMessageKey.MainUICurAmmo, $"{_currentAmmo} / {origin.magazineSize}");
        }

        public void RefillAmmo()
        {
            _currentAmmo = origin.magazineSize;
            PostManager.Instance.Post(PostMessageKey.MainUICurAmmo, $"{_currentAmmo} / {origin.magazineSize}");
        }
        
        public StatusUIMsg GetStatusUIMsgStruct()
        {
            return new StatusUIMsg()
            {
                textId = origin.textId,
                damage = damage,
                attackSpeed = attackSpeed,
                critRate = critRate,
                critMultiplier = critMultiplier,
                reloadTime = reloadTime
            };
        }
    }
}
