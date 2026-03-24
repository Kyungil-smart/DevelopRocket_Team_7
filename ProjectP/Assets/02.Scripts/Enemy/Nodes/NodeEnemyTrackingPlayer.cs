using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyTrackingPlayer : EnemyBaseNode 
{
	[Input] public EnemyStateConnection entry;
	[Output] public EnemyStateConnection exitToAttackPlayer;
	[Output] public EnemyStateConnection exitToDamaged;
	
	public override string Execute(EnemyBlackboard blackboard)
	{
		Debug.Log("겁나 쫒아가고 있긔");
		if (blackboard.IsAttacking) return "exitToAttackPlayer";
		if (blackboard.IsDamaged) return "exitToDamaged";
		return null;
	}
}