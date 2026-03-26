using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    // 구현 원리 요약:
    // Raycast로 맞은 대상에게 SendMessage로 데미지를 전달 (인터페이스 없이 처리)

    private LineRenderer line;
    private float damagePerSecond;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = 2;

        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        if (line.material == null)
        {
            line.material = new Material(Shader.Find("Sprites/Default"));
        }

        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    public void Init(float dps, int chargeLevel)
    {
        damagePerSecond = dps;

        // 단계별 체감 크게
        float width = 0.1f + (chargeLevel * 0.15f);

        line.startWidth = width;
        line.endWidth = width;
    }

    public void Fire(Vector2 origin, Vector2 dir, float range)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, range);

        Vector2 endPos = origin + dir * range;

        if (hit.collider != null)
        {
            endPos = hit.point;

            int damage = Mathf.CeilToInt(damagePerSecond * Time.deltaTime);

            hit.collider.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            hit.collider.GetComponentInParent<Transform>()?.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            Debug.Log($"레이저 히트: {hit.collider.name} / 데미지: {damage}");
        }

        line.SetPosition(0, origin);
        line.SetPosition(1, endPos);
    }
}