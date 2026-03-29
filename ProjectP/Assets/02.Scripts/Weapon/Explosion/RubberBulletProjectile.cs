using UnityEngine;

[DisallowMultipleComponent]
public class RubberBulletProjectile : MonoBehaviour
{
    // 벽 반사 → 조건 충족 시 폭발 → 범위 내 IDamageable 대상에게 Explosion 데미지 전달

    private Vector2 _direction;
    private float _speed;
    private int _damage;
    private Vector2 _velocity;
    private int _remainingBounce;
    private float _bounceMultiplier;
    private float _explosionRadius;

    private float _lifeTimer;

    public void Init(Vector2 dir, WeaponDataSO data)
    {
        _direction = dir.normalized;
        _speed = data.projectileSpeed;
        _damage = data.damage;

        _remainingBounce = data.bounceCount;
        _bounceMultiplier = data.bounceDamageMultiplier;
        _explosionRadius = data.explosionRadius;

        _lifeTimer = data.explosionDelay;
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
        // 충돌 대상이 IDamageable이면 폭발 처리

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Explode();
            return;
        }

        // 벽 충돌 시 반사
        if (collision.CompareTag("Wall"))
        {
            Vector2 currentPos = transform.position;
            Vector2 closestPoint = collision.ClosestPoint(currentPos);

            Vector2 normal = (currentPos - closestPoint).normalized;

            _direction = Vector2.Reflect(_direction, normal).normalized;

            _remainingBounce--;

            // 반사 시 데미지 증가
            _damage = Mathf.RoundToInt(_damage * _bounceMultiplier);

            if (_remainingBounce < 0)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        // 폭발 범위 내 모든 IDamageable 대상에게 Explosion 타입 데미지 적용

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(DamageType.Explosion, _damage);
            }
        }

        PostManager.Instance.Post<GameObject>(PostMessageKey.ProjectileDespawned, gameObject);
    }
}