using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using NewWeaponSystem;

public class StatusUIController : MonoBehaviour
{
    [Header("Value TMP")]
    [SerializeField] private TextMeshProUGUI _weaponValue;
    [SerializeField] private TextMeshProUGUI _attackValue;
    [SerializeField] private TextMeshProUGUI _attackRateValue;
    [SerializeField] private TextMeshProUGUI _criticalRateValue;
    [SerializeField] private TextMeshProUGUI _criticalDmgValue;
    [SerializeField] private TextMeshProUGUI _reloadValue;
    [SerializeField] private TextMeshProUGUI _moveSpeedValue;

    [Header("Connect UI")]
    // 무기 선택 창이 떠 있을 때 Status 창 제어를 하지 못하게 하기 위함.
    [SerializeField] private GameObject _weaponSelectorUI;
    [SerializeField] private GameObject _statusUI;
    
    [SerializeField] private InputActionAsset _inputAsset;
    private InputAction _input;
    private bool _isViewing;

    private void Awake()
    {
        _input = _inputAsset.FindActionMap("UI").FindAction("Status");
        Debug.Log(_input);
    }
    
    private void OnEnable()
    {
        _input.Enable();
        _input.started += OnControlStatusUI;
    }

    private void OnDisable()
    {
        _input.started -= OnControlStatusUI;
        _input.Disable();
    }

    private void OnControlStatusUI(InputAction.CallbackContext context)
    {
        if (_weaponSelectorUI.activeSelf) return;
        
        _isViewing = !_isViewing;
        _statusUI.SetActive(_isViewing);
        float moveSpeed = PostManager.Instance.Request<bool, float>(PostMessageKey.PlayerStatusUIPlayer, true);
        StatusUIMsg statusData = PostManager.Instance.Request<bool, StatusUIMsg>(PostMessageKey.PlayerStatusUIWeapon, true);
        _moveSpeedValue.text = moveSpeed.ToString("0.00");
        _weaponValue.GetComponent<TextLoader>().SetTextId(statusData.textId);
        _attackValue.text = statusData.damage.ToString();
        _attackRateValue.text = statusData.attackSpeed.ToString();
        _criticalRateValue.text = (statusData.critRate * 100).ToString("0.00");
        _criticalDmgValue.text = (statusData.damage * statusData.critMultiplier).ToString("0.00");
        _reloadValue.text = statusData.reloadTime.ToString("0.00");
        _moveSpeedValue.text = moveSpeed.ToString();
        if (_isViewing) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}
