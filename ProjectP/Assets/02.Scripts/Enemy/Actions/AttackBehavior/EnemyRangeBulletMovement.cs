using System;
using UnityEngine;

public class EnemyRangeBulletMovement : MonoBehaviour
{
    [SerializeField][Range (1, 20)] private float speed;
    [SerializeField] private LayerMask _layerMask;
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private bool _isMoving;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Utils.CompareLayer(collision.gameObject.layer, _layerMask))
            DespawnBullet();
    }

    private void DespawnBullet() =>
        PostManager.Instance.Post(PostMessageKey.EnemyRangeBulletDespawned, gameObject);

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void FireBullet()
    {
        _rb.WakeUp();
        Debug.Log($"fire bullet: {_direction.normalized * speed}");
        _rb.linearVelocity = _direction.normalized * speed;
    }
}
