using System;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] public int playerHp = 5; // 플레이어 HP
    [SerializeField] public float moveSpeed = 10f;  // 플레이어 기본 속도

    private void Start()
    {
        PostManager.Instance.Subscribe<PlayerStat>(PostMessageKey.PlayerStat, ChangeMovementStats);
        PostManager.Instance.Subscribe<PlayerStat>(PostMessageKey.PlayerStat, ChangeHPStats);
    }

    private void ChangeMovementStats(PlayerStat stat)
    {
        if (Mathf.Approximately(moveSpeed,moveSpeed + moveSpeed * stat.moveSpeed)) return;
        Debug.Log($"증가 전 능력치 : {moveSpeed}");
        moveSpeed += moveSpeed * stat.moveSpeed;
        Debug.Log($"증가 후 능력치 : {moveSpeed}");
    }

    private void ChangeHPStats(PlayerStat stat)
    {
        if (playerHp == playerHp + stat.playerHp) return;
        Debug.Log($"증가 전 능력치 : {playerHp}");
        playerHp += stat.playerHp;
        Debug.Log($"증가 후 능력치 : {playerHp}");
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<PlayerStat>(PostMessageKey.PlayerStat, ChangeMovementStats);
        PostManager.Instance.Unsubscribe<PlayerStat>(PostMessageKey.PlayerStat, ChangeHPStats);
    }
}
