using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private InputActionReference _inputActionReference;
   [SerializeField] private InputActionAsset _inputActionAsset;

   private Vector2 input;
   
   private void Update()
   {
      transform.Translate(input * Time.deltaTime);
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
