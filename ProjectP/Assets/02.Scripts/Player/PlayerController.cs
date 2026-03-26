using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private InputActionReference _inputActionReference;
   [SerializeField] private InputActionAsset _inputActionAsset;

   [SerializeField] private Vector2 input;
   [SerializeField] private float moveSpeed = 10f;  // 플레이어 기본 속도
   
   [SerializeField] private float dashSpeed = 30f;  // 플레이어 대쉬 속도
   
   private void FixedUpdate()
   {
      //transform.Translate(input.normalized * moveSpeed* Time.deltaTime);
      GetComponent<Rigidbody2D>().linearVelocity = new Vector2(input.x * moveSpeed, input.y * moveSpeed);
   }

   private void Awake()
   {
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
      input = context.ReadValue<Vector2>();
      moveSpeed = dashSpeed;
   }
}
