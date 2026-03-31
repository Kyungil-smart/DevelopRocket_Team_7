using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon/Sniper")]
public class SniperData : ScriptableObject
{
    [Header("스나이퍼 설정")]
    [Tooltip("관통 횟수")] public int pierceCount = 3;
    // 적을 몇 번까지 관통할 수 있는지
}