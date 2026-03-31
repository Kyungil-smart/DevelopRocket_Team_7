using System;
using UnityEngine;

public struct WeaponUpgradeMsg
{
    public float damage;
    public float attackSpeed;
    public float critRate;
    public float critMultiplier;
}

public class WeaponUpgradeManager : MonoBehaviour
{
    WeaponBlackboard _blackboard;
    
    private void OnEnable()
    {
        PostManager.Instance.Subscribe<WeaponUpgradeMsg>(PostMessageKey.UpgradeWeapon, UpdateData);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<WeaponUpgradeMsg>(PostMessageKey.UpgradeWeapon, UpdateData);
    }

    private void SetBlackboard(WeaponBlackboard blackboard)
    {
        _blackboard = blackboard;
    }

    private void UpdateData(WeaponUpgradeMsg data)
    {
        if (data.damage != 0) _blackboard.damage = Mathf.CeilToInt(_blackboard.damage * (1 + data.damage));
        else if (data.attackSpeed != 0) _blackboard.attackSpeed += data.attackSpeed;
        else if (data.critRate != 0) _blackboard.critRate += data.critRate;
        else if (data.critMultiplier != 0) _blackboard.critMultiplier += data.critMultiplier;
    }
}