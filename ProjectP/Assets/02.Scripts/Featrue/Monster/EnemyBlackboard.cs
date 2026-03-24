using System;

public class EnemyBlackboard
{
    public readonly EnemyData origin;
    public int currentHp;
    
    // state 변화를 위한 flagements
    public bool isAttacking;
    public Action OnAttacked;
    public bool isDead;
    public Action OnDead;
    public bool isFollowing;
    public Action OnFollowed;
    public bool isDamaged;
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