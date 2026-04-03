using System;
using UnityEngine;
[System.Serializable]
public struct PlayerStatMSG
{
    public int PlayerHp;
    public float moveSpeed;
}
public class PlayerStat : MonoBehaviour
{
    
    [SerializeField] private int _playerHp; // 플레이어 HP
    public int PlayerHp
    {
        get { return _playerHp; }
        set
        {
            _playerHp = value;
            Debug.Log($"my hp {value}");
            PostManager.Instance.Post(PostMessageKey.MainUIPlayerHp , value);
        }
    }
    [SerializeField] private int MAX_Hp = 5; // 플레이어 최대 HP (MAX수치값 변동 있음)

    [SerializeField] public float moveSpeed = 10f;  // 플레이어 기본 속도
    
    [SerializeField] private int _playerLevel = 0; // 플레이어 현재 레벨
    public int PlayerLevel
    {
        get { return _playerLevel; }
        set
        {
            _playerLevel = value;
            PostManager.Instance.Post(PostMessageKey.MainUIPlayerLv , value);
        }
    }
    [SerializeField] public int playerMaxLevel = 3; // 플레이어 최대 레벨
    
    [SerializeField] public int playerExp = 0; // 플레이어 현재 경험치
    [SerializeField] public int NeedLevelUpExp = 100; // 플레이어 레벨업 시 필요한 경험치량

    public int Sum_hp => MAX_Hp + _AddStat.PlayerHp; // 플레이어 기본 HP + 노드 증가 HP
    public float Sum_moveSpeed => moveSpeed+ _AddStat.moveSpeed;
    [SerializeField] public PlayerStatMSG _AddStat=new();

    private void Start()
    {
        PlayerHp = MAX_Hp;
        PlayerLevel = 0;
    }
    private void OnEnable()
    { 
        //TODo : PostMessageKey쪽에서 PlayerStat 관련 enum 추가 해야함.
        PostManager.Instance.Subscribe<PlayerStatMSG>(PostMessageKey.PlayerStat , GetStat);
        PostManager.Instance.Subscribe<int>(PostMessageKey.NodeReset, InitStat);
    }
    private void OnDisable()
    {
        //TODo : PostMessageKey쪽에서 PlayerStat 관련 enum 추가 해야함.
        PostManager.Instance.Unsubscribe<PlayerStatMSG>(PostMessageKey.PlayerStat , GetStat);
        PostManager.Instance.Unsubscribe<int>(PostMessageKey.NodeReset, InitStat);
    }
    public void GetStat(PlayerStatMSG _MSG)
    {
        /*
         이 함수는 노스시스템 쪽에서 Post로 보내서 받는 함수
          노드 시스템 쪽에서 증가시키는 스탯을 받아서 적용하는 기능이라고 보시면 된다.
          아직 미구현 
         */
        //임시
        _AddStat.PlayerHp += _MSG.PlayerHp;
        _AddStat.moveSpeed += _MSG.moveSpeed;
    }
    public void InitStat(int tmep)
    {
        _AddStat.PlayerHp = 0;
        _AddStat.moveSpeed = 0;
    }
    public void FullRecovery()
    {
        _playerHp = Sum_hp;
    }
}
