using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    private EnemyAttack _attackScript;
    private EnemyDead _deadScript;
    private EnemyFindTarget _findTargetScript;
    private EnemyMovement _movementScript;
    private EnemyDamaged _damagedScript;
    
    private EnemyBlackboard _blackboard;

    private void Start()
    {
        // MonoBehavior Classes
        if (_attackScript == null) _attackScript = GetComponent<EnemyAttack>();
        if (_deadScript == null) _deadScript = GetComponent<EnemyDead>();
        if (_movementScript == null) _movementScript = GetComponent<EnemyMovement>();
        
        // Pure C# Classes
        if (_damagedScript == null) _damagedScript = new EnemyDamaged();
        if (_findTargetScript == null) _findTargetScript = GetComponent<EnemyFindTarget>();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void SetBlackBoard(EnemyBlackboard blackboard)
    {
        _blackboard = blackboard;
        AddListeners();
    }

    private void AddListeners()
    {
        _blackboard.OnAttacked += OnAttack;
        _blackboard.OnDamaged += OnDamaged;
        _blackboard.OnDead += OnDead;
        _blackboard.OnFollowed += OnMoveToPlayer;
    }

    private void RemoveListeners()
    {
        _blackboard.OnAttacked -= OnAttack;
        _blackboard.OnDamaged -= OnDamaged;
        _blackboard.OnDead -= OnDead;
        _blackboard.OnFollowed -= OnMoveToPlayer;
    }

    public void OnMoveToPlayer()
    {
        
    }
    
    public void OnAttack()
    {
        
    }

    public void OnDamaged()
    {
        
    }
    
    public void OnDead()
    {
        
    }
}
