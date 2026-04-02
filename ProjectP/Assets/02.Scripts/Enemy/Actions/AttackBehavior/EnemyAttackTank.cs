using System;
using UnityEngine;

public class EnemyAttackTank : MonoBehaviour, IEnemyAttackBehavior
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _hitAreaPrefab;
    private EnemyBlackboard _blackboard;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard)
    {
        // 공통화 하려다 보니.. 이런 이상한 Logic 이 생기는..
        if (_blackboard == null) _blackboard = blackboard;
        _hitAreaPrefab.transform.position = (Vector2)transform.position + _blackboard.facingDirection * 0.6f;
        _hitAreaPrefab.GetComponent<EnemyAttackTankHit>().SetColor(new Color(0.65f, 0, 0, 0.4f));
        _animator.SetBool("Attack", true);
        _animator.SetBool("Move", false);
    }

    // 애니메이션 Event 로 등록 할 함수
    public void OnTankAttack()
    {
        _hitAreaPrefab.GetComponent<EnemyAttackTankHit>().OnPlayerHit(_blackboard.origin.damage);
        _animator.SetBool("Attack", false);
        if (_blackboard.IsAttacking) _blackboard.IsAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_hitAreaPrefab != null)
        {
            CircleCollider2D col = _hitAreaPrefab.GetComponent<CircleCollider2D>(); 
            Gizmos.DrawWireSphere(_hitAreaPrefab.transform.position, col.radius);
        }
    }
}
