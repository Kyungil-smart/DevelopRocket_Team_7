using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Enemy 의 동작에 대한 중심 역할. State 에서 해야할 역할에 대한 것들은 모두 Agent 에 존재.
/// State 는 직접적으로 agent 를 호출 하는 것은 "되도록"지양하되, 필요시에는 호출 가능.
/// Blackboard 내 Bool Flag 변경으로 Observer Pattern 으로 구현되어 있음.
/// </summary>
public class EnemyAgent : MonoBehaviour
{
    [SerializeField] private EnemyAttack _attackScript;
    [SerializeField] private EnemyAttackDelay _attackDelayScript;
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
        _attackDelayScript = GetComponent<EnemyAttackDelay>();
        // Pure C# Classes
    }

    private void OnEnable()
    {
        if (_blackboard != null) _blackboard.IsDead = false;
    }

    private void OnDisable()
    {
        RemoveListeners();
        PostManager.Instance.Unsubscribe<Vector2>(PostMessageKey.PlayerPosition, UpdateTargetPosition);
    }

    public void SetBlackBoard(EnemyBlackboard blackboard)
    {
        _blackboard = blackboard;
        // blackboard 가 필요한 스크립트에 blackboard 전달하기.
        if (_damagedScript != null) _damagedScript.SetBlackboard(blackboard);
        
        PostManager.Instance.Subscribe<Vector2>(PostMessageKey.PlayerPosition, UpdateTargetPosition);
        AddListeners();
    }

    private void AddListeners()
    {
        _blackboard.OnIdle += OnPatrol;
        _blackboard.OnAttackDelay += OnAttackDelay;
        _blackboard.OnAttacked += OnAttack;
        _blackboard.OnDead += OnDead;
        _blackboard.OnFollowed += OnMoveToPlayer;
    }

    private void RemoveListeners()
    {
        _blackboard.OnIdle -= OnPatrol;
        _blackboard.OnAttackDelay -= OnAttackDelay;
        _blackboard.OnAttacked -= OnAttack;
        _blackboard.OnDead -= OnDead;
        _blackboard.OnFollowed -= OnMoveToPlayer;
    }

    private void UpdateTargetPosition(Vector2 position)
    {
        _blackboard.targetPosition = position;
    }

    private void OnPatrol()
    {
        if (_movementScript != null) _movementScript.Patrol(_blackboard);
    }
    
    private void OnMoveToPlayer()
    {
        if (_movementScript != null) _movementScript.GoToPlayer(_blackboard);
    }

    private void OnAttackDelay()
    {
        if (_attackDelayScript != null) _attackDelayScript.OnDelayBeforeAttack(_blackboard);
    }
    
    private void OnAttack()
    {
        Debug.Log("Attack!!");
        if (_attackScript != null) _attackScript.Attack(_blackboard);
    }
    
    private void OnDead()
    {
        if (_deadScript != null) _deadScript.Dead(_blackboard);
        // 경험치를 100% 확률로 전달.
        PostManager.Instance.Post(PostMessageKey.PostExp, _blackboard.origin.exp);
        
        // 배터리를 특정 활률로 떨궈
        if (UnityEngine.Random.value <= _blackboard.origin.batteryProbability)
        {
            PostManager.Instance.Post<Vector2>(PostMessageKey.BatterySpawned, transform.position);
        }
        
        // Despawn
        string name = gameObject.name.Split('_')[0];
        EnemyDespawnMsg msg = new EnemyDespawnMsg()
        {
            name = name,
            obj = gameObject
        };
        PostManager.Instance.Post(PostMessageKey.EnemyDespawned, msg);
    }
    
    // Test Code
    [ContextMenu("Test/Damaged")]
    public void TestTakeDamage()
    {
        _damagedScript.TakeDamage(DamageType.Projectile, 20);
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
