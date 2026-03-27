// UTF-8
using UnityEngine;

[DisallowMultipleComponent]
public class RubberBulletProjectile : MonoBehaviour
{
    // Trigger 기반 충돌 → 벽 반사 / 몬스터 맞으면 폭발 / 폭발 범위 데미지 적용

    private Vector2 _direction;
    private float _speed;
    private int _damage;

    private int _remainingBounce;
    private float _bounceMultiplier;
    private float _explosionRadius;

    private float _lifeTimer;

    public void Init(Vector2 dir, WeaponDataSO data)
    {
        _direction = dir.normalized; // 방향 정규화 (반사 정확도 안정화)
        _speed = data.projectileSpeed;
        _damage = data.damage;

        _remainingBounce = data.bounceCount;
        _bounceMultiplier = data.bounceDamageMultiplier;
        _explosionRadius = data.explosionRadius;

        _lifeTimer = data.explosionDelay;

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += (Vector3)(_direction * _speed * Time.deltaTime);

        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터 충돌
        TestMonster monster = collision.GetComponent<TestMonster>();

        if (monster != null)
        {
            Explode();
            return;
        }

        // 벽 반사
        if (collision.CompareTag("Wall"))
        {
            Vector2 currentPos = transform.position;
            Vector2 closestPoint = collision.ClosestPoint(currentPos);

            Vector2 normal = (currentPos - closestPoint).normalized;

            _direction = Vector2.Reflect(_direction, normal).normalized;

            _remainingBounce--;

            _damage = Mathf.RoundToInt(_damage * _bounceMultiplier);

            if (_remainingBounce < 0)
            {
                Explode();
            }
        }
    }
    

    private void Explode()
    {
        // 범위 데미지 (TestMonster 기준)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (var hit in hits)
        {
            TestMonster monster = hit.GetComponent<TestMonster>();

            if (monster != null)
            {
                monster.TakeDamage(_damage);

                Debug.Log($"[고무탄 폭발] 데미지: {_damage}");
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}