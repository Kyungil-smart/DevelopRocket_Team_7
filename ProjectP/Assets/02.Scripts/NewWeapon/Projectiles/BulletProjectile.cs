using UnityEngine;
using Random = UnityEngine.Random;

namespace NewWeaponSystem
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        private WeaponBlackboard _blackboard;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utils.CompareLayer(collision.gameObject.layer, _layerMask))
            {
                var damage = _blackboard.damage;
                if (Random.value <= _blackboard.critRate)
                {
                    // 치명타 이팩트 필요하면 추가
                    damage = Mathf.RoundToInt(damage * _blackboard.critMultiplier); 
                }
                collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(DamageType.Projectile, damage);
                PostManager.Instance.Post(PostMessageKey.ProjectileDespawned, gameObject);
            }
        }

        public void SetUpData(WeaponBlackboard data)
        {
            _blackboard = data;
        }
    }
}