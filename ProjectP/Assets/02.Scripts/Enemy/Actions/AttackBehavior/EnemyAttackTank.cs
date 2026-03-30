using UnityEngine;

public class EnemyAttackTank : MonoBehaviour, IEnemyAttackBehavior
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _hitAreaPrefab;
    private Collider2D _collider;
    private int _damage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard)
    {
        _collider = collider;
        _damage = blackboard.origin.damage;
    }

    // 애니메이션 Event 로 등록 할 함수
    public void OnTankAttack()
    {
        
    }
}
