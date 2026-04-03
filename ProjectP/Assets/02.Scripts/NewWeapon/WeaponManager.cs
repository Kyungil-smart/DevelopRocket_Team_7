using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewWeaponSystem
{
    public struct WeaponUpgradeMsg
    {
        public float damage;
        public float attackSpeed;
        public float critRate;
        public float critMultiplier;
    }
    
    [Serializable]
    public struct WeaponMgmt
    {
        public WeaponType type;
        public GameObject prefab;
    }

    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private Transform _playerTf;
        [SerializeField] private Transform _initPos;
        [SerializeField] private List<WeaponMgmt> _weapons;
        [SerializeField] private GameObject _scopePrefab;
        private GameObject _selectedWeapon;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            PostManager.Instance.Subscribe<WeaponUpgradeMsg>(PostMessageKey.UpgradeWeapon, UpdateData);
            PostManager.Instance.Subscribe<WeaponType>(PostMessageKey.SelectWeapon, SelectWeapon);
            PostManager.Instance.Subscribe<int>(PostMessageKey.NodeReset, ResetUpgrade);
        }

        private void OnDisable()
        {
            PostManager.Instance.Unsubscribe<WeaponUpgradeMsg>(PostMessageKey.UpgradeWeapon, UpdateData);
            PostManager.Instance.Unsubscribe<WeaponType>(PostMessageKey.SelectWeapon, SelectWeapon);
            PostManager.Instance.Unsubscribe<int>(PostMessageKey.NodeReset, ResetUpgrade);
        }

        public void SelectWeapon(WeaponType wType)
        {
            GameObject prefab = GetWeaponPrefab(wType);
            if (prefab == null)
            {
                Debug.LogError("선택된 타입의 무기가 존재하지 않습니다.");
                return;
            }
            _selectedWeapon = Instantiate(prefab, _playerTf);
            _selectedWeapon.transform.position = _initPos.position;
            WeaponController wc = _selectedWeapon.GetComponent<WeaponController>();
            wc.SetScopePrefab(_scopePrefab);
            GameObject projectile = wc.GetProjectilePrefab();
            PostManager.Instance.Post(PostMessageKey.ProjectileSelection, projectile);
            (int curAmmo, int maxAmmo) = wc.GetAmmo();
            PostManager.Instance.Post(PostMessageKey.MainUICurAmmo, $"{curAmmo} / {maxAmmo}");
        }

        private void ResetUpgrade(int dummy)
        {
            Debug.Log("Receive Request Reset Weapon.");
            _selectedWeapon.GetComponent<WeaponController>().ResetBlackboard();
        }

        private GameObject GetWeaponPrefab(WeaponType wType)
        {
            foreach (var weapon in _weapons)
            {
                if (weapon.type == wType) return weapon.prefab;
            }
            return null;
        }

        private void UpdateData(WeaponUpgradeMsg data)
        {
            WeaponController wc = _selectedWeapon.GetComponent<WeaponController>();
            WeaponBlackboard blackboard = wc.Blackboard;
            if (data.damage != 0)
            {
                Debug.Log("damage Upgrade");
                blackboard.damage = Mathf.CeilToInt(blackboard.damage * (1 + data.damage));
            }
            else if (data.attackSpeed != 0)
            {
                Debug.Log("attackSpeed Upgrade");
                blackboard.attackSpeed += data.attackSpeed;
            }
            else if (data.critRate != 0)
            {
                Debug.Log("critRate Upgrade");
                blackboard.critRate += data.critRate;
            }
            else if (data.critMultiplier != 0)
            {
                Debug.Log("critMultiplier Upgrade");
                blackboard.critMultiplier += data.critMultiplier;
            }
        }

        [ContextMenu("Test/SelectWeapon/Rifle")]
        public void OnTestSelectWeaponRifle()
        {
            SelectWeapon(WeaponType.Rifle);
        }
        
        [ContextMenu("Test/SelectWeapon/Shotgun")]
        public void OnTestSelectWeaponShotgun()
        {
            SelectWeapon(WeaponType.Shotgun);
        }
        
        [ContextMenu("Test/SelectWeapon/Sniper")]
        public void OnTestSelectWeaponSniper()
        {
            SelectWeapon(WeaponType.Sniper);
        }
        
        [ContextMenu("Test/Upgrade")]
        public void OnTestUpgrade()
        {
            WeaponUpgradeMsg msg = new()
            {
                damage = 0.1f,
                attackSpeed = 0,
                critRate = 0,
                critMultiplier = 0,
            };
            UpdateData(msg);
        }
    }
}