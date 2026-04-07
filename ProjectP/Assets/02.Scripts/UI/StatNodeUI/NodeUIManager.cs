using UnityEngine;
using UnityEngine.InputSystem;

public class CloseNodeUI : MonoBehaviour
{
    [SerializeField] private GameObject _nodeUI;
    
    [SerializeField] private InputActionAsset _inputAsset;
    private InputAction _input;

    private bool _isActive;

    private void Awake()
    {
        _isActive = false;
        _input = _inputAsset.FindActionMap("UI").FindAction("NodeUIOnOff");
    }
    private void OnEnable()
    {
        _input.Enable();
        _input.performed += NodeUIOnOff;
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
        _input.performed -= NodeUIOnOff;
        _input.Disable();
    }

    private void NodeUIOnOff(InputAction.CallbackContext context)
    {
        Debug.Log("NodeUIOnOff");
        if (context.control.name == "n")
        {
            _isActive = !_isActive;
            _nodeUI.SetActive(_isActive);
        }
    }
    
    public void OnClickCloseUI()
    {
        _isActive = false;
        _nodeUI.SetActive(false);
    }
}
