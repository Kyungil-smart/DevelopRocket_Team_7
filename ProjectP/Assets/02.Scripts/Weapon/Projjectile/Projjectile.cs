using UnityEngine;

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