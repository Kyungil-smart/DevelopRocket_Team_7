using UnityEngine;

public class EnemyDamaged : IEnemyDamagable, INeedBlackboard
{   // State 독립 트리거. 
    private EnemyBlackboard blackboard;
    
    public void TakeDamage(int damage)
    {
        // ToDo. 필요시 유저 무기에 의한 피격 계산 추가 필요
        blackboard.currentHp -= damage;
        
        // 유저가 멀리 있는 경우
        float distance = Mathf.Abs(Vector2.Distance(
            blackboard.agent.target.transform.position,
            blackboard.agent.transform.position));
        
        if (blackboard.origin.attackRange > distance && blackboard.origin.detectRadius <= distance)
        {
            blackboard.IsFollowing = true;
        }
        
        // 맞다가 hp 가 없으면 Die.
        if (blackboard.currentHp <= 0)
        {
            blackboard.IsDead = true;
        }
    }

    public void SetBlackboard(EnemyBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }
}