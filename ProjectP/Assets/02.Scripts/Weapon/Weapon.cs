using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 구현 원리 요약:
    // 프리팹 하나가 무기 1개이며 SO 데이터를 참조해서 공격한다

    // Chaebh; 이 클래스의 정체가 도데체 뭔가요???????????????????????
    
    [Header("무기 데이터")]

    [Tooltip("무기 스탯 데이터")]
    [SerializeField] private WeaponDataSO weaponData;

    [Tooltip("발사 위치")]
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;

    public void Init(WeaponDataSO data)
    {
        weaponData = data;
    }

    public void TryAttack()
    {
        // 공격 속도 제한
        if (Time.time - lastAttackTime < 1f / weaponData.attackSpeed)
            return;

        lastAttackTime = Time.time;

        // 실제 공격 실행
        // weaponData.fireStrategy.Fire(firePoint, weaponData);
    }
}