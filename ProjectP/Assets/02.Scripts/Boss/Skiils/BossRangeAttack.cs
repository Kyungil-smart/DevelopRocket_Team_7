using UnityEngine;

public class BossRangeAttack : MonoBehaviour
{
    [SerializeField] private BossBulletSpawner _spawner;
    
    // 아래 함수는 [BossRangeAttack.anim] 에 Animation Event 로 등록됨.
    public void OnRangeAttackBehavior()
    {
        _spawner.SpawnBullets();
    }
}
