using System;
using System.Collections;
using UnityEngine;

namespace NewWeaponSystem
{
    public class StraightMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] LayerMask _layerMask;
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
                PostManager.Instance.Post(PostMessageKey.ProjectileDespawned, gameObject);
        }

        public void Fire()
        {
            _rb.linearVelocity = _direction.normalized * _speed;
        }
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
    }
}