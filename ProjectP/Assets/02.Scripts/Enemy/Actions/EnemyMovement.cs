using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private EnemyFindTarget _findTargetScript;
    private CircleCollider2D _collider2D;
    private Vector3 _nextPosition;
    private bool _isChasing;
    private float _speed;
    private Coroutine _nxPosCoroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2.0f);
    private Vector2[] _patrolDirections = new Vector2[4] { Vector2.right, Vector2.left, Vector2.down, Vector2.up };
    private Ray2D _ray;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        if (_findTargetScript == null) _findTargetScript = new EnemyFindTarget();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _nextPosition, step);
    }
    
    public void Move(Vector2 nextPosition, float speed)
    {  // ToDo. 연산된 방향으로만 움직이도록 할 것.
        _isChasing = true;
        if (_nxPosCoroutine != null) StopCoroutine(_nxPosCoroutine);
        _nextPosition = nextPosition;
        _speed = speed;
    }

    public void Patrol(float speed)
    {  // Init 상태에서 실행할 것.
        _speed = speed;
        _isChasing = false;
        if (_nxPosCoroutine != null) StopCoroutine(_nxPosCoroutine);
        _nxPosCoroutine = StartCoroutine(ChoiceNextPositionCoroutine());
    }
    
    private IEnumerator ChoiceNextPositionCoroutine()
    {
        while (!_isChasing)
        {
            Vector2 direction = _patrolDirections[Random.Range(0, _patrolDirections.Length)];
            _nextPosition = transform.position + new Vector3(direction.x, direction.y);
            Vector3 rayOriginPos = transform.position + 
                                   new Vector3(direction.x * _collider2D.radius, direction.y * _collider2D.radius);
            
            _ray = new Ray2D(rayOriginPos, direction);
            RaycastHit2D hits = Physics2D.Raycast(_ray.origin, _ray.direction, 1, LayerMask.GetMask("Wall"));
            if (!hits) yield return _waitForSeconds;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction);
    }
}
