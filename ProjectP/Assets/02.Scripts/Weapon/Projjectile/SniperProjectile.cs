using UnityEngine;

public class SniperProjectile : MonoBehaviour
{
    // 관통형 투사체 → IDamageable 대상에게 Projectile 타입 데미지 적용

    private Vector2 dir;
    private float speed;
    private int damage;

    private int pierceCount;
    private int hitCount = 0;

    // 풀링용 
    private float lifeTimer;

    public void Init(Vector2 direction, float spd, int dmg, int pierce)
    {
        dir = direction.normalized;
        speed = spd;
        damage = dmg;
        pierceCount = pierce;

        // 발사 방향 회전 적용
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        lifeTimer = 5f;
    }

    private void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            PostManager.Instance.Post<GameObject>(PostMessageKey.ProjectileDespawned, gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // IDamageable 대상은 관통하며 데미지 적용, 벽은 즉시 제거

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(DamageType.Projectile, damage);

            Debug.Log($"[스나이퍼] 데미지: {damage}");

            hitCount++;

            if (hitCount >= pierceCount)
            {
                PostManager.Instance.Post<GameObject>(PostMessageKey.ProjectileDespawned, gameObject);
            }

            return;
        }

        // 벽 충돌 처리
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("[스나이퍼] 벽 충돌 → 제거");

            PostManager.Instance.Post<GameObject>(PostMessageKey.ProjectileDespawned, gameObject);
        }
    }
}