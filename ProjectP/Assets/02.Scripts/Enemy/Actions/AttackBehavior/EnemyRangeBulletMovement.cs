using System;
using UnityEngine;

public class EnemyRangeBulletMovement : MonoBehaviour
{
    [SerializeField][Range (1, 20)] private float speed;
    [SerializeField] private LayerMask _layerMask;
    private Vector2 _direction;
    private bool _isMoving;

    private void Update()
    {
        if (_isMoving) Movement();
    }

    private void OnDisable()
    {
        _isMoving = false;
    }

    private void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, _direction, speed * Time.deltaTime);
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
        _isMoving = true;
    }
}
