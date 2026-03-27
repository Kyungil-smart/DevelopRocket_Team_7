using UnityEngine;
using XNode;


public abstract class EnemyBaseNode : Node
{
    public abstract string Execute(EnemyBlackboard blackboard);

    protected static string ToIdle(EnemyBlackboard blackboard)
    {
        float distance = Mathf.Abs(Vector2.Distance(
            blackboard.targetPosition, blackboard.agent.transform.position));
        if (blackboard.origin.detectRadius < distance)
        {
            blackboard.IsIdle = true;
            return "exitToIdle";
        }
        return null;
    }
    
    protected static string ToFollowingToPlayer(EnemyBlackboard blackboard)
    {
        float distance = Mathf.Abs(Vector2.Distance(
            blackboard.targetPosition, blackboard.agent.transform.position));
        if (blackboard.origin.attackRange < distance && blackboard.origin.detectRadius >= distance)
        {
            blackboard.IsFollowing = true;
            return "exitToFollowingPlayer";
        }
        return null;
    }
    
    protected static string ToAttackDelay(EnemyBlackboard blackboard)
    {
        float distance = Mathf.Abs(Vector2.Distance(
            blackboard.targetPosition,
            blackboard.agent.transform.position));
        
        if (blackboard.origin.attackRange >= distance)
        {
            blackboard.IsAttacking = true;
            return "exitToAttackDelay";
        }
        return null;
    }
}