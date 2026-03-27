using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyAttackPlayer : EnemyBaseNode
{
	[Input] public EnemyStateConnection entry;
	[Output] public EnemyStateConnection exitToAttackDelay;
	[Output] public EnemyStateConnection exitToFollowingPlayer;
	[Output] public EnemyStateConnection exitToDead;
	
	public override string Execute(EnemyBlackboard blackboard)
	{
		blackboard.IsAttacking = false;
		
		string transitionName;
		// 1. 공격 범위를 벗어남
		transitionName = ToFollowingToPlayer(blackboard);
		if (transitionName != null) return transitionName;
		
		// 2. 공격 볌위 안에 있음
		return ToAttackDelay(blackboard);
	}
}