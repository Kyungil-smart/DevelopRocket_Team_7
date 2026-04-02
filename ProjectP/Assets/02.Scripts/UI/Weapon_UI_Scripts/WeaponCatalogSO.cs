using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponCatalogEntry
{
    [Header("기본 정보")]
    [Tooltip("UI에 표시할 무기 이름")]
    public string weaponName;
    public int locTxtNum;

    [Tooltip("무기 종류")]
    public WeaponType weaponType;

    [Header("UI 표시 정보")]
    [Tooltip("슬롯과 왼쪽 미리보기에 사용할 무기 아이콘")]
    public Sprite weaponIcon;
}


[CreateAssetMenu(fileName = "WeaponCatalogSO", menuName = "Tool/SO/무기 카탈로그 SO")]
public class WeaponCatalogSO : ScriptableObject
{
    [Header("무기 목록")]
    [Tooltip("무기 선택 패널에 표시할 무기 목록")]
    public List<WeaponCatalogEntry> weaponEntries = new List<WeaponCatalogEntry>();

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