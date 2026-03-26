using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyTrackingPlayer : EnemyBaseNode 
{
	[Input] public EnemyStateConnection entry;
	[Output] public EnemyStateConnection exitToAttackDelay;
	[Output] public EnemyStateConnection exitToDead;
	[Output] public EnemyStateConnection exitToIdle;
	
	public override string Execute(EnemyBlackboard blackboard)
	{
		Debug.Log("겁나 쫒아가고 있긔");
		blackboard.IsFollowing = false;
		
		// AttackRange 이내로 들어오면 공격 모션
		string transitionName = ToAttackDelay(blackboard);
		if (transitionName != null) return transitionName;
		
		return ToIdle(blackboard);
	}
}