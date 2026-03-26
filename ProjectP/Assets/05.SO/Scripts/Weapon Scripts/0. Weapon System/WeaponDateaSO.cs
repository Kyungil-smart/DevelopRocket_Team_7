using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [Header("무기 정보")]

    [Tooltip("무기 이름")]
    public string weaponName;

    [Tooltip("공격 속도 (초당 공격 횟수)")]
    public float attackSpeed = 4f;

    [Header("공격력")]

    [Tooltip("기본 공격력")]
    public int damage = 10;

    [Header("탄창")]

    [Tooltip("탄창 크기")]
    public int magazineSize = 30;

    [Tooltip("현재 탄 수")]
    [HideInInspector] public int currentAmmo;

    [Tooltip("재장전 시간")]
    public float reloadTime = 1.5f;

    [Header("투사체")]

    [Tooltip("투사체 프리팹")]
    public GameObject projectilePrefab;

    [Tooltip("투사체 속도")]
    public float projectileSpeed = 15f;

    [Tooltip("투사체 개수")]
    public int projectileCount = 1;

    [Header("기타 스탯")]

    [Tooltip("튕김 횟수")]
    public int bounceCount = 0;

    [Tooltip("이동 속도 증가")]
    public float moveSpeedBonus = 0;

    [Tooltip("회피율")]
    public float dodgeRate = 5f;

    [Tooltip("치명타 확률")]
    public float critRate = 0.05f;

    [Tooltip("치명타 배율")]
    public float critMultiplier = 1.5f;

    [Header("외형")]

    [Tooltip("무기 색상")]
    public Color weaponColor = Color.white;

    [Header("공격 방식")]

    [Tooltip("공격 로직")]
    public WeaponFireStrategy fireStrategy;

    [Header("샷건 설정")]

    [Tooltip("샷건 펠렛 개수")]
    public int pelletCount = 5;

    [Tooltip("퍼짐 각도")]
    public float spreadAngle = 30f;

    [Header("스나이퍼 설정")]

    [Tooltip("관통 횟수")]
    public int pierceCount = 3;

    [Header("근접 공격 (건틀릿)")]

    [Tooltip("공격 반경")]
    public float meleeRadius = 2.5f;

    [Tooltip("공격 각도")]
    public float meleeAngle = 90f;

    [Tooltip("넉백 힘")]
    public float knockbackForce = 5f;

    [Tooltip("히트스탑 시간")]
    public float hitStopTime = 0.05f;

    [Header("레이저 설정")]

    [Tooltip("레이저 최대 거리")]
    public float laserRange = 10f;

    [Tooltip("초당 데미지 (DPS)")]
    public float laserDPS = 10f;

    [Tooltip("차징 단계 상승 시간")]
    public float chargeTime = 2f;

    [Tooltip("최대 차징 단계")]
    public int maxChargeLevel = 3;

    // 초기화
    public void Init()
    {
        currentAmmo = magazineSize;
    }
}
