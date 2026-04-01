using UnityEngine;

public class EnemyAttackRange : MonoBehaviour, IEnemyAttackBehavior
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bulletPrefab;
    private int _damage;
    private Collider2D _collider;
    private EnemyBlackboard _blackboard;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard)
    {
        _animator.SetBool("Attack", true);
        _animator.SetBool("Move", false);
        _damage = blackboard.origin.damage;
        _collider = collider;
        _blackboard = blackboard;
    }

    // 애니메이션 Event 를 통해 호출되는 함수
    public void OnFire()
    {
        Vector2 dir = (_blackboard.targetPosition - (Vector2)transform.position);
        EnemyRangeBulletSpawnMsg data = new EnemyRangeBulletSpawnMsg()
        {
            startPos = (Vector2)transform.position + (dir.normalized * 0.2f),
            direction = (_blackboard.targetPosition - (Vector2)transform.position),
            damage = _damage
        };
        PostManager.Instance.Post(PostMessageKey.EnemyRangeBulletSpawned, data);
        _animator.SetBool("Attack", false);
        if (_blackboard.IsAttacking) _blackboard.IsAttacking = false;
    }
}
