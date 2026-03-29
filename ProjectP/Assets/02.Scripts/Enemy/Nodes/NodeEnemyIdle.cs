using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyIdle : EnemyBaseNode
{
	[Input] public EnemyStateConnection entry;
	[Output] public EnemyStateConnection exitToFollowingPlayer;
	[Output] public EnemyStateConnection exitToDead;

	public override string Execute(EnemyBlackboard blackboard)
	{
		blackboard.IsIdle = false;
		
		// 1. 인식 범위안에 있는지 확인
		string transitionName;
		transitionName = ToFollowingToPlayer(blackboard);
		if (transitionName != null) return transitionName;
		// 2. Patrol; 
		return null;
	}
}