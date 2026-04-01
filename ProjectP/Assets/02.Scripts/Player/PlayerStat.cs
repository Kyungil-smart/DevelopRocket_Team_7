using UnityEngine;

public struct PlayerStatMSG
{
    public int PlayerHp;
    public int moveSpeed;
}
public class PlayerStat : MonoBehaviour
{
    [SerializeField] public int playerHp = 5; // 플레이어 HP
    [SerializeField]private int MAX_Hp; // 플레이어 최대 HP (MAX수치값 변동 있음)

    [SerializeField] public float moveSpeed = 10f;  // 플레이어 기본 속도
    
    [SerializeField] public int playerLevel = 0; // 플레이어 현재 레벨
    [SerializeField] public int playerMaxLevel = 3; // 플레이어 최대 레벨
    
    [SerializeField] public int playerExp = 0; // 플레이어 현재 경험치
    [SerializeField] public int NeedLevelUpExp = 100; // 플레이어 레벨업 시 필요한 경험치량
    private void Awake()
    {
        MAX_Hp=playerHp;
    }
    private void OnEnable()
    { 
        //TODo : PostMessageKey쪽에서 PlayerStat 관련 enum 추가 해야함.
      //PostManager.Instance.Subscribe<PlayerStatMSG>(PostMessageKey. , GetStat);
    }
    private void OnDisable()
    {
        //TODo : PostMessageKey쪽에서 PlayerStat 관련 enum 추가 해야함.
        //PostManager.Instance.Unsubscribe<PlayerStatMSG>(PostMessageKey. , GetStat);
    }
    public void GetStat(PlayerStatMSG _MSG)
    {
       /*
        이 함수는 노스시스템 쪽에서 Post로 보내서 받는 함수
         노드 시스템 쪽에서 증가시키는 스탯을 받아서 적용하는 기능이라고 보시면 된다.
         아직 미구현 
        */
        //임시
        MAX_Hp += _MSG.PlayerHp;
    }
   
}
