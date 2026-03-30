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

   private void Awake()
   {
      _inputActionAsset.Enable();
      _inputActionAsset["Move"].performed += Move;
      _inputActionAsset["Move"].canceled += MoveStop;
      _inputActionAsset["Dash"].started += OnDash;
      
      _rb = GetComponent<Rigidbody2D>();
   }

   private void OnEnable()
   {
      PostManager.Instance.Subscribe<Vector2>(PostMessageKey.InitPlayerPosition, UpdatePosition);
   }

   private void OnDisable()
   {
      PostManager.Instance.Unsubscribe<Vector2>(PostMessageKey.InitPlayerPosition, UpdatePosition);
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
      Debug.Log(input);
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

   private void UpdatePosition(Vector2 position)
   {
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
}