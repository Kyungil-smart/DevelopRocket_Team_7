using System;

public class EnemyBlackboard
{
    public readonly EnemyData origin;
    public int currentHp;
    
    // state 변화를 위한 flagements
    private bool _isIdle;
    public bool IsIdle
    {
        get { return _isIdle; }
        set
        {
            _isIdle = value;
            if (value) OnIdle?.Invoke();
        }
    }
    public Action OnIdle;
    
    private bool _isAttackDelay;
    public bool IsAttackDelay
    {
        get { return _isAttackDelay; }
        set
        {
            _isAttackDelay = value;
            if (value) OnAttackDelay?.Invoke();
        }
    }
    public Action OnAttackDelay;
    
    private bool _isAttacking;
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set
        {
            _isAttacking = value;
            if (value) OnAttacked?.Invoke();
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
            if (value) OnDead?.Invoke();
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
            if (value) OnFollowed?.Invoke();
        }
    }
    public Action OnFollowed;
    
    // Action 을 위한 Agent
    public EnemyAgent agent;

    public EnemyBlackboard(EnemyData origin, EnemyAgent agent)
    {
        this.origin = origin;
        this.agent = agent;
    }

    public void Init()
    {
        currentHp = origin.maxHp;
    }
}