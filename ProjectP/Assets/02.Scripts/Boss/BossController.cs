using UnityEngine;

public class BossController : MonoBehaviour, IBossDamagable
{
    [SerializeField] private BossData bossData;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BossBasicAttack _basicAttackScript;
    [SerializeField] private BossRangeAttack _rangeAttackScript;
    [SerializeField] private BossBulletSpawner _bulletSpawnerScript;
    [SerializeField] private BossChangePhase _changePhaseScript;
    [SerializeField] private BossMovement _movementScript;
    
    private BossBlackBoard _blackBoard;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _blackBoard = new BossBlackBoard(bossData);
        _movementScript.SetBlackboard(_blackBoard);
        _changePhaseScript.SetBlackboard(_blackBoard);
        _basicAttackScript.SetBlackboard(_blackBoard);
    }
    
    public void TakeDamage(float damage)
    {
        if (!_blackBoard.IsInvincible) _blackBoard.currentHp -= damage;
        if (!_movementScript.IsChaseForce) _movementScript.OnChaseForce();
        if (_blackBoard.currentHp <= 0) OnDead();
    }
    
    private void OnBasicAttack()
    {
        _animator.SetBool("IsMoving", false);
        _animator.SetTrigger("OnBasicAttack");
    }

    private void OnRangeAttack()
    {
        _animator.SetBool("IsMoving", false);
        _animator.SetTrigger("OnRangeAttack");
    }

    private void OnChangePhase()
    {
        _animator.SetBool("IsMoving", false);
        _animator.SetTrigger("OnChangePhase");
    }

    private void OnDead()
    {
        _animator.SetBool("IsMoving", false);
        _animator.SetTrigger("OnDead");
    }


    [ContextMenu("Test/BasicAttack")]
    private void OnTestBasicAttack()
    {
        OnBasicAttack();
    }
    
    [ContextMenu("Test/RangeAttack")]
    private void OnTestRangeAttack()
    {
        OnRangeAttack();
    }
    
    [ContextMenu("Test/ChangePhase")]
    private void OnTestChangePhase()
    {
        OnChangePhase();
    }
    
    [ContextMenu("Test/Dead")]
    private void OnTestDead()
    {
        OnDead();
    }
}
