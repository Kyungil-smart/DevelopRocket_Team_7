using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NewWeaponSystem
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] LayerMask _layerMask;
        private WeaponBlackboard _blackboard;
        private Rigidbody2D _rb;
        private Vector2 _direction;
        private bool _isFired;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
        }
        
        private void OnEnable()
        {
            StartCoroutine(LifeCourtine());
        }
        
        private IEnumerator LifeCourtine()
        {
            yield return new WaitForSeconds(5f);
            PostManager.Instance.Post(PostMessageKey.ProjectileDespawned, gameObject);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
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

        public void Fire()
        {
            _rb.linearVelocity = _direction.normalized * _speed;
        }
        public void SetData(Vector2 direction, WeaponBlackboard data)
        {
            _direction = direction;
            _blackboard = data;
        }
    }
}