using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyDamaged : EnemyBaseNode 
{
    [Input] public EnemyStateConnection entry;
    [Output] public EnemyStateConnection exitToDead;
    [Output] public EnemyStateConnection exitToAttackPlayer;
    [Output] public EnemyStateConnection exitToFollowingPlayer;
    public override string Execute(EnemyBlackboard blackboard)
    {
        Debug.Log("지금은 적이 두두려 맞고 있습니다?");
        if (blackboard.isDead) return "exitToDead";
        if (blackboard.isFollowing) return "exitToFollowing";
        if (blackboard.isAttacking) return "exitToAttackPlayer";
        return null;
    }
}