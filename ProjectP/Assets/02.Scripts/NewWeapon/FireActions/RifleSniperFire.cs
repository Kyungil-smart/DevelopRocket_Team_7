using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace NewWeaponSystem
{
    public class RifleSniperFire : FireAbstractClass
    {
        public override void SetUp(WeaponBlackboard blackboard, Transform portTf)
        {
            _blackboard = blackboard;
            _portTf = portTf;
        }

        public override void Fire(InputAction.CallbackContext context)
        {
            if (_fireCoroutine == null) _fireCoroutine = StartCoroutine(FireCoroutine());
        }

        public override void FireStop(InputAction.CallbackContext context)
        {
            if (_fireCoroutine != null) StopCoroutine(_fireCoroutine);
            _fireCoroutine = null;
        }

        protected override IEnumerator FireCoroutine()
        {
            while (true)
            {
                if (_blackboard.CurrentAmmo <= 0)
                {
                    if (!_isReloading) StartCoroutine(Reload());
                }
                else
                {
                    Vector2 direction = _mousePos - (Vector2)transform.position;
                    PostManager.Instance.Post(PostMessageKey.ProjectileSpawned, new ProjectileSpwanMsg()
                    {
                        startPos = _portTf.position,
                        direction = new List<Vector2>() {direction},
                        blackboard = _blackboard
                    });
                    _blackboard.WasteAmmo(1);
                }
                float rate = (1 / _blackboard.attackSpeed);
                yield return new WaitForSecondsRealtime(rate);    
            }
        }

        protected override IEnumerator Reload()
        {
            Debug.Log("Reload");
            _isReloading = true;
            yield return new WaitForSecondsRealtime(_blackboard.reloadTime);
            _blackboard.RefillAmmo();
            _isReloading = false;
        }
    }
}