using TMPro;
using UnityEngine;

public class BatteryAnimControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textUI;
    private Animator _animator;
    private BatteryStatus _batteryStatus;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetBatteryStatus(BatteryStatus batteryStatus)
    {
        _batteryStatus = batteryStatus;
    }

    public void PlayAnimation()
    {
        _animator.SetTrigger("GetItem");
    }
    
    // Animation Event
    public void SetText()
    {
        if (!_textUI.enabled)
        {
            Debug.Log("text 안보임");
            return;
        }
        _textUI.GetComponent<TextLoader>().SetTextId(_batteryStatus.textid);
    }
    
    // Animation Event
    public void OnDisableBattery()
    {
        GameObject parent = transform.parent.gameObject;
        PostManager.Instance.Post(PostMessageKey.BatteryDespawned, parent);
    }
}
