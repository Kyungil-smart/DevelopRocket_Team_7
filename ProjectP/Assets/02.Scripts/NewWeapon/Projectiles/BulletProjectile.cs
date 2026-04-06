using UnityEngine;
using Random = UnityEngine.Random;

namespace NewWeaponSystem
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        private WeaponBlackboard _blackboard;
        private BulletAnimController _animCtrl;
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
                _rb.linearVelocity = Vector2.zero;
                _animCtrl.OnHit();
            }
        }

        public void SetUpData(WeaponBlackboard data)
        {
            _blackboard = data;
        }
    }
}