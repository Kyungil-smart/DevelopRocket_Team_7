using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private InputActionReference _inputActionReference;
   [SerializeField] private InputActionAsset _inputActionAsset;

   private Vector2 input;
   [SerializeField] private float moveSpeed = 10f;  // 플레이어 기본 속도
   
   private void Update()
   {
      transform.Translate(input.normalized * moveSpeed* Time.deltaTime);
   }

   private void Awake()
   {
      _inputActionAsset.Enable();
      _inputActionAsset["Move"].performed += Move;
   }


   public void Move(InputAction.CallbackContext context)
   {
      input = context.ReadValue<Vector2>();
      input = new Vector2(input.x, input.y);
      
   }
}
