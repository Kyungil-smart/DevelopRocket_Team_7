using UnityEngine;
using Random = UnityEngine.Random;

namespace NewWeaponSystem
{
    public class SniperProjectile : MonoBehaviour
    {
        [SerializeField] private SniperData _sniperData;
        [SerializeField] private LayerMask _layerMask;
        private WeaponBlackboard _blackboard;
        private int _hitCount;
        
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
                // 관통 계산
                _hitCount++;
                if (_sniperData.pierceCount <= _hitCount) PostManager.Instance.Post(PostMessageKey.ProjectileDespawned, gameObject);
            }
        }
        
        public void SetUpData(WeaponBlackboard data)
        {
            Debug.Log("Setting up projectile");
            _blackboard = data;
            Debug.Log("Set blackboard data");
            _hitCount = 0;
            Debug.Log("_hitCount: 0");
        }
    }
}