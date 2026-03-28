using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IBossDamagable
{
    [SerializeField] private BossData bossData;
    [Header("Animation Settings")]
    [SerializeField] private Animator _animator;
    [SerializeField] private List<AnimationClip> _animationClips;
    
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BossBasicAttack _basicAttackScript;
    [SerializeField] private BossRangeAttack _rangeAttackScript;
    [SerializeField] private BossBulletSpawner _bulletSpawnerScript;
    [SerializeField] private BossChangePhase _changePhaseScript;
    [SerializeField] private BossMovement _movementScript;
    
    private BossBlackBoard _blackBoard;
    private Coroutine _coroutine;
    private WaitForSecondsRealtime _globalCooldown = new WaitForSecondsRealtime(0.1f);

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

    private void Update()
    {
        if (_coroutine == null && _blackBoard.IsAttacking)
        {
            _coroutine = StartCoroutine(SkillCoroutine());
        }
        else if (_coroutine != null && !_blackBoard.IsAttacking)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
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

    public void OnDeath()
    {
        _blackBoard.IsDead = true;
        Destroy(gameObject);
    }

    private IEnumerator SkillCoroutine()
    {
        while (!_blackBoard.IsDead)
        {
            // ToDo. 요기 Skill 이 모두 완료 될 때 까지 기다리고 싶은데 어떻게 하면 좋을까?
            float hpRate = _blackBoard.currentHp / _blackBoard.origin.maxHp;
            if (hpRate <= 0.5f && !_blackBoard.IsBurnning)
            {
                OnChangePhase();
                _blackBoard.IsBurnning = true;
                yield return new WaitForSeconds(GetAnimationClip("BossChangePhase").length);
            } else if (_blackBoard.currentHp >= 0 && hpRate * 100 % 11 == 0)
            {   // ToDo. 각 11% 구간내에 한번만 사용 해야하는데..?
                OnRangeAttack();
                yield return new WaitForSeconds(GetAnimationClip("BossRangeAttack").length);
            }
            else
            {   
                OnBasicAttack();
                yield return new WaitForSeconds(GetAnimationClip("BossBasicAttack").length);
            }
            yield return _globalCooldown;
        }   
    }

    private AnimationClip GetAnimationClip(string name)
    {
        foreach (AnimationClip clip in _animationClips)
        {
            if (clip.name == name) return clip;
        }
        return null;
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
    
    [ContextMenu("Test/TakeDamage")]
    private void OnTestTakeDamage()
    {
        TakeDamage(1000);
    }
    
    [ContextMenu("Test/Dead")]
    private void OnTestDead()
    {
        OnDead();
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, _blackBoard.origin.attackRange);
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, _blackBoard.origin.detectRadius);
    // }
}
