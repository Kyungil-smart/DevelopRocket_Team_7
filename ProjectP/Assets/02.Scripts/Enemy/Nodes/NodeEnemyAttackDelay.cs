using UnityEngine;

public class NodeEnemyAttackDelay : EnemyBaseNode
{
    [Input] public EnemyStateConnection entry;
    [Output] public EnemyStateConnection exitToDamaged;
    [Output] public EnemyStateConnection exitToAttackPlayer;
    
    public override string Execute(EnemyBlackboard blackboard)
    {
        return null;
    }
}
