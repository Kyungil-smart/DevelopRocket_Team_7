using UnityEngine;

public class NodeEnemyAttackDelay : EnemyBaseNode
{
    [Input] public EnemyStateConnection entry;
    [Output] public EnemyStateConnection exitToAttackPlayer;
    [Output] public EnemyStateConnection exitToDead;
    
    public override string Execute(EnemyBlackboard blackboard)
    {
        Debug.Log("자, 이제 어금니 꽉 깨물거릐");
        blackboard.IsAttackDelay = false;
        
        // 1. 공격하기
        if (blackboard.IsAttacking) return "exitToAttackPlayer";
        
        return null;
    }
}
