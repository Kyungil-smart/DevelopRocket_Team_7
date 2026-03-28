using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour, INeedBossBlackboard
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    private BoxCollider2D _collider2D;
    private BossBlackBoard _blackBoard;
    private Vector2 _targetPos;
    private Vector2 _prePosition;
    private bool _isChaseForce;
    public bool IsChaseForce { get { return _isChaseForce; } }
    
    // Patrol 관련
    [Header("Patrol 을 위한 데이터")]
    [SerializeField] private LayerMask _layerMask;
    private bool _isPatrol;
    private Ray2D _rayPatrol;
    private Coroutine _nxPosCoroutine;
    private WaitForSeconds _waitFor2Sec = new WaitForSeconds(2.0f);
    private WaitForSeconds _waitFor10mSec = new WaitForSeconds(0.01f);
    private Vector2[] _patrolDirections = new Vector2[] { Vector2.right, Vector2.left, Vector2.down, Vector2.up };

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        _isPatrol = false;
        PostManager.Instance.Subscribe<Vector2>(PostMessageKey.PlayerPosition, UpdateMoveDirection);
        _animator.SetBool("IsMoving", true);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<Vector2>(PostMessageKey.PlayerPosition, UpdateMoveDirection);
    }

    private void FixedUpdate()
    {
        if (!_animator.GetBool("IsMoving"))
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        SetBodyDirection();
        Vector3 direction;
        if (_isPatrol)
        {
            Patrol();
            direction = _targetPos;
        }
        else
        {
            ChasePlayer();
            direction = new Vector3(_targetPos.x, _targetPos.y) - transform.position;
        }
        
        // ToDo. Y Sorting 관련. 우선적으로 이렇게 처리하지만, 추후 자연스럽게 하기 위해 어떻게 해야할지 연구 필요.
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        
        if (direction.x >= 0) _spriteRenderer.flipX = false;
        else _spriteRenderer.flipX = true;
        
        _prePosition = transform.position;
    }

    public void OnSFXWalking()
    {
        // ToDo. 걷는 소리 재생하기.
    }
    
    private void ChasePlayer()
    {
        float direction = Vector2.Distance(_targetPos, transform.position);
        if (_blackBoard.origin.attackRange < direction)
            _rb.linearVelocity = _targetPos.normalized * _blackBoard.speed;
    }
    
    private void Patrol()
    {
        _rb.linearVelocity = _targetPos * _blackBoard.speed;
    }

    public void OnChaseForce()
    {   // ToDo. Boss 가 Hit 되면 바로..
        _isChaseForce = true;
    }

    private void UpdateMoveDirection(Vector2 targetPos)
    {
        if (_blackBoard == null) return;
        float distance = Vector2.Distance(targetPos, transform.position);
        if (_blackBoard.origin.detectRadius >= distance || _isChaseForce)
        {
            _targetPos = targetPos;
            _isPatrol = false;
            if (_nxPosCoroutine != null)
            {
                StopCoroutine(_nxPosCoroutine);
                _nxPosCoroutine = null;
            }
        }
        else
        {
            _isPatrol = true;
            if (_nxPosCoroutine == null) 
                _nxPosCoroutine = StartCoroutine(ChoiceNextPositionInPatrolCoroutine());
        }
    }

    public void SetBlackboard(BossBlackBoard blackboard)
    {
        _blackBoard = blackboard;
    }
    
    private IEnumerator ChoiceNextPositionInPatrolCoroutine()
    {
        yield return _waitFor2Sec; // 잠깐 기다렸다 시작.
        while (_isPatrol)
        {
            Vector2 direction = _patrolDirections[Random.Range(0, _patrolDirections.Length)];
            _rb.linearVelocity = Vector2.zero;
            _targetPos = direction;
            if (_collider2D != null)
            {
                _rayPatrol = GetRay(direction);
                RaycastHit2D hit = Physics2D.Raycast(_rayPatrol.origin, _rayPatrol.direction, 3, _layerMask);
                if (!hit) yield return _waitFor2Sec;
                else yield return _waitFor10mSec;
            }
            else yield return null;
        }
    }

    private Ray2D GetRay(Vector2 direction)
    { 
        Vector3 rayOriginPos = transform.position + new Vector3(0.5f, 0.5f);
        return new Ray2D(rayOriginPos, direction);
    }

    private void SetBodyDirection()
    {
        _blackBoard.bodyDirection = (transform.position - new Vector3(_prePosition.x, _prePosition.y)).normalized;
    }

    public void OnStartMoving()
    {
        _animator.SetBool("IsMoving", true);
    }
    //
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(_rayPatrol.origin, _rayPatrol.direction);
    // }
}
