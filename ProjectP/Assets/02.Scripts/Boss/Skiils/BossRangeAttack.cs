using UnityEngine;

public class BossRangeAttack : MonoBehaviour
{
    [SerializeField] private BossBulletSpawner _spawner;
    
    public void OnRangeAttackBehavior()
    {
        _spawner.SpawnBullets();
    }
}
