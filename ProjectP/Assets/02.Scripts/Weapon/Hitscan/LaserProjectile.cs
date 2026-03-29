using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class LaserProjectile : MonoBehaviour
{
    // Raycast로 충돌 위치까지 길이를 계산하고 Sprite를 스케일로 늘려 레이저 표현 + 지속 데미지 적용

    [Header("레이저 설정")]

    [Tooltip("초당 데미지")]
    private float damagePerSecond;

    [Tooltip("최대 길이")]
    private float maxLength;

    private Vector2 direction;

    private SpriteRenderer sprite;
    private BoxCollider2D col;

    private float damageTimer; // 지속 데미지 누적용

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        // Collider는 Trigger로 사용
        col.isTrigger = true;
    }

    private void OnEnable()
    {
        // 풀에서 재사용될 때 이전 상태 초기화

        damageTimer = 0f;
    }

    public void Init(float dps, float range)
    {
        damagePerSecond = dps;
        maxLength = range;
    }

    public void Fire(Vector2 origin, Vector2 dir)
    {
        direction = dir.normalized;

        // 방향 설정
        transform.right = direction;

        // Raycast로 길이 계산
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxLength);

        float length = maxLength;

        if (hit.collider != null)
        {
            length = hit.distance;

            // 초당 데미지를 누적해서 일정 단위로 적용 (프레임 의존 제거)

            damageTimer += damagePerSecond * Time.deltaTime;

            if (damageTimer >= 1f)
            {
                int damage = Mathf.FloorToInt(damageTimer);
                damageTimer -= damage;

                hit.collider.SendMessage(
                    "TakeDamage",
                    damage,
                    SendMessageOptions.DontRequireReceiver
                );
            }
        }

        // Pivot Left 기준 → 시작점 고정
        transform.position = origin;

        // 길이 기반 스케일 (한 방향으로만 증가)
        transform.localScale = new Vector3(length, 1f, 1f);

        // Collider도 동일하게 맞춤 (Pivot Left 기준이면 offset 0)
        col.size = new Vector2(length, col.size.y);
        col.offset = new Vector2(length * 0.5f, 0f); // 중심 보정
    }

    public void StopLaser()
    {
        // 레이저 종료 시 풀로 반환

        gameObject.SetActive(false);
    }
}