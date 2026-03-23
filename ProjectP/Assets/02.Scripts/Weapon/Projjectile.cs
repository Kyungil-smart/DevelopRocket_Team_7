// UTF-8
using UnityEngine;

/// <summary>
/// [구현 원리 요약]
/// 무기 데이터에서 전달받은 공격력을 사용하여 몬스터에게 데미지 적용
/// </summary>
public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;

    public void Init(Vector2 dir, float spd, int dmg)
    {
        direction = dir;
        speed = spd;
        damage = dmg;

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TestMonster monster = collision.GetComponent<TestMonster>();

        if (monster != null)
        {
            monster.TakeDamage(damage);

            Debug.Log($"[투사체] 데미지: {damage}");

            Destroy(gameObject);
        }
    }
}