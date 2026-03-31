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
   [SerializeField] private SpriteRenderer _sp;
   
   [Header("Stat")]
   [SerializeField] private PlayerStat _playerStat;
   [SerializeField] private float dashSpeed = 30f;  // 플레이어 대쉬 속도
   [SerializeField] private float dashTime = 0.5f;  // 플레이어 대쉬 시간
   [SerializeField] private float dashCooldown = 5f; // 대쉬 쿨타임 
   [SerializeField] private float countDownInterval = 0.5f;
   [SerializeField] private Vector2 input;
   [SerializeField] private bool isDashing = false;  // 플레이어 대쉬 중인지 체크

   private Coroutine _dashCountDownCoroutine;
   [SerializeField] private int _dashStack = 2;
   private int _maxDashStack = 2;

   private Vector2 prePos;

   public float raycastDistance = 3f;  // 레이캐스트 감지 거리

   private RaycastHit2D hit;
   private Ray ray;
   
   
   private void Awake()
   {
      _inputActionAsset.Enable();
      _inputActionAsset["Move"].performed += Move;
      _inputActionAsset["Move"].canceled += MoveStop;
      _inputActionAsset["Dash"].started += OnDash;
      _inputActionAsset["Interact"].started += Interact;
      
      _rb = GetComponent<Rigidbody2D>();
      _sp = GetComponent<SpriteRenderer>();
   }

   private void OnEnable()
   {
      PostManager.Instance.Subscribe<Vector2>(PostMessageKey.InitPlayerPosition, UpdatePosition);
   }

   private void OnDisable()
   {
      PostManager.Instance.Unsubscribe<Vector2>(PostMessageKey.InitPlayerPosition, UpdatePosition);
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
      if(isDashing) return; // 대쉬 때 무적 판정
      _playerStat.playerHp -= damage;
      if (_playerStat.playerHp <= 0) Dead();
   }

   public void Move(InputAction.CallbackContext context)
   {
      if(isDashing) return;

      input = context.ReadValue<Vector2>();
      _rb.linearVelocity = input * _playerStat.moveSpeed;
      _animator.SetFloat("Horizontal", input.x);
      _animator.SetFloat("Vertical", input.y);
   }

   public void MoveStop(InputAction.CallbackContext context)
   {
      if(isDashing) return;

      _rb.linearVelocity = Vector2.zero;
   }
   
   public void OnDash(InputAction.CallbackContext context)
   {
      
      if ((isDashing == false) && _dashStack > 0)
      {
         
         Dash(input);

         if(_dashCountDownCoroutine == null)
         {
            _dashCountDownCoroutine = StartCoroutine(DashCountDownRoutine(input));
         }
      }
   }

   private void Dash(Vector2 direction)
   {
      
      isDashing = true;
      GetComponent<Rigidbody2D>().linearVelocity = direction * dashSpeed;
      _dashStack--;
      DashStop();
   }

   private void DashStop()
   {
      isDashing = false;
   }

   private IEnumerator DashCountDownRoutine(Vector2 direction)
   {
      float currentTimeCount = dashCooldown;
      float dashOffCount = dashTime;

      while (_dashStack < _maxDashStack || isDashing) // 대시 스택이 덜 찼거나, 대시중일때만 반복
      {
         yield return new WaitForSeconds(countDownInterval);

         // 대시 스택이 덜 찼다면
         if (_dashStack < _maxDashStack)
         {
            currentTimeCount -= countDownInterval;

            // 시간 카운팅 다 됐으면
            if (currentTimeCount < 0)
            {
               // 대시 카운트 시간 초기화하고 스택 +1;
               currentTimeCount = dashCooldown;
               _dashStack++;
            }
         }
      }
   }

   public void Interact(InputAction.CallbackContext context)   // 플레이어 상호작용
   {
      ray = new Ray(transform.position, transform.forward); // 플레이어가 바라보는 방향으로 레이캐스트
      if (Physics2D.Raycast(ray.origin, ray.direction, raycastDistance)) // 레이캐스트 범위 안에서 물체 확인
      {
         GameObject hitObject = hit.collider.gameObject; // 감지 범위의 물체 정보 전달
         if (hitObject != null)  // 물체가 있을 경우
         {
            Debug.Log(gameObject.name); // 그 물체의 레이어? 오브젝트?에 따라 상호작용처리
         }
         
      }
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

   [ContextMenu("Test/Dead")]
   public void OnTestDead()
   {
      Dead();
   }

   public void GetExp() // 플레이어가 경험치 획득 시 (포스트매니저 통해서 몬스터 구독하여 경험치 습득 코드 추가할 예정)
   {

      if (_playerStat.playerLevel >= _playerStat.playerMaxLevel) return; // 현재 플레이어 레벨이 최대레벨 이상일 경우 리턴
      
      if (_playerStat.playerExp >= _playerStat.NeedLevelUpExp) // 현재 경험치가 레벨업 시 필요한 경험치 이상일 경우    
      {
         _playerStat.playerExp -= _playerStat.NeedLevelUpExp; // 현재 경험치에서 레벨업 시 필요한 경험치 만큼 감소
         LevelUp();
      }
   }


   public void LevelUp()
   {
      _playerStat.playerLevel++;

      // (협업해서 구현 필요)레벨 업 후 노드에 레벨업 정보 전달
      
   }
   
   

}