using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttackable
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // ToDO: 유저에게 공격하는 행위에 대한 정의.
    // 함수로 Melee 와 Range 를 나눌지 Component 로 나눌지 애매함.
    public void Attack(float damage, GameObject target)
    {
        Debug.Log($"Target 에게 {damage} 데미지를 입힘");
    }

    private void AttackMelee()
    {  // 직접 공격
        
    }

    private void OnAttackRange()
    {  // 투사체 발사. Animation 있을 경우 해당 함수 실행. 그 전에는 간단히 코루틴으로 진행.
        
    }
}
