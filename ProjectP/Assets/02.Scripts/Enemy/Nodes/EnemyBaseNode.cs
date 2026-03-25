using UnityEngine;
using XNode;


public abstract class EnemyBaseNode : Node
{
    public abstract string Execute(EnemyBlackboard blackboard);

    protected static string ToFollowingToPlayer(EnemyBlackboard blackboard)
    {
        if (blackboard.agent.target == null) return null;
        float distance = Mathf.Abs(Vector2.Distance(
            blackboard.agent.target.transform.position,
            blackboard.agent.transform.position));
        
        if (blackboard.origin.attackRange > distance && blackboard.origin.detectRadius <= distance)
        {
            blackboard.IsFollowing = true;
            return "exitToFollowing";
        }
        return null;
    }
    
    protected static string ToAttackDelay(EnemyBlackboard blackboard)
    {
        if (blackboard.agent.target == null) return null;
        float distance = Mathf.Abs(Vector2.Distance(
            blackboard.agent.target.transform.position,
            blackboard.agent.transform.position));
        
        if (blackboard.origin.attackRange <= distance)
        {
            blackboard.IsAttacking = true;
            return "exitToAttackDelay";
        }
        return null;
    }
}