using UnityEngine;

public interface IEnemyAttackBehavior
{
    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard);
}