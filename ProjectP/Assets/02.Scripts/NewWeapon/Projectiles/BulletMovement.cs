using System.Collections;
using UnityEngine;

namespace NewWeaponSystem
{
    public class BulletMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _layerMask;
        private Rigidbody2D _rb;
        private Vector2 _direction;
        private BulletAnimController _animCtrl;

        private void Awake()
        {
            _animCtrl = GetComponent<BulletAnimController>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
        }
        
        private void OnEnable() => StartCoroutine(LifeCourtine());
        
        private IEnumerator LifeCourtine()
        {
            yield return new WaitForSeconds(5f);
            PostManager.Instance.Post(PostMessageKey.ProjectileDespawned, gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utils.CompareLayer(collision.gameObject.layer, _layerMask))
            {
                _rb.linearVelocity = Vector2.zero;
                _animCtrl.OnHit();
            }
        }

        public void Fire()
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            _rb.linearVelocity = _direction.normalized * _speed;
        }

        public void SetDirection(Vector2 direction) =>  _direction = direction;
    }
}