using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TestMonster : MonoBehaviour
{
    // 구현 원리 요약:
    // 데미지를 받으면 체력 감소 + 로그 출력 + 필요 시 경직 적용

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
        // 시작 시 체력 초기화
        currentHp = maxHp;

        // Rigidbody 가져오기
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
        // 이미 죽은 경우 무시
        if (currentHp <= 0) return;

        // 체력 감소
        currentHp -= damage;

        // 몬스터 이름 + 데미지 로그 출력
        Debug.Log($"[몬스터:{gameObject.name}] 데미지:{damage} / 현재HP:{currentHp} / 위치:{transform.position}");

        // 경직 처리
        if (applyStun)
        {
            Debug.Log($"[몬스터:{gameObject.name}] 경직 적용");

            if (!isStunned)
            {
                StartCoroutine(Stun());
            }
        }
        else
        {
            Debug.Log($"[몬스터:{gameObject.name}] 경직 없음");
        }

        // 사망 체크
        if (currentHp <= 0)
        {
            Die();
        }
    }


    private IEnumerator Stun()
    {
        // 구현 원리 요약:
        // 일정 시간 동안 이동/행동 제한 후 복구

        isStunned = true;

        Debug.Log($"[몬스터:{gameObject.name}] 경직 시작");

        // 물리 멈춤
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
        // 사망 로그
        Debug.Log($"[몬스터:{gameObject.name}] 사망");

        Destroy(gameObject);
    }


    public bool IsStunned()
    {
        return isStunned;
    }
}