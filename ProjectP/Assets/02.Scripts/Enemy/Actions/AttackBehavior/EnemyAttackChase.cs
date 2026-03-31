using UnityEngine;

public class EnemyAttackChase : MonoBehaviour, IEnemyAttackBehavior
{
    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard)
    {
        if (blackboard.IsAttacking) blackboard.IsAttacking = false;
        collider?.GetComponent<IDamage>().TakeDamage(blackboard.origin.damage);   
    }
}
