using UnityEngine;

public class EnemyDamaged : IEnemyDamagable, INeedBlackboard
{
    private EnemyBlackboard blackboard;
    
    // ToDo. 유저 무기에 의한 피격 계산 추가 필요
    public void TakeDamage(int damage)
    {
        blackboard.currentHp -= damage;
        blackboard.IsDamaged = true;
    }

    public void SetBlackboard(EnemyBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }
}