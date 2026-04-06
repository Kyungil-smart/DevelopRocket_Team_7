using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [Header("무기 정보")]

    [Tooltip("무기 이름")]
    public string weaponName;
    public int textId;

    [Tooltip("무기 외형")]
    public Sprite weaponSprite;
    // 무기 프리팹 없이도 외형을 표현할 수 있는 기본 데이터
    // WeaponController에서 SpriteRenderer에 적용해서 사용

    [Tooltip("무기 프리팹")]
    public GameObject weaponPrefab;
    // 실제 장착될 무기 오브젝트
    // SpriteRenderer, FirePoint 포함된 프리팹

    [Tooltip("공격 속도 (초당 공격 횟수)")]
    public float attackSpeed = 4f;
    // 초당 몇 번 공격 가능한지 (쿨타임 = 1 / attackSpeed)

    [Header("공격력")]

    [Tooltip("기본 공격력")]
    public int damage = 10;
    // 모든 무기 기본 데미지 기준값
    
    [Header("탄창")]

    [Tooltip("탄창 크기")]
    public int magazineSize = 30;

    [Tooltip("현재 탄 수")]
    [HideInInspector] public int currentAmmo;
    // 런타임에서만 사용 (무기 교체 시 유지되는 값)

    [Tooltip("재장전 시간")]
    public float reloadTime = 1.5f;
    // R키 입력 시 이 시간만큼 대기 후 탄약 채움

    [Header("투사체")]

    [Tooltip("투사체 프리팹")]
    public GameObject projectilePrefab;
    // 실제 발사되는 총알 프리팹
    // 반드시 Projectile 스크립트가 붙어 있어야 함

    [Tooltip("투사체 속도")]
    public float projectileSpeed = 15f;

    [Tooltip("투사체 개수")]
    public int projectileCount = 1;

    [Header("기타 스탯")]

    [Tooltip("이동 속도 증가")]
    public float moveSpeedBonus = 0;

    [Tooltip("회피율")]
    public float dodgeRate = 5f;

    [Tooltip("치명타 확률")]
    public float critRate = 0.05f;

    [Tooltip("치명타 배율")]
    public float critMultiplier = 1.5f;
    // 데미지 계산 시
    // if (Random < critRate) damage *= critMultiplier

    [Header("외형")]

    [Tooltip("무기 색상")]
    public Color weaponColor = Color.white;
    // 현재 임시 외형 (Sprite 색상 변경용)
    // 이후 프리팹 방식으로 넘어가면 보조 역할

    [Header("공격 방식")]

    [Tooltip("공격 로직")]
    public WeaponFireStrategy fireStrategy;
    // RifleFireSO, ShotgunFireSO 같은 공격 방식 연결
    // 무기마다 다른 공격 구조를 가지도록 설계된 핵심

    [Header("샷건 설정")]

    [Tooltip("샷건 펠렛 개수")]
    public int pelletCount = 5;

    [Tooltip("퍼짐 각도")]
    public float spreadAngle = 30f;
    // 여러 방향으로 퍼지는 각도 (샷건 전용)

    [Header("스나이퍼 설정")]

    [Tooltip("관통 횟수")]
    public int pierceCount = 3;
    // 적을 몇 번까지 관통할 수 있는지

    [Header("근접 공격 (건틀릿)")]

    [Tooltip("공격 반경")]
    public float meleeRadius = 2.5f;

    [Tooltip("공격 각도")]
    public float meleeAngle = 90f;

    [Tooltip("넉백 힘")]
    public float knockbackForce = 5f;

    [Tooltip("히트스탑 시간")]
    public float hitStopTime = 0.05f;
    // 공격 시 잠깐 멈추는 연출 (타격감)

    [Header("레이저 설정")]

    [Tooltip("레이저 최대 거리")]
    public float laserRange = 10f;

    [Tooltip("초당 데미지 (DPS)")]
    public float laserDPS = 10f;

    [Tooltip("차징 단계 상승 시간")]
    public float chargeTime = 2f;

    [Tooltip("최대 차징 단계")]
    public int maxChargeLevel = 3;
    // 홀드 공격 무기용 (레이저)

    [Header("고무탄 옵션")]

    [Tooltip("최대 튕김 횟수")]
    public int bounceCount = 2;

    [Tooltip("튕길 때 데미지 증가 배율")]
    public float bounceDamageMultiplier = 1.2f;

    [Tooltip("폭발 반경")]
    public float explosionRadius = 2f;

    [Tooltip("자동 폭발 시간")]
    public float explosionDelay = 3f;
    // 일정 시간 후 자동 폭발

    // 초기화
    public void Init()
    {
        currentAmmo = magazineSize;
    }
}