using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    public GameObject target;
    
    [SerializeField] private EnemyAttack _attackScript;
    [SerializeField] private EnemyDead _deadScript;
    [SerializeField] private EnemyMovement _movementScript;
    [SerializeField] private EnemyDamaged _damagedScript;
    
    private EnemyBlackboard _blackboard;

    private void Awake()
    {
        // MonoBehavior Classes
        _attackScript = GetComponent<EnemyAttack>();
        _deadScript = GetComponent<EnemyDead>();
        _movementScript = GetComponent<EnemyMovement>();
        _damagedScript = GetComponent<EnemyDamaged>();
        // Pure C# Classes
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void SetBlackBoard(EnemyBlackboard blackboard)
    {
        _blackboard = blackboard;
        // blackboard 가 필요한 스크립트에 blackboard 전달하기.
        if (_damagedScript != null) _damagedScript.SetBlackboard(blackboard);
        
        AddListeners();
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

    public void Patrol()
    {
        if (_movementScript != null) _movementScript.Patrol(_blackboard);
    }
    
    public void OnMoveToPlayer()
    {
        if (_movementScript != null) _movementScript.GoToPlayer(_blackboard);
    }
    
    public void OnAttack()
    {
        if (_attackScript != null) _attackScript.Attack(_blackboard.origin.damage, target);
    }
    
    public void OnDead()
    {
        if (_deadScript != null) _deadScript.Dead();
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
