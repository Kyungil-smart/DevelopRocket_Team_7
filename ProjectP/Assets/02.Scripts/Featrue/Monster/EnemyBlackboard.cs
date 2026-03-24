using System;

public class EnemyBlackboard
{
    public readonly EnemyData origin;
    public int currentHp;
    
    // state 변화를 위한 flagements
    private bool _isAttacking;
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set
        {
            _isAttacking = value;
            OnAttacked?.Invoke();
        }
    }
    public Action OnAttacked;

    private bool _isDead;
    public bool IsDead
    {
        get { return _isDead; }
        set
        {
            _isDead = value;
            OnDead?.Invoke();
        }
    }
    public Action OnDead;

    private bool _isFollowing;
    public bool IsFollowing
    {
        get { return _isFollowing; }
        set
        {
            _isFollowing = value;
            OnFollowed?.Invoke();
        }
    }
    public Action OnFollowed;
    
    private bool _isDamaged;
    public bool IsDamaged
    {
        get { return _isDamaged; }
        set
        {
            _isDamaged = value;
            OnDamaged?.Invoke();
        }
    }
    public Action OnDamaged;
    
    // Action 을 위한 Agent
    public EnemyAgent agent;

    public EnemyBlackboard(EnemyData origin, EnemyAgent agent)
    {
        this.origin = origin;
        currentHp = origin.maxHp;
        
        this.agent = agent;
    }
}