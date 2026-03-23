using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [Header("무기 정보")]

    [Tooltip("무기 이름")]
    public string weaponName;

    [Tooltip("공격 쿨타임")]
    public float attackCooldown = 0.2f;


    [Header("공격력")]

    [Tooltip("무기 기본 공격력")]
    public int damage = 10;


    [Header("투사체")]

    [Tooltip("투사체 프리팹")]
    public GameObject projectilePrefab;

    [Tooltip("투사체 속도")]
    public float projectileSpeed = 15f;


    [Header("외형 설정")]

    [Tooltip("무기 색상 (임시 외형)")]
    public Color weaponColor = Color.white;


    [Header("공격 방식")]

    [Tooltip("공격 로직")]
    public WeaponFireStrategy fireStrategy;
}