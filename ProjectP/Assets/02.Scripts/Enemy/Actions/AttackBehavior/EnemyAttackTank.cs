using System;
using UnityEngine;

public class EnemyAttackTank : MonoBehaviour, IEnemyAttackBehavior
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _hitAreaPrefab;
    private EnemyBlackboard _blackboard;
    private Collider2D _collider;
    private int _damage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_blackboard == null) return;
        _hitAreaPrefab.transform.position = _blackboard.facingDirection * 2;
    }

    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard)
    {
        // 공통화 하려다 보니.. 이런 이상한 Logic 이 생기는..
        if (_blackboard == null) _blackboard = blackboard;
        _animator.SetBool("Attack", true);
        _animator.SetBool("Move", false);
        _collider = collider;
        _damage = blackboard.origin.damage;
    }

    // 애니메이션 Event 로 등록 할 함수
    public void OnTankAttack()
    {
        // ToDo. 공격관련 내용 추가
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
