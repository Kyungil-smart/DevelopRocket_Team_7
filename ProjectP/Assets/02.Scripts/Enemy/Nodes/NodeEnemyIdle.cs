using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyIdle : EnemyBaseNode 
{
	[Output] public EnemyStateConnection exitToFollowingPlayer;
	[Output] public EnemyStateConnection exitToDead;

	public override string Execute(EnemyBlackboard blackboard)
	{
		Debug.Log("난 Idle 상태여");
		
		// 1. 인식 범위안에 있는지 확인
		string transitionName;
		transitionName = ToFollowingToPlayer(blackboard);
		if (transitionName != null) return transitionName;
		
		// 2. Patrol => 행위는 Agent 에서 하고 있음.
		return null;
	}
}