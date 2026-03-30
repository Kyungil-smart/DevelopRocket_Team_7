using System;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttackable
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private MonoBehaviour _attackBehavior;
    private CircleCollider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
    }

    public void Attack(EnemyBlackboard blackboard)
    {
        Vector2 targetPos = blackboard.targetPosition;
        Vector3 direction = new Vector3(targetPos.x * _collider2D.radius, targetPos.y * _collider2D.radius);
        Vector2 rayOriginPos = transform.position + direction;
        Ray2D ray = new Ray2D(rayOriginPos, targetPos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, blackboard.origin.attackRange, _layerMask);
        if (hit.collider != null)
        {
            if (_attackBehavior is EnemyAttackChase chaseBh) chaseBh.OnAttack(hit.collider, blackboard);
            else if (_attackBehavior is EnemyAttackRange rangeBh) rangeBh.OnAttack(hit.collider, blackboard);
            else (_attackBehavior as EnemyAttackTank)?.OnAttack(hit.collider, blackboard);
        }        
    }
}
