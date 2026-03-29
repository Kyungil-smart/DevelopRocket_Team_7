using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터 관리와 행위에 대한 모든 관리를 함.
/// 단일 책임을 위배하지만, 현재는 이렇게 구현하겠습니다.
/// </summary>
public class BossController : MonoBehaviour, IBossDamagable
{
    [Header("Boss Script Objects")]
    [SerializeField] private BossData bossData;
    
    [Header("Animation Settings")]
    [SerializeField] private Animator _animator;
    [SerializeField] private List<AnimationClip> _animationClips;
    
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BossBasicAttack _basicAttackScript;
    [SerializeField] private BossRangeAttack _rangeAttackScript;
    [SerializeField] private BossBulletSpawner _bulletSpawnerScript;
    [SerializeField] private BossChangePhase _changePhaseScript;
    [SerializeField] private BossMovement _movementScript;

    [Header("Phase Settings")]
    [SerializeField] [Range(0f, 1f)] private float _rangeAttackHpRateStep;
    [SerializeField] [Range(0f, 1f)] private float _burningPhaseHpRate;
    
    private BossBlackBoard _blackBoard;
    private Coroutine _coroutine;
    private WaitForSecondsRealtime _globalCooldown = new WaitForSecondsRealtime(0.1f);
    private float nxHpForRangeAttack;
    private int nxHpRateStep;

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
        nxHpRateStep = 1;
        SetHpStepForRangeAttack();
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
        PostManager.Instance.Post<int>(PostMessageKey.PostExp, _blackBoard.origin.experience);
        PostManager.Instance.Post<Vector2>(PostMessageKey.BatterySpawned, transform.position);
        Destroy(gameObject);
    }

    private IEnumerator SkillCoroutine()
    {
        while (!_blackBoard.IsDead)
        {
            float hpRate = _blackBoard.currentHp / _blackBoard.origin.maxHp;
            if (hpRate <= _burningPhaseHpRate && !_blackBoard.IsBurnning)
            {
                OnChangePhase();
                _blackBoard.IsBurnning = true;
                yield return new WaitForSeconds(GetAnimationClip("BossChangePhase").length);
            } else if (_blackBoard.currentHp > 0 && _blackBoard.currentHp <= nxHpForRangeAttack)
            {   
                OnRangeAttack();
                SetHpStepForRangeAttack();
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

    private void SetHpStepForRangeAttack()
    {
        //         boss 풀 체력                다음 구간          기준 %      
        float hp = _blackBoard.origin.maxHp * nxHpRateStep++ * _rangeAttackHpRateStep;
        nxHpForRangeAttack = _blackBoard.origin.maxHp - hp;
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
        TakeDamage(50);
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
