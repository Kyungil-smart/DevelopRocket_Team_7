using UnityEngine;

[DisallowMultipleComponent]
public class TestMonster : MonoBehaviour
{
    [Header("몬스터 체력")]

    [Tooltip("몬스터 최대 체력")]
    [SerializeField] private int maxHp = 50;

    [Tooltip("현재 체력")]
    [SerializeField] private int currentHp;

    private void Awake()
    {
        // 시작 시 체력 초기화
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        // 체력 감소
        currentHp -= damage;

        Debug.Log($"[몬스터] 데미지 받음: {damage} / 남은 HP: {currentHp}");

        // 사망 처리
        if (currentHp <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("[몬스터] 사망");

        // 오브젝트 삭제
        Destroy(gameObject);
    }
}