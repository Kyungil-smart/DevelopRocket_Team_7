using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 데이터 관리와 행위에 대한 모든 관리를 함.
/// 단일 책임을 위배하지만, 현재는 이렇게 구현하겠습니다.
/// </summary>
public class BossController : MonoBehaviour, IDamageable
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
    
    [Header("Remnant")]
    [SerializeField] private GameObject _remnant;

    private SpriteRenderer _spriteRenderer;
    private BossBlackBoard _blackBoard;
    private Coroutine _coroutine;
    private Coroutine _effectInDamagedCoroutine;
    private WaitForSecondsRealtime _globalCooldown = new WaitForSecondsRealtime(0.1f);
    private float nxHpForRangeAttack;
    private int nxHpRateStep;
    // 다른 스크립트에서 보스가 죽었는지 체크하기 위해 추가
    public bool isDead => _blackBoard.IsDead;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (_blackBoard.IsDead) return;

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

    public void TakeDamage(DamageType type, int damage)
    {
        Debug.Log($"보스 죽었니? : {_blackBoard.IsDead}");
        if (_blackBoard.IsDead) return;
        Debug.Log($"무적판정 플래그 {!_blackBoard.IsInvincible}");
        if (!_blackBoard.IsInvincible)
        {
            _blackBoard.currentHp -= damage;
            Debug.Log($"[Boss] 아프다 닝겐: 남은 체력 {_blackBoard.currentHp}");
        }

        if (_blackBoard.currentHp <= 0)
        {
            OnDead();
            return;
        }
        if (_effectInDamagedCoroutine == null) StartCoroutine(EffectInDamagedCoroutine());
        if (!_movementScript.IsChaseForce) _movementScript.OnChaseForce();
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
        _blackBoard.IsDead = true;
        _animator.SetBool("IsMoving", false);
        _animator.SetTrigger("OnDead");
    }

    public void OnDeath()
    {
        PostManager.Instance.Post(PostMessageKey.PostExp, _blackBoard.origin.experience);
        // PostManager.Instance.Post<Vector2>(PostMessageKey.BatterySpawned, transform.position);
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Instantiate(_remnant, transform.position, transform.rotation);
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject,0.1f);
    }

    private IEnumerator SkillCoroutine()
    {
        Debug.Log($"보스 뒤짐?:{_blackBoard.IsDead}");
        while (!_blackBoard.IsDead)
        {

            float hpRate = _blackBoard.currentHp / _blackBoard.origin.maxHp;
            Debug.Log($"보스 다음 원거리 공격 체력 단계 : {nxHpForRangeAttack}");
            if (hpRate <= _burningPhaseHpRate && !_blackBoard.IsBurnning)
            {
                OnChangePhase();
                _blackBoard.IsBurnning = true;
                yield return new WaitForSeconds(GetAnimationClip("BossChangePhase").length+0.5f);
                _blackBoard.IsInvincible = false; // 애니메이션 이벤트로도 켜지지만, 혹시나 하는 마음에 한 번 더 켜줌.
            }
            else if (_blackBoard.currentHp > 0 && _blackBoard.currentHp <= nxHpForRangeAttack)
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
    
    private IEnumerator EffectInDamagedCoroutine()
    {
        _spriteRenderer.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.color = new Color(1, 1, 1);
        _effectInDamagedCoroutine = null;
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
        TakeDamage(DamageType.Projectile, 50);
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
