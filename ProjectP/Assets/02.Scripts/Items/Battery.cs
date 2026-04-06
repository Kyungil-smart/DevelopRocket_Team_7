using System.Collections.Generic;
using UnityEngine;
using NewWeaponSystem;
using Random = UnityEngine.Random;

public class Battery : MonoBehaviour
{
    [SerializeField] private List<BatteryStatus> batteryStatuses;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private AudioClip _getSound;
    [SerializeField] private BatteryAnimControl _animControl;
    private BatteryStatus _batteryStatus;

    private void OnEnable()
    {
        SelectStatus();
    }

    private void OnDisable()
    {
        _batteryStatus = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utils.CompareLayer(other.gameObject.layer, _layerMask))
        {
            if (_batteryStatus == null) return;
            WeaponUpgradeMsg data = new()
            {
                damage = _batteryStatus.damage,
                attackSpeed = _batteryStatus.attackSpeed,
                critRate = _batteryStatus.critRate,
                critMultiplier = _batteryStatus.critMultiplier
            };
            AudioManager.Instance.OnSfxPlayOnShot(_getSound);
            PostManager.Instance.Post(PostMessageKey.UpgradeWeapon, data);
            _animControl.PlayAnimation();
        }
    }

    private void SelectStatus()
    {
        int length = batteryStatuses.Count;
        int selectedIndex = Random.Range(0, length);
        _batteryStatus = batteryStatuses[selectedIndex];
        _animControl.SetBatteryStatus(_batteryStatus);
    }
}