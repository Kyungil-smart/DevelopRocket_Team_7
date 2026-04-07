using System;
using System.Collections;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour, IDamageable, INeedEnemyBlackboard
{   // State 독립 트리거. 
    private EnemyBlackboard blackboard;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _effectInDamagedCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(DamageType type, int damage)
    {
        // ToDo. 필요시 유저 무기에 의한 피격 계산 추가 필요
        // DamageType type 에 따라 몇가지 추가 행위 필요.
        blackboard.currentHp -= damage;
        if (_effectInDamagedCoroutine == null) _effectInDamagedCoroutine = StartCoroutine(EffectInDamagedCoroutine());
        // 유저가 멀리 있는 경우
        float distance = Mathf.Abs(Vector2.Distance(
            blackboard.targetPosition, blackboard.agent.transform.position));
        
        Debug.Log($"{name}: 아파여 {blackboard.currentHp}");
        Debug.Log($"{name}: {blackboard.origin.attackRange}");
        Debug.Log($"{name}: {distance}");
        // 맞으면, 공격 범위 내에 없으면 일단 모르겠고 쫒아가.
        if (blackboard.origin.attackRange < distance)
            blackboard.IsFollowing = true;
        
        // 맞다가 hp 가 없으면 Die.
        if (blackboard.currentHp <= 0)
            blackboard.IsDead = true;
    }

    public void SetBlackboard(EnemyBlackboard blackboard)
    {
        if (_effectInDamagedCoroutine != null) StopCoroutine(_effectInDamagedCoroutine);
        _effectInDamagedCoroutine = null;
        this.blackboard = blackboard;
        _spriteRenderer.color = new Color(1, 1, 1);
    }

    private IEnumerator EffectInDamagedCoroutine()
    {
        _spriteRenderer.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.color = new Color(1, 1, 1);
        _effectInDamagedCoroutine = null;
    }
}