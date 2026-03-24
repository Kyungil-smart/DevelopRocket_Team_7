using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XNode;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private EnemyData _originData;
    [SerializeField] private EnemyAgent _agent;
    [SerializeField] private EnemyNodeGraph _graph;
    private Node _currentNode;
    private Coroutine _coroutine;
    private WaitForSeconds _wait = new WaitForSeconds(0.1f);
    private EnemyBlackboard _blackboard;
    
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
        _blackboard = new EnemyBlackboard(_originData, _agent);
        _agent.SetBlackBoard(_blackboard);
    }
    
    private void SetIdleNode()
    {
        // idle state 먼저 실행
        foreach (var node in _graph.nodes)
        {
            NodePort inputPort = node.GetInputPort("entry");
            if (inputPort == null)
            {
                _currentNode = node;
                break;
            }
        }
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            string portName = (_currentNode as EnemyBaseNode)?.Execute(_blackboard);
            if (portName != null)
            {
                _currentNode = _currentNode.GetOutputPort(portName).Connection.node;
            }
            yield return _wait;
        }
        
    }
}
