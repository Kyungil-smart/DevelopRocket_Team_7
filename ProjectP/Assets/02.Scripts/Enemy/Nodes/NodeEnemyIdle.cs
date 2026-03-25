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
		// Damage 에 대한 것은 EnemyDamaged 클래스에서 제어됨.
		if (blackboard.IsDamaged) return "exitToDamaged";
		
		// 인식 범위안에 있는지 확인
		return ToFollowingToPlayer(blackboard);
	}
}