using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon/Common")]
public class CommonWeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType WeaponType;
    
    [Header("기본 무기 스탯")]
    public float attackSpeed = 4f;
    public int damage = 10;
    public float critRate = 0.05f;
    public float critMultiplier = 1.5f;

    [Header("탄창")]
    public int magazineSize = 30;
    public float reloadTime = 1.5f;
    // R키 입력 시 이 시간만큼 대기 후 탄약 채움

    [Header("투사체")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;
    public int projectileCount = 1;
}