using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IAttackable
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
}
