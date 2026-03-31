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
        float distance = Vector2.Distance(blackboard.targetPosition, transform.position);
        Debug.Log($"d;{distance} a;{blackboard.origin.attackRange} t;{blackboard.origin.detectRadius}");
        if (distance <= blackboard.origin.attackRange) blackboard.IsAttacking = true;
        else if (distance <= blackboard.origin.detectRadius) blackboard.IsFollowing = true;
        else blackboard.IsIdle = true;
    }
}
