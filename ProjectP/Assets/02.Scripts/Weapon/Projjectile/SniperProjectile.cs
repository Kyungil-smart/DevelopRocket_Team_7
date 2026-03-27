using UnityEngine;

public class SniperProjectile : MonoBehaviour
{
    // 구현 원리 요약:
    // 일정 횟수까지 관통하면서 데미지 적용

    private Vector2 dir;
    private float speed;
    private int damage;

    private int pierceCount;
    private int hitCount = 0;

    public void Init(Vector2 direction, float spd, int dmg, int pierce)
    {
        dir = direction;
        speed = spd;
        damage = dmg;
        pierceCount = pierce;

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