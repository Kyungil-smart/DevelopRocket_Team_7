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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utils.CompareLayer(other.gameObject.layer, _layerMask))
        {
            other.gameObject.GetComponent<IDamage>().TakeDamage(_damage);
            DespawnBullet();
        }
    }
    
    private void DespawnBullet() =>
        PostManager.Instance.Post(PostMessageKey.EnemyRangeBulletDespawned, gameObject);
}
