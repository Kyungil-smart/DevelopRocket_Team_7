using UnityEngine;

public class EnemyAttackChase : MonoBehaviour, IEnemyAttackBehavior
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard)
    {
        _animator.SetBool("Move", true);
        collider?.GetComponent<IDamage>().TakeDamage(blackboard.origin.damage);
        blackboard.IsAttacking = false;
        blackboard.IsAttackDelay = true;
    }
}
