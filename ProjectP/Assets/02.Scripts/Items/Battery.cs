using System;
using System.Collections.Generic;
using UnityEngine;
using NewWeaponSystem;
using Random = UnityEngine.Random;

public class Battery : MonoBehaviour
{
    [SerializeField] private List<BatteryStatus> batteryStatuses;
    [SerializeField] private LayerMask _layerMask;
    private BatteryStatus _batteryStatus;

    private void OnEnable()
    {
        SelectStatus();
    }

    private void OnDisable()
    {
        _batteryStatus = null;
    }

    private void OnCollisionEnter2D(Collision2D other)
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
            PostManager.Instance.Post(PostMessageKey.UpgradeWeapon, data);
            PostManager.Instance.Post(PostMessageKey.BatteryDespawned, gameObject);
        }
    }

    private void SelectStatus()
    {
        int length = batteryStatuses.Count;
        int selectedIndex = Random.Range(0, length);
        _batteryStatus = batteryStatuses[selectedIndex];
    }
}