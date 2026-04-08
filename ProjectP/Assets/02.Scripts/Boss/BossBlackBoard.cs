using UnityEngine;

public class BossBlackBoard
{   // 객체 내부 커뮤니케이션용 공유 데이터
    public BossData origin;
    public float currentHp;
    public float speed;
    public Vector2 bodyDirection;
    // ToDo. State Machine 까지는 안쓰니까.. 좀 더 심플하고 괜찮은 방법이 있으면 고민해보자.
    public bool IsAttacking;  
    public bool IsDead;
    

    private bool _isInvincible = false;
    public bool IsInvincible
    {
        get { return _isInvincible; }
        set { _isInvincible = value; }
    }
    public bool IsBurnning;
    
    public BossBlackBoard(BossData origin)
    {
        this.origin = origin;
        currentHp = origin.maxHp;
        speed = origin.speed;
    }
}