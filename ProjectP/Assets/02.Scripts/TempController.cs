using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempController : MonoBehaviour
{
    [SerializeField] private GameObject NodeUI;
    private bool _isSelected;
    private InputSystem_Actions _actions;

    private void Awake()
    {
        _actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _actions.Enable();
        _actions.Player.TempNodeUI.started += ChangeNodeUI;
        _actions.Player.TempRifle.started += SelectRifle;
        _actions.Player.TempShotgun.started += SelectShotgun;
        _actions.Player.TempSniper.started += SelectSniper;
    }

    private void OnDisable()
    {
        _actions.Player.TempNodeUI.started -= ChangeNodeUI;
        _actions.Player.TempRifle.started -= SelectRifle;
        _actions.Player.TempShotgun.started -= SelectShotgun;
        _actions.Player.TempSniper.started -= SelectSniper;
        _actions.Disable();
    }

    private void ChangeNodeUI(InputAction.CallbackContext context)
    {
        NodeUI.SetActive(!NodeUI.activeSelf); 
    }
    
    private void SelectRifle(InputAction.CallbackContext context)
    {
        if (!_isSelected) PostManager.Instance.Post(PostMessageKey.SelectWeapon, WeaponType.Rifle);
        _isSelected = true;
    }
    
    private void SelectShotgun(InputAction.CallbackContext context)
    {
        if (!_isSelected) PostManager.Instance.Post(PostMessageKey.SelectWeapon, WeaponType.Shotgun);
        _isSelected = true;
    }
    
    private void SelectSniper(InputAction.CallbackContext context)
    {
        if (!_isSelected) PostManager.Instance.Post(PostMessageKey.SelectWeapon, WeaponType.Sniper);
        _isSelected = true;
    }
}
