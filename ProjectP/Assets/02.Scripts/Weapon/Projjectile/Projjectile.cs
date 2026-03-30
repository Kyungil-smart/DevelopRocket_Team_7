using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : MonoBehaviour
{
    // 방향 기반 이동 + 충돌 시 IDamageable 인터페이스로 데미지 전달

    private Vector2 direction;
    private float speed;
    private int damage;

    // 풀링용
    private float lifeTimer;

    public void Init(Vector2 dir, float spd, int dmg)
    {
        direction = dir.normalized; // 방향 정규화 (속도 일정하게 유지)
        speed = spd;
        damage = dmg;

        // 방향 기준으로 회전 적용
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 생존 시간 초기화
        lifeTimer = 5f;
    }

    private void Update()
    {
        // 방향 기준 이동
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // 생존 시간 감소
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            PostManager.Instance.Post<GameObject>(PostMessageKey.ProjectileDespawned, gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 대상이 IDamageable이면 데미지 적용

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(DamageType.Projectile, damage);

            Debug.Log($"[투사체] 데미지: {damage}");

            PostManager.Instance.Post<GameObject>(PostMessageKey.ProjectileDespawned, gameObject);
            return;
        }

        // 벽 충돌 처리
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("[투사체] 벽 충돌 → 제거");
            PostManager.Instance.Post<GameObject>(PostMessageKey.ProjectileDespawned, gameObject);
        }
    }
}