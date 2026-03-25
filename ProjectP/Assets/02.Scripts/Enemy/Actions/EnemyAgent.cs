using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    public GameObject target;
    
    private EnemyAttack _attackScript;
    private EnemyDead _deadScript;
    private EnemyMovement _movementScript;
    private EnemyDamaged _damagedScript;
    
    private EnemyBlackboard _blackboard;

    private void OnEnable()
    {
        // MonoBehavior Classes
        if (_attackScript == null) _attackScript = GetComponent<EnemyAttack>();
        if (_deadScript == null) _deadScript = GetComponent<EnemyDead>();
        if (_movementScript == null) _movementScript = GetComponent<EnemyMovement>();
        
        // Pure C# Classes
        if (_damagedScript == null) _damagedScript = new EnemyDamaged();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void SetBlackBoard(EnemyBlackboard blackboard)
    {
        _blackboard = blackboard;
        // blackboard 가 필요한 스크립트에 blackboard 전달하기.
        _damagedScript.SetBlackboard(blackboard);
        
        AddListeners();
        Patrol();
    }

    private void AddListeners()
    {
        _blackboard.OnAttacked += OnAttack;
        _blackboard.OnDead += OnDead;
        _blackboard.OnFollowed += OnMoveToPlayer;
    }

    private void RemoveListeners()
    {
        _blackboard.OnAttacked -= OnAttack;
        _blackboard.OnDead -= OnDead;
        _blackboard.OnFollowed -= OnMoveToPlayer;
    }

    private void Patrol()
    {
        _movementScript.Patrol(_blackboard.origin.speed);
    }
    
    public void OnMoveToPlayer()
    {
        _movementScript.Move(target.transform.position, _blackboard.origin.speed);
    }
    
    public void OnAttack()
    {
        _attackScript.Attack(_blackboard.origin.damage, target);
    }
    
    public void OnDead()
    {
        _deadScript.Dead();
    }

    private void OnDrawGizmos()
    {
        if (_blackboard == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _blackboard.origin.attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _blackboard.origin.detectRadius);
    }
}
