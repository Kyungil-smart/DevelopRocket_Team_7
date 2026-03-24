using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyIdle : EnemyBaseNode 
{
	[Output] public EnemyStateConnection exitToDamaged;
	[Output] public EnemyStateConnection exitToFollowingPlayer;
	[Output] public EnemyStateConnection exitToAttackPlayer;

	public override string Execute(EnemyBlackboard blackboard)
	{
		// ToDo: Idle 상태에서 어떻게 있을 것인지 정해야함.
		// 그것에 따라 Trigger 가 변경됨.
		if (blackboard.isDamaged) return "exitToDamaged";
		if (blackboard.isAttacking) return "exitToAttackPlayer";
		if (blackboard.isFollowing) return "exitToFollowingPlayer";
		return null;
	}
}