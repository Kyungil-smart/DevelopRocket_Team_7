using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyIdle : EnemyBaseNode 
{
	[Output] public EnemyStateConnection exitToDamaged;
	[Output] public EnemyStateConnection exitToFollowingPlayer;

	public override string Execute(EnemyBlackboard blackboard)
	{
		// ToDo: Idle 상태에서 어떻게 있을 것인지 정해야함.
		// 그것에 따라 Trigger 가 변경됨.
		Debug.Log("난 Idle 상태여");
		// Trigger Player 에게 데미지를 입음
		// blackboard.IsDamaged = true;
		
		// Trigger Player 가 가까움
		// blackboard.IsFollowing = true;
		
		if (blackboard.IsDamaged) return "exitToDamaged";
		if (blackboard.IsFollowing) return "exitToFollowingPlayer";
		return null;
	}
}