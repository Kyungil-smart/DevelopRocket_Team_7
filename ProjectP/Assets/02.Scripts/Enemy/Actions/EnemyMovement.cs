using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _layerMask;
    private Rigidbody2D _rb;
    private EnemyBlackboard _blackboard;
    private CircleCollider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _targetPos;
    private Vector2 _prevPos;
    private bool _isChasing;
    private Coroutine _nxPosCoroutine;
    private WaitForSeconds _waitFor2Sec = new WaitForSeconds(2.0f);
    private WaitForSeconds _waitFor10mSec = new WaitForSeconds(0.01f);
    private Vector2[] _patrolDirections = new Vector2[] { Vector2.right, Vector2.left, Vector2.down, Vector2.up };
    private Ray2D _ray;
    private Ray2D _rayPatrol;
    private float _detectDistance = 0.2f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf) Move();
    }

    private void OnEnable()
    {
        _isChasing = false;
        _targetPos = transform.position;
    }

    private void OnDisable()
    {
        if (_nxPosCoroutine != null) StopCoroutine(_nxPosCoroutine);
        _nxPosCoroutine = null;
    }

    private void Move()
    {
        if (_blackboard == null) return;
        float distance = Vector2.Distance(transform.position, _blackboard.targetPosition);
        if (distance <= _blackboard.origin.attackRange)
        {
            _rb.linearVelocity = Vector2.zero;
            _animator.SetBool("Move", false);
            return; // 공격 범위내에 있으면 멈춤
        }

        if (Vector2.Distance(_prevPos, transform.position) > 0) _animator.SetBool("Move", true);    
        else  _animator.SetBool("Move", false);
        
        // float step = _blackboard.origin.speed * Time.deltaTime;
        if (_isChasing)  // 쫒아가는 Moving
        {
            Vector2 direction = _blackboard.targetPosition - (Vector2)transform.position;
            _ray = GetRay(_blackboard.targetPosition.normalized);
            RaycastHit2D hit = Physics2D.Raycast(_ray.origin, _ray.direction, _detectDistance, _layerMask);
            if (!hit) _rb.linearVelocity = direction.normalized * _blackboard.origin.speed;
        }  
        else _rb.linearVelocity = _targetPos.normalized * _blackboard.origin.speed;
        
        // ToDo. Y Sorting 관련. 우선적으로 이렇게 처리하지만, 추후 자연스럽게 하기 위해 어떻게 해야할지 연구 필요.
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;

        Facing();
        _prevPos = transform.position;
    }
    
    public void GoToPlayer(EnemyBlackboard blackboard) {  
        if (_blackboard == null) _blackboard = blackboard;
        _isChasing = true;
        if (_nxPosCoroutine != null) StopCoroutine(_nxPosCoroutine);
        _nxPosCoroutine = null;
    }

    public void Patrol(EnemyBlackboard blackboard)
    {  // Init 상태에서 실행할 것.
        if (_blackboard == null) _blackboard = blackboard;
        _isChasing = false;
        if (_nxPosCoroutine == null && !_blackboard.IsDead)
        {
            _nxPosCoroutine = StartCoroutine(ChoiceNextPositionInPatrolCoroutine());    
        }
    }
    
    private IEnumerator ChoiceNextPositionInPatrolCoroutine()
    {
        yield return _waitFor2Sec; // 잠깐 기다렸다 시작.
        while (!_isChasing)
        {
            Vector2 direction = _patrolDirections[Random.Range(0, _patrolDirections.Length)];
            _targetPos = direction;
            _rb.linearVelocity = Vector2.zero;
            if (_collider2D != null)
            {
                _rayPatrol = GetRay(direction);
                RaycastHit2D hit = Physics2D.Raycast(_rayPatrol.origin, _rayPatrol.direction, 0.5f, _layerMask);
                if (!hit) yield return _waitFor2Sec;
                else yield return _waitFor10mSec;
            }
            else yield return null;
        }
    }

    private Ray2D GetRay(Vector2 direction)
    { 
        Vector3 rayOriginPos = transform.position + 
                               new Vector3(direction.x * _collider2D.radius, direction.y * _collider2D.radius);
        return new Ray2D(rayOriginPos, direction);
    }

    private void Facing()
    {
        Vector2 direction;
        if (!_isChasing) direction = _targetPos;
        else direction = (_blackboard.targetPosition - (Vector2)transform.position).normalized;
        _animator.SetFloat("Horizontal", direction.x);
        _animator.SetFloat("Vertical", direction.y);
        _blackboard.facingDirection = direction;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction);
        Gizmos.color = Color.rebeccaPurple;
        Gizmos.DrawLine(_rayPatrol.origin, _rayPatrol.origin + _rayPatrol.direction);
    }
}
