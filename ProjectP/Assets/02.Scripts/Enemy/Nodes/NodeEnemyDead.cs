using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeEnemyDead : EnemyBaseNode 
{
	[Input] public EnemyStateConnection entry;
	
	public override string Execute(EnemyBlackboard blackboard)
	{
		Debug.Log("난 죽어써");
		blackboard.IsDead = false;
		return null;
	}
}