using UnityEngine;
using UnityEngine.InputSystem;

public class TempController : MonoBehaviour
{
    [SerializeField] private GameObject NodeUI;
    private bool _isSelected;
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NodeUI.SetActive(!NodeUI.activeSelf);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PostManager.Instance.Post(PostMessageKey.SelectWeapon, WeaponType.Rifle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PostManager.Instance.Post(PostMessageKey.SelectWeapon, WeaponType.Shotgun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PostManager.Instance.Post(PostMessageKey.SelectWeapon, WeaponType.Sniper);
        }
    }
}
