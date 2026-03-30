using System;
using UnityEngine;

public class EnemyRangeBulletAttack : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private int _damage;
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Utils.CompareLayer(collision.gameObject.layer, _layerMask))
        {
            collision.gameObject.GetComponent<IDamage>().TakeDamage(_damage);
            DespawnBullet();
        }
    }
    
    private void DespawnBullet() =>
        PostManager.Instance.Post(PostMessageKey.EnemyRangeBulletDespawned, gameObject);
}
