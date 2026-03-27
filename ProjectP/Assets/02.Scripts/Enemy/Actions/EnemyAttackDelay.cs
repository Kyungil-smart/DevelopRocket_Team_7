using System.Collections;
using UnityEngine;

public class EnemyAttackDelay : MonoBehaviour
{
    public void OnDelayBeforeAttack(EnemyBlackboard blackboard)
    {
        StartCoroutine(WaitBeforeAttackCorutine(blackboard));
    }

    private IEnumerator WaitBeforeAttackCorutine(EnemyBlackboard blackboard)
    {
        yield return new WaitForSeconds(blackboard.origin.attackDelay);
        blackboard.IsAttacking = true;
    }
}
