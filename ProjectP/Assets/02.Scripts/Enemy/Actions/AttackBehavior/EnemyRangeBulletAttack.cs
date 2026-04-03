using System;
using UnityEngine;

public class EnemyRangeBulletAttack : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private bool isDamaged;
    
    private int _damage;
    public void SetDamage(int damage)
    {
        isDamaged = false;
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utils.CompareLayer(other.gameObject.layer, _layerMask))
        {
            if (!isDamaged)
            {
                other.gameObject.GetComponent<IDamage>().TakeDamage(_damage);
                isDamaged = true;
            }
            DespawnBullet();
        }
    }
    
    private void DespawnBullet() =>
        PostManager.Instance.Post(PostMessageKey.EnemyRangeBulletDespawned, gameObject);
}
