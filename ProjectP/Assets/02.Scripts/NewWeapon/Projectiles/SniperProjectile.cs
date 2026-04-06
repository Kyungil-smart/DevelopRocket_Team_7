using UnityEngine;
using Random = UnityEngine.Random;

namespace NewWeaponSystem
{
    public class SniperProjectile : MonoBehaviour
    {
        [SerializeField] private SniperData _sniperData;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private AudioClip _hitSound;
        private BulletAnimController _animCtrl;
        private WeaponBlackboard _blackboard;
        private int _hitCount;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _animCtrl = GetComponent<BulletAnimController>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
        }
        
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
                if (_sniperData.pierceCount > _hitCount) AudioManager.Instance.OnSfxPlayOnShot(_hitSound);
                    
                if (_sniperData.pierceCount <= _hitCount)
                {
                    _rb.linearVelocity = Vector2.zero;
                    _animCtrl.OnHit();
                }
            }
        }
        
        public void SetUpData(WeaponBlackboard data)
        {
            _blackboard = data;
            _hitCount = 0;
        }
    }
}