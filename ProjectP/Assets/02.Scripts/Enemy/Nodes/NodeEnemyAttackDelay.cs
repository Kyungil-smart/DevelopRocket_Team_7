using UnityEngine;

public class NodeEnemyAttackDelay : EnemyBaseNode
{
    [Input] public EnemyStateConnection entry;
    [Output] public EnemyStateConnection exitToAttackPlayer;
    [Output] public EnemyStateConnection exitToDead;
    
    public override string Execute(EnemyBlackboard blackboard)
    {
        blackboard.IsAttackDelay = false;
        
        // 1. 공격하기
        if (blackboard.IsAttacking) return "exitToAttackPlayer";
        
        return null;
    }
}
