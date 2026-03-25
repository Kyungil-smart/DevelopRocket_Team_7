using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyDamaged : EnemyBaseNode 
{
    [Input] public EnemyStateConnection entry;
    [Output] public EnemyStateConnection exitToDead;
    [Output] public EnemyStateConnection exitToAttackDelay;
    [Output] public EnemyStateConnection exitToFollowingPlayer;
    
    public override string Execute(EnemyBlackboard blackboard)
    {
        Debug.Log("지금은 적이 두두려 맞고 있습니다?");
        blackboard.IsDamaged = false;

        string transitionName;
        
        // 사망시
        if (blackboard.currentHp <= 0)
        {
            blackboard.IsDead = true;
            return "exitToDead";
        }
        
        // 공격 범위 밖에 있고 인식 범위 안에 있으면
        transitionName = ToFollowingToPlayer(blackboard); 
        if (transitionName != null) return transitionName; 
        
        // 공격범위 안에 있으면
        return ToAttackDelay(blackboard);
    }
}