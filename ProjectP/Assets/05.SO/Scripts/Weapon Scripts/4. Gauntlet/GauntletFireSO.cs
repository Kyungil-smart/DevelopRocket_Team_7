using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Fire/Gauntlet")]
public class GauntletFireSO : WeaponFireStrategy
{
    // 구현 원리 요약:
    // 부채꼴 범위 내 적 탐색 후 데미지 적용

    public override void Fire(Transform firePoint, WeaponBlackboard data)
    {
        Vector2 center = firePoint.position;

        // 범위 내 모든 콜라이더 탐색
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, data.origin.meleeRadius);

        foreach (var hit in hits)
        {
            TestEnemy monster = hit.GetComponent<TestEnemy>();
            if (monster == null) continue;

            // 방향 판정 (부채꼴)
            Vector2 dirToTarget = (hit.transform.position - firePoint.position).normalized;
            float angle = Vector2.Angle(firePoint.right, dirToTarget);

            if (angle <= data.origin.meleeAngle * 0.5f)
            {
                int finalDamage = CalculateDamage(data);

                monster.TakeDamage(finalDamage);

                Debug.Log($"[건틀릿] 데미지: {finalDamage}");

                // 넉백
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // 기존 속도 제거 (중요)
                    rb.linearVelocity = Vector2.zero;

                    // 강하게 밀기
                    rb.AddForce(dirToTarget * data.origin.knockbackForce, ForceMode2D.Impulse);

                    Debug.Log($"[건틀릿] 넉백 적용 → 힘: {data.origin.knockbackForce}");
                }
                else
                {
                    Debug.LogWarning("[건틀릿] Rigidbody2D 없음 → 넉백 실패");
                }
            }
            // 이펙트 생성
            SpawnEffect(firePoint, data.origin);
        }
    }
            
        
    

    private int CalculateDamage(WeaponBlackboard data)
    {
        float dmg = data.damage;

        if (Random.value < data.critRate)
        {
            dmg *= data.critMultiplier;
        }

        return Mathf.RoundToInt(dmg);
    }

    private void SpawnEffect(Transform firePoint, WeaponDataSO data)
    {
        GameObject obj = new GameObject("GauntletEffect");

        obj.transform.position = firePoint.position;
        obj.transform.rotation = firePoint.rotation;

        GauntletSwingEffect effect = obj.AddComponent<GauntletSwingEffect>();
        effect.Init(data.meleeRadius, data.meleeAngle);
    }
}