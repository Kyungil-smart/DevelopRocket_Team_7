using UnityEngine;

public class NodeEnemyAttackDelay : EnemyBaseNode
{
    [Input] public EnemyStateConnection entry;
    [Output] public EnemyStateConnection exitToAttackPlayer;
    [Output] public EnemyStateConnection exitToFollowingPlayer;
    [Output] public EnemyStateConnection exitToDead;
    
    public override string Execute(EnemyBlackboard blackboard)
    {
        blackboard.IsAttackDelay = false;
        
        // 1. 공격하기
        if (blackboard.IsAttacking) return "exitToAttackPlayer";

        string transitionName = ToFollowingToPlayer(blackboard);
        if (transitionName != null) return transitionName;
        
        return null;
    }
}

