using System;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttackable
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private MonoBehaviour _attackBehavior;
    
    public void Attack(EnemyBlackboard blackboard)
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, blackboard.origin.attackRange * 0.3f, _layerMask);
        if (collider != null)
        {
            if (_attackBehavior is EnemyAttackChase chaseBh) chaseBh.OnAttack(collider, blackboard);
            else if (_attackBehavior is EnemyAttackRange rangeBh) rangeBh.OnAttack(collider, blackboard);
            else (_attackBehavior as EnemyAttackTank)?.OnAttack(collider, blackboard);
        }
        else
        {   // Player 가 없을 경우 강제로 IsAttacking 을 false 로.
            blackboard.IsAttacking = false;
        }
    }
}
