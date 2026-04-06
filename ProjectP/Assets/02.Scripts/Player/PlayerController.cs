using System;
using System.Collections;
using Unity.VisualScripting;
 
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour , IDamage
{
   [Header("Components")]
   [SerializeField] private InputActionReference _inputActionReference;
   [SerializeField] private InputActionAsset _inputActionAsset;
   [SerializeField] private Rigidbody2D _rb;
   [SerializeField] private Animator _animator;
   [SerializeField] private PlayerLvUpAnimController _LvUp;
   [SerializeField] private SpriteRenderer _sp;
   
   [Header("Stat")]
   [SerializeField] private PlayerStat _playerStat;
   [SerializeField] private float dashSpeed = 10f;  // 플레이어 대쉬 속도
   [SerializeField] private float dashTime = 0.15f;  // 플레이어 대쉬 시간
   [SerializeField] private float dashCooldown = 5f; // 대쉬 쿨타임 
   [SerializeField] private float coolDownInterval = 0.5f;
   [SerializeField] private Vector2 input;
   
   [Header("Dash")]
   [SerializeField] private bool isMoving = false;  // 플레이어 이동 중인지 체크
   [SerializeField] private bool isDashing = false;  // 플레이어 대쉬 중인지 체크
   [SerializeField] private int _dashStack = 2;
   private Coroutine _dashCountDownCoroutine; // 대쉬 지속시간 코루틴
   private Coroutine _dashCoolDownCoroutine; // 대쉬 쿨다운 코루틴
   private int _maxDashStack = 2;
   
   [Header("Interaction")]
   [SerializeField] private float raycastDistance = 3f;  // 레이캐스트 감지 거리
   [SerializeField] private LayerMask interactLayer; // 레이캐스트가 감지할 레이어
   private RaycastHit2D hit;
   private Ray2D ray;
   private GameObject _interactedGameObject;
   
   [Header("SoundEffects")]
   // 피격 사운드 클립
   [SerializeField] private AudioClip _takeDamagedSound;
   // 대쉬 사운드 클립
   [SerializeField] private AudioClip _dashSound;
   
   private Vector2 prePos;  // 플레이어 현재 위치 계산용 Cache 값
    [Header("InteractionOBJ")]
    [SerializeField] private GameObject _NodeCanvas;
    [SerializeField] private bool isDead=false;  
    
    private void Awake()
   {  
      _rb = GetComponent<Rigidbody2D>();
      _sp = GetComponent<SpriteRenderer>();
   }

   private void OnEnable()
   {
      _inputActionAsset.Enable();
      _inputActionAsset["Move"].performed += Move;
      _inputActionAsset["Move"].canceled += MoveStop;
      _inputActionAsset["Dash"].started += OnDash;
      _inputActionAsset["Interact"].started += Interact;
      PostManager.Instance.Subscribe<Vector2>(PostMessageKey.InitPlayerPosition, UpdatePosition);
      PostManager.Instance.Subscribe<int>(PostMessageKey.PostExp, GetExp);
   }

   private void OnDisable()
   {
      _inputActionAsset["Move"].performed -= Move;
      _inputActionAsset["Move"].canceled -= MoveStop;
      _inputActionAsset["Dash"].started -= OnDash;
      _inputActionAsset["Interact"].started -= Interact;
      _inputActionAsset.Disable();
      PostManager.Instance.Unsubscribe<Vector2>(PostMessageKey.InitPlayerPosition, UpdatePosition);
      PostManager.Instance.Unsubscribe<int>(PostMessageKey.PostExp, GetExp);
   }

   private void FixedUpdate()
   {  // Player 위치 정보 상시 체크를 위한 PostManager Channel 등록 및 데이터 전송.
      if (Vector2.Distance(_rb.position, prePos) > 0f)
      {
         PostManager.Instance.Post<Vector2>(PostMessageKey.PlayerPosition, transform.position);
      }
      prePos = transform.position;

      _sp.sortingOrder = (int)(transform.position.y * -1);
   }
   
   public void TakeDamage(int damage)
   {
        if (isDead) return;
        if(isDashing) return; // 대쉬 때 무적 판정
        _playerStat.PlayerHp -= damage;
        // 피격 시 사운드 출력
        AudioManager.Instance.OnSfxPlayOnShot(_takeDamagedSound);
        if (_playerStat.PlayerHp <= 0)
        {
            Dead();
            isDead = true;
        }
   }

   public void Move(InputAction.CallbackContext context)
   {
      if(isDashing) return;
      isMoving = true;

      input = context.ReadValue<Vector2>();
      _rb.linearVelocity = input * _playerStat.Sum_moveSpeed;
      _animator.SetBool("Walk", true);
      _animator.SetFloat("Horizontal", input.x);
      _animator.SetFloat("Vertical", input.y);
   }

   public void MoveStop(InputAction.CallbackContext context)
   {
      isMoving = false;
      if(isDashing) return;
      _animator.SetBool("Walk", false);
      _rb.linearVelocity = Vector2.zero;
   }
   
   public void OnDash(InputAction.CallbackContext context)
   {
      if ((isDashing == false) && _dashStack > 0)
      {
         // 대쉬 시전 시 사운드 출력
         AudioManager.Instance.OnSfxPlayOnShot(_dashSound);
         Dash(input);
         
         if(_dashCoolDownCoroutine == null)
         {
            _dashCoolDownCoroutine = StartCoroutine(DashCoolDownRoutine(input));
         }
      }
   }

   private void Dash(Vector2 direction)
   {
      isDashing = true;
      GetComponent<Rigidbody2D>().linearVelocity = direction * dashSpeed;
      _dashCountDownCoroutine = StartCoroutine(DashCountDownRoutine(input));
      PostManager.Instance.Post(PostMessageKey.MainUIDashCount, --_dashStack);
   }

   private void DashStop()
   {
      if(isMoving) _rb.linearVelocity = input * _playerStat.Sum_moveSpeed;
      else _rb.linearVelocity = Vector2.zero;
      
      isDashing = false;
   }

   private IEnumerator DashCountDownRoutine(Vector2 direction)
   {
      yield return new WaitForSeconds(dashTime);
      DashStop();
   }

   private IEnumerator DashCoolDownRoutine(Vector2 direction)
   {
      float currentTimeCount = dashCooldown;

      while (_dashStack < _maxDashStack) // 대시 스택이 덜 찼거나, 대시중이 아닐때만 반복
      {
         yield return new WaitForSeconds(coolDownInterval);
         
         // 대시 스택이 덜 찼다면
         if (_dashStack < _maxDashStack)
         {
            currentTimeCount -= coolDownInterval;

            // 시간 카운팅 다 됐으면
            if (currentTimeCount < 0)
            {
               // 대시 카운트 시간 초기화하고 스택 +1;
               currentTimeCount = dashCooldown;
               PostManager.Instance.Post(PostMessageKey.MainUIDashCount, ++_dashStack);
            }
         }
      }
      _dashCoolDownCoroutine = null;
   }

   public void Interact(InputAction.CallbackContext context)   // 플레이어 상호작용
   {
      if (!context.started) return;
        RayCastingForInteraction();
      // 추후 석상이나 신전에 따라 구별하여 상호작용 처리 코드 추가
      
   }

   private void RayCastingForInteraction()
   {
      //레이캐스트가 시작하는 지점, 레이캐스트 방향, 레이캐스트 감지 거리, 레이캐스트가 감지할 레이어 순으로 인자가 들어감
      ray = new Ray2D(transform.position, input);
      hit = Physics2D.Raycast(ray.origin, ray.direction, raycastDistance, interactLayer);
      if (hit) // 무언가 레이캐스트에 충돌되면
      {
         _interactedGameObject = hit.collider.gameObject;
            Oninteract(_interactedGameObject);
      }
      Debug.Log(hit);
   }

   private void UpdatePosition(Vector2 position)
   {
      // 플레이어 현재 위치
      transform.position = position;
   }
   
   private void Dead()
   {    // 플레이어 사망시 어떻게 동작할 것인지..? 
      _animator.SetTrigger("Dead");
   }

   // Player Dead Animation Event 함수로 등록함
   public void SendSignalPlayerLose()
   {
      // false => 게임 실패
      PostManager.Instance.Post(PostMessageKey.MainUIGameResult, false);
   }

   public void GetExp(int exp) // 플레이어가 경험치 획득 시 (포스트매니저 통해서 몬스터 구독하여 경험치 습득 코드 추가할 예정)
   {
      _playerStat.playerExp += exp;
      if (_playerStat.PlayerLevel >= _playerStat.playerMaxLevel) return; // 현재 플레이어 레벨이 최대레벨 이상일 경우 리턴
      if (_playerStat.playerExp >= _playerStat.NeedLevelUpExp) // 현재 경험치가 레벨업 시 필요한 경험치 이상일 경우    
      {
         _playerStat.playerExp -= _playerStat.NeedLevelUpExp; // 현재 경험치에서 레벨업 시 필요한 경험치 만큼 감소
         LevelUp();
      }
   }

   [ContextMenu("LevelUp")]
   public void LevelUp()
   {
      _LvUp.OnPlayAnimation();
      PostManager.Instance.Post(PostMessageKey.PlayerLevelUp, ++_playerStat.PlayerLevel);
   }
    
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * raycastDistance);
   }
   
   [ContextMenu("Test/Dead")]
   public void OnTestDead()
   {
      Dead();
   }
    private void Oninteract(GameObject obj)
    {
        var index=obj.GetComponentInChildren<IinteractiveObject>()?.Interact();
        if(index ==1)
        {
            _playerStat.FullRecovery();
        }
        else if(index ==2)
        {
            if(_NodeCanvas !=null)
            {
                _NodeCanvas.SetActive(!_NodeCanvas.activeSelf);
            }
        }
    }
}