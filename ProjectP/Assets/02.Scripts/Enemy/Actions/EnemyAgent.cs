using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    public GameObject target;
    
    private EnemyAttack _attackScript;
    private EnemyDead _deadScript;
    private EnemyFindTarget _findTargetScript;
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
        if (_findTargetScript == null) _findTargetScript = new EnemyFindTarget();
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
        Vector2 nxPos = _findTargetScript.GetNextPosition(null);
        _movementScript.Move(nxPos);
    }
    
    public void OnAttack()
    {
        _attackScript.Attack(_blackboard.origin.damage, null);
    }

    public void OnDamaged()
    {
        _damagedScript.TakeDamage(100);
    }
    
    public void OnDead()
    {
        _deadScript.Dead();
    }
}
