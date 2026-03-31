using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] public int playerHp = 5; // 플레이어 HP
    [SerializeField] public float moveSpeed = 10f;  // 플레이어 기본 속도
    
    [SerializeField] public int playerLevel = 0; // 플레이어 현재 레벨
    [SerializeField] public int playerMaxLevel = 3; // 플레이어 최대 레벨
    
    [SerializeField] public int playerExp = 0; // 플레이어 현재 경험치
    [SerializeField] public int NeedLevelUpExp = 100; // 플레이어 레벨업 시 필요한 경험치량
    
}
