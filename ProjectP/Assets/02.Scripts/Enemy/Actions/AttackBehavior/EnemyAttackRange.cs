using System.Collections;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour, IEnemyAttackBehavior
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bulletPrefab;
    private int _damage;
    private EnemyBlackboard _blackboard;
    private Coroutine _fireCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnAttack(Collider2D collider, EnemyBlackboard blackboard)
    {
        _animator.SetBool("Attack", true);
        _animator.SetBool("Move", false);
        _damage = blackboard.origin.damage;
        _blackboard = blackboard;
    }

    // 애니메이션 Event 를 통해 호출되는 함수
    public void OnFire()
    {
        if (_fireCoroutine == null) _fireCoroutine = StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        // ToDo. (기술부채) 이슈; 케릭터가 두번 데미지 입음. 실제로 원거리 총알이 두번 날라감.
        // 해당 내용은 state 가 갑자기 변동 한 것으로 보이지만 테스트가 어려워 일단 아래와 같이 수정함  
        Vector2 dir = (_blackboard.targetPosition - (Vector2)transform.position);
        EnemyRangeBulletSpawnMsg data = new EnemyRangeBulletSpawnMsg()
        {
            startPos = (Vector2)transform.position + (dir.normalized * 0.2f),
            direction = (_blackboard.targetPosition - (Vector2)transform.position),
            damage = _damage
        };
        PostManager.Instance.Post(PostMessageKey.EnemyRangeBulletSpawned, data);
        yield return new WaitForEndOfFrame();
        _animator.SetBool("Attack", false);
        if (_blackboard.IsAttacking) _blackboard.IsAttacking = false;
        _fireCoroutine = null;
    }
}
