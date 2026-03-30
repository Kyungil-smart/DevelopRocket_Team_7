using UnityEngine;

public class EnemyAttackChase : MonoBehaviour, IEnemyAttackBehavior
{
    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard) => 
        collider?.GetComponent<IDamage>().TakeDamage(blackboard.origin.damage);
}
