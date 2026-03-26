using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
   [SerializeField] private InputActionReference _inputActionReference;
   [SerializeField] private InputActionAsset _inputActionAsset;
   
   
   [SerializeField] private Vector2 input;
   [SerializeField] private float moveSpeed = 10f;  // 플레이어 기본 속도
   
   [SerializeField] private float dashSpeed = 30f;  // 플레이어 대쉬 속도
   [SerializeField] private float dashTime = 0.5f;  // 플레이어 대쉬 시간
   [SerializeField] private float dashCooldown = 0f; // 대쉬 쿨타임 
   
   private bool isDashing = false;  // 플레이어 대쉬 중인지 체크
   
   
   
   
   private void FixedUpdate()
   {
      if (dashCooldown > 0f) dashCooldown -= Time.fixedDeltaTime; // 대쉬 쿨타임 감소
      if (isDashing) return;  // 대쉬중이면 일반 이동 불가
      GetComponent<Rigidbody2D>().linearVelocity = new Vector2(input.x * moveSpeed, input.y * moveSpeed);
   }

   private void Awake()
   {
      base.Awake();
      _inputActionAsset.Enable();
      _inputActionAsset["Move"].performed += Move;
      _inputActionAsset["Dash"].performed += Dash;
   }


   public void Move(InputAction.CallbackContext context)
   {
      input = context.ReadValue<Vector2>();
   }
   
   public void Dash(InputAction.CallbackContext context)
   {
      if (context.started && !isDashing)
      {
         StartCoroutine(DashRoutine());
      }
   }

   private System.Collections.IEnumerator DashRoutine()
   {

      if (dashCooldown <= 0f)    // 대쉬 쿨타임 0초 이하일 때
      {
         isDashing = true;
         dashCooldown = 5f;

         Vector2 dashDirection = input.normalized; // 바라보는 방향으로 대쉬
         if (dashDirection == Vector2.zero) dashDirection = Vector2.right; // 입력한 키 없으면 기본 우측으로 대쉬

         GetComponent<Rigidbody2D>().linearVelocity = dashDirection * dashSpeed;
         yield return new WaitForSeconds(dashTime);


         isDashing = false; // 대쉬 중 종료
         GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
      }

   }
}
