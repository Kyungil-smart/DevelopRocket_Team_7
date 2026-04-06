using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using NewWeaponSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject _settingUI;
    
    [SerializeField] private InputActionAsset _inputAsset;
    private InputAction _inputStatus;
    private InputAction _inputSetting;
    private bool _isStatusViewing;
    private bool _isSettingViewing;

    private void Awake()
    {
        _inputStatus = _inputAsset.FindActionMap("UI").FindAction("Status");
        _inputSetting = _inputAsset.FindActionMap("UI").FindAction("Setting");
        Debug.Log(_inputStatus);
    }
    
    private void OnEnable()
    {
        _inputStatus.Enable();
        _inputStatus.started += OnControlStatusUI;
        _inputSetting.Enable();
        _inputSetting.started += OnControlSettingUI;
    }

    private void OnDisable()
    {
        _inputStatus.started -= OnControlStatusUI;
        _inputStatus.Disable();
        _inputSetting.started -= OnControlSettingUI;
        _inputSetting.Disable();
    }

    private void OnControlStatusUI(InputAction.CallbackContext context)
    {
        if (_weaponSelectorUI.activeSelf) return;
        
        _isStatusViewing = !_isStatusViewing;
        _statusUI.SetActive(_isStatusViewing);
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
        if (_isStatusViewing) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
    
    private void OnControlSettingUI(InputAction.CallbackContext context)
    {
        _isSettingViewing = !_isSettingViewing;
        _settingUI.SetActive(_isSettingViewing);
        if (_isSettingViewing) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}
