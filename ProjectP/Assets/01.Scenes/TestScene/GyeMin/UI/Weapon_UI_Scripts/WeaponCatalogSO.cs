using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None,
    Rifle,
    Shotgun,
    Sniper,
    Bouncy,
    gauntlet,
    Laser,
}

[System.Serializable]
public class WeaponCatalogEntry
{
    [Header("기본 정보")]

    [Tooltip("무기 고유 ID")]
    public string weaponId;

    [Tooltip("UI에 표시할 무기 이름")]
    public string weaponName;

    [Tooltip("무기 종류")]
    public WeaponType weaponType;


    [Header("UI 표시 정보")]

    [Tooltip("슬롯과 왼쪽 미리보기에 사용할 무기 아이콘")]
    public Sprite weaponIcon;

    [TextArea(2, 5)]
    [Tooltip("왼쪽 설명 영역에 표시할 무기 설명")]
    public string description;


    [Header("실제 연결 데이터")]

    [Tooltip("게임 시작 후 플레이어에게 장착할 무기 프리팹")]
    public GameObject weaponPrefab;

    [Tooltip("무기 데이터 SO")]
    public ScriptableObject weaponData;
}

[CreateAssetMenu(
    fileName = "WeaponCatalogSO",
    menuName = "Tool/SO/무기 카탈로그 SO")]
public class WeaponCatalogSO : ScriptableObject
{
    [Header("무기 목록")]

    [Tooltip("무기 선택 패널에 표시할 무기 목록")]
    [SerializeField] private List<WeaponCatalogEntry> weaponEntries = new List<WeaponCatalogEntry>();


    public int Count => weaponEntries.Count;

    public WeaponCatalogEntry GetEntry(int index)
    {
        if (index < 0 || index >= weaponEntries.Count)
        {
            return null;
        }

        return weaponEntries[index];
    }
}