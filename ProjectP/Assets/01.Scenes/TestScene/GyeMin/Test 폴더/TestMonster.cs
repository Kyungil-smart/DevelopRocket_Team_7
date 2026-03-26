using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TestMonster : MonoBehaviour
{
    [Header("몬스터 체력")]

    [Tooltip("몬스터 최대 체력")]
    [SerializeField] private int maxHp = 50;

    [Tooltip("현재 체력")]
    [SerializeField] private int currentHp;

    [Header("경직 설정")]
    [Tooltip("건틀릿에 맞았을 때만 적용할 경직 시간")]
    [SerializeField] private float stunDuration = 0.2f;

    [Tooltip("현재 경직 상태 여부")]
    [SerializeField] private bool isStunned = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
    }

    // 레이저 / 총알용 (SendMessage 대응)
    public void TakeDamage(int damage)
    {
        // 구현 원리 요약:
        // 기본 공격은 경직 없이 데미지만 적용

        TakeDamage(damage, false);
    }

    // 건틀릿 등 (경직 포함 공격)
    public void TakeDamage(int damage, bool applyStun)
    {
        if (currentHp <= 0) return;

        currentHp -= damage;

        Debug.Log($"[몬스터] 데미지 받음: {damage} / 남은 HP: {currentHp}");

        if (applyStun)
        {
            Debug.Log("[몬스터] 경직 적용됨");

            if (!isStunned)
            {
                StartCoroutine(Stun());
            }
        }
        else
        {
            Debug.Log("[몬스터] 경직 없음");
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private IEnumerator Stun()
    {
        // 구현 원리 요약:
        // 일정 시간 동안 행동 제한 후 복구

        isStunned = true;

        Debug.Log("[몬스터] 경직 상태 시작");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;

        Debug.Log("[몬스터] 경직 상태 종료");
    }

    private void Die()
    {
        Debug.Log("[몬스터] 사망");
        Destroy(gameObject);
    }

    public bool IsStunned()
    {
        return isStunned;
    }
}