using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TestEnemy : MonoBehaviour, IDamageable
{
    // IDamageable 인터페이스 기반으로 데미지 타입별 처리 + 경직 분기 처리

    [Header("몬스터 체력")]

    [Tooltip("몬스터 최대 체력 (초기 체력 값)")]
    [SerializeField] private int maxHp = 50;

    [Tooltip("현재 체력 (게임 시작 시 maxHp로 초기화됨)")]
    [SerializeField] private int currentHp;


    [Header("경직 설정")]

    [Tooltip("건틀릿 공격 등에 맞았을 때 적용되는 경직 시간")]
    [SerializeField] private float stunDuration = 0.2f;

    [Tooltip("현재 경직 상태 여부")]
    [SerializeField] private bool isStunned = false;


    private Rigidbody2D rb;


    private void Awake()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
    }


    // 인터페이스 구현
    public void TakeDamage(DamageType type, int damage)
    {
        // 데미지 타입에 따라 경직 여부 분기

        bool applyStun = false;

        switch (type)
        {
            case DamageType.Melee:
                applyStun = true;   // 건틀릿만 경직
                break;

            case DamageType.Projectile:
            case DamageType.Explosion:
            case DamageType.Hitscan:
                applyStun = false;
                break;
        }

        ApplyDamage(damage, applyStun, type);
    }


    // 기존 구조 유지용
    public void TakeDamage(int damage)
    {
        ApplyDamage(damage, false, DamageType.Projectile);
    }


    // 실제 데미지 처리 함수
    private void ApplyDamage(int damage, bool applyStun, DamageType type)
    {
        if (currentHp <= 0) return;

        currentHp -= damage;

        Debug.Log($"[몬스터:{gameObject.name}] 타입:{type} 데미지:{damage} / 현재HP:{currentHp}");

        if (applyStun)
        {
            if (!isStunned)
            {
                StartCoroutine(Stun());
            }
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }


    private IEnumerator Stun()
    {
        // 일정 시간 동안 이동 제한 후 복구

        isStunned = true;

        Debug.Log($"[몬스터:{gameObject.name}] 경직 시작");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;

        Debug.Log($"[몬스터:{gameObject.name}] 경직 종료");
    }


    private void Die()
    {
        Debug.Log($"[몬스터:{gameObject.name}] 사망");

        Destroy(gameObject);
    }


    public bool IsStunned()
    {
        return isStunned;
    }
}