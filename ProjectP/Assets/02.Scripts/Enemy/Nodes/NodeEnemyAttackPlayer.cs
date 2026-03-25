using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyAttackPlayer : EnemyBaseNode
{
	[Input] public EnemyStateConnection entry;
	[Output] public EnemyStateConnection exitToDamaged;
	[Output] public EnemyStateConnection exitToFollowingPlayer;
	
	public override string Execute(EnemyBlackboard blackboard)
	{
		Debug.Log("겁나 패고 있음");
		if (blackboard.IsFollowing) return "exitToFollowingPlayer";
		if (blackboard.IsDamaged) return "exitToDamaged";
		return null;
	}
}