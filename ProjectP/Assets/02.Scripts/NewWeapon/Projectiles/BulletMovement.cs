using System.Collections;
using UnityEngine;

namespace NewWeaponSystem
{
    public class BulletMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Rigidbody2D _rb;
        private Vector2 _direction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
        }
        
        private void OnEnable() => StartCoroutine(LifeCourtine());
        
        private IEnumerator LifeCourtine()
        {
            yield return new WaitForSeconds(5f);
            PostManager.Instance.Post(PostMessageKey.ProjectileDespawned, gameObject);
        }

        public void Fire() => _rb.linearVelocity = _direction.normalized * _speed;
        public void SetDirection(Vector2 direction) =>  _direction = direction;
    }
}