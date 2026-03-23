// UTF-8
using UnityEngine;

/// <summary>
/// [구현 원리 요약]
/// 무기에서 전달받은 데미지를 사용하며 관통 처리
/// </summary>
public class SniperProjectile : MonoBehaviour
{
    private Vector2 dir;
    private float speed;
    private int damage;

    [Header("관통 설정")]

    [Tooltip("최대 관통 횟수")]
    [SerializeField] private int pierceCount = 3;

    private int hitCount = 0;


    public void Init(Vector2 direction, float spd, int dmg)
    {
        dir = direction;
        speed = spd;
        damage = dmg;

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TestMonster monster = collision.GetComponent<TestMonster>();

        if (monster != null)
        {
            monster.TakeDamage(damage);

            Debug.Log($"[스나이퍼] 데미지: {damage}");

            hitCount++;

            if (hitCount >= pierceCount)
            {
                Destroy(gameObject);
            }
        }
    }
}