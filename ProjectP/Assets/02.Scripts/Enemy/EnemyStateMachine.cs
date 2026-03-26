using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XNode;
using Object = System.Object;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private EnemyData _originData;
    [SerializeField] private EnemyAgent _agent;
    [SerializeField] private EnemyNodeGraph _graph;
    [Header("아래 CurrentNode 는 데이터 확인용이니 Inspector 에서 제어하지 마세요.")]
    [SerializeField] private Node _currentNode;
    private Coroutine _coroutine;
    private WaitForSeconds _wait = new WaitForSeconds(0.1f);
    private EnemyBlackboard _blackboard;

    private void Awake()
    {
        _agent = GetComponent<EnemyAgent>();
    }

    private void OnEnable()
    {
        Init();
        SetIdleNode();
        _coroutine = StartCoroutine(StateMachine());
    }

    private void OnDisable()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    private void Init()
    {
        if (_blackboard == null)
            _blackboard = new EnemyBlackboard(_originData, _agent);
        
        _blackboard.Init();
        _agent.SetBlackBoard(_blackboard);
    }
    
    private void SetIdleNode()
    {
        // idle state 먼저 실행
        foreach (var node in _graph.nodes)
        {
            // ToDo. 이름 변경시 동작 안됨. 주의. 
            if (node.name == "Node Enemy Idle") 
            {
                _currentNode = node;
                _blackboard.IsIdle = true;
                break;
            }
        }
    }

    private IEnumerator StateMachine()
    {
        while (!_blackboard.IsDead)
        {
            string portName = (_currentNode as EnemyBaseNode)?.Execute(_blackboard);
            if (portName != null)
            {
                _currentNode = _currentNode.GetOutputPort(portName).Connection.node;
            }
            yield return _wait;
        }
        yield return null;
    }

    [ContextMenu("Debug/Idle")]
    private void DebugOnIdle()
    {
        _blackboard.IsIdle = !_blackboard.IsIdle;
    }
    
    [ContextMenu("Debug/Attack")]
    private void DebugOnAttack()
    {
        _blackboard.IsAttacking = !_blackboard.IsAttacking;
    }
    
    [ContextMenu("Debug/AttackDelay")]
    private void DebugOnAttackDelay()
    {
        _blackboard.IsAttackDelay = !_blackboard.IsAttackDelay;
    }
    
    [ContextMenu("Debug/Following")]
    private void DebugOnFollowing()
    {
        _blackboard.IsFollowing = !_blackboard.IsFollowing;
    }
    
    [ContextMenu("Debug/Dead")]
    private void DebugOnDead()
    {
        _blackboard.IsDead = !_blackboard.IsDead;
    }
}
