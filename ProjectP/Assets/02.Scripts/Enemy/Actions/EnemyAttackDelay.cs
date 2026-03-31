using System.Collections;
using UnityEngine;

public class EnemyAttackDelay : MonoBehaviour
{
    private Coroutine _delayCoroutine;
    
    public void OnDelayBeforeAttack(EnemyBlackboard blackboard)
    {   // 방어 로직 추가를 해야겠다
        if (_delayCoroutine == null) 
            _delayCoroutine = StartCoroutine(WaitBeforeAttackCorutine(blackboard));
    }

    private IEnumerator WaitBeforeAttackCorutine(EnemyBlackboard blackboard)
    {
        yield return new WaitForSeconds(blackboard.origin.attackDelay);
        blackboard.IsAttacking = true;
        _delayCoroutine = null;
    }
}
