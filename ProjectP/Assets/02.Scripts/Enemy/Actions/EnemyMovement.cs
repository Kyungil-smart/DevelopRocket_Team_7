using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _layerMask;
    private EnemyBlackboard _blackboard;
    private CircleCollider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _targetPos;
    private Transform _targetTransform;
    private bool _isChasing;
    private Coroutine _nxPosCoroutine;
    private WaitForSeconds _waitFor2Sec = new WaitForSeconds(2.0f);
    private Vector2[] _patrolDirections = new Vector2[4] { Vector2.right, Vector2.left, Vector2.down, Vector2.up };
    private Ray2D _ray;
    private float _detectDistance = 0.2f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_blackboard == null) return;
        float distance = Mathf.Abs(
            Vector2.Distance(transform.position, _blackboard.agent.target.transform.position));
        if (distance <= _blackboard.origin.attackRange) return;

        float step = _blackboard.origin.speed * Time.deltaTime;
        if (_targetTransform != null && _isChasing)
        {
            _ray = GetRay(_targetTransform.position.normalized);
            RaycastHit2D hit = Physics2D.Raycast(_ray.origin, _ray.direction, _detectDistance, _layerMask);
            if (!hit) transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, step);
        }  
        else transform.position = Vector3.MoveTowards(transform.position, _targetPos, step);
        
        // ToDo. 우선적으로 이렇게 처리하지만, 추후 자연스럽게 하기 위해 어떻게 해야할지 연구 필요.
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y);
    }
    
    public void GoToPlayer(EnemyBlackboard blackboard) {  
        if (_blackboard == null) _blackboard = blackboard;
        _isChasing = true;
        _targetTransform = _blackboard.agent.target.transform;
        if (_nxPosCoroutine != null) StopCoroutine(_nxPosCoroutine);
        _nxPosCoroutine = null;
        Debug.Log("점마 딱 대라이");
    }

    public void Patrol(EnemyBlackboard blackboard)
    {  // Init 상태에서 실행할 것.
        if (_blackboard == null) _blackboard = blackboard;
        _isChasing = false;
        if (_nxPosCoroutine == null)
        {
            _nxPosCoroutine = StartCoroutine(ChoiceNextPositionInPatrolCoroutine());    
        }
    }
    
    private IEnumerator ChoiceNextPositionInPatrolCoroutine()
    {
        while (!_isChasing)
        {
            Vector2 direction = _patrolDirections[Random.Range(0, _patrolDirections.Length)];
            _targetPos = transform.position + new Vector3(direction.x, direction.y);
            Ray2D ray = GetRay(direction);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1, _layerMask);
            if (!hit) yield return _waitFor2Sec;
        }
    }

    private Ray2D GetRay(Vector2 direction)
    {
        Vector3 rayOriginPos = transform.position + 
                               new Vector3(direction.x * _collider2D.radius, direction.y * _collider2D.radius);
        return new Ray2D(rayOriginPos, direction);
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.white;
    //     Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction);
    // }
}
