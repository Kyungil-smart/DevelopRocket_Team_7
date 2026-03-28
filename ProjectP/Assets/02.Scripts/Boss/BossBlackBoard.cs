using UnityEngine;

public class BossBlackBoard
{
    public BossData origin;
    public float currentHp;
    public float speed;
    public Vector2 bodyDirection;
    
    private bool _isInvincible = false;
    public bool IsInvincible
    {
        get { return _isInvincible; }
        set { _isInvincible = value; }
    }
    
    public BossBlackBoard(BossData origin)
    {
        this.origin = origin;
        currentHp = origin.maxHp;
        speed = origin.speed;
    }
}