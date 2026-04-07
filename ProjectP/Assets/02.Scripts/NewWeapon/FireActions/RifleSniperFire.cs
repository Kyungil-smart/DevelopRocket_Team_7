using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace NewWeaponSystem
{
    public class RifleSniperFire : FireAbstractClass
    {
        [SerializeField] private AudioClip _shotSoundClip;
        [SerializeField] private AudioClip _reloadSoundClip;
        private bool _isFire;
        private Coroutine _isFireCoroutine;

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
                    if (!_isFire)
                    {
                        float fireRate = (1 / _blackboard.attackSpeed);
                        if (_isFireCoroutine == null) _isFireCoroutine = StartCoroutine(FireRateRoutine(fireRate));
                        Vector2 direction = _mousePos - (Vector2)transform.position;
                        AudioManager.Instance.OnSfxPlayOnShot(_shotSoundClip);
                        PostManager.Instance.Post(PostMessageKey.ProjectileSpawned, new ProjectileSpwanMsg()
                        {
                            startPos = _portTf.position,
                            direction = new List<Vector2>() { direction },
                            blackboard = _blackboard
                        });
                        _blackboard.WasteAmmo(1);    
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator FireRateRoutine(float fireRate)
        {
            _isFire = true;
            yield return new WaitForSecondsRealtime(fireRate);
            _isFire = false;
            _isFireCoroutine = null;
        }

    protected override IEnumerator Reload()
        {
            AudioManager.Instance.OnSfxPlayOnShot(_reloadSoundClip);
            _isReloading = true;
            yield return new WaitForSecondsRealtime(_blackboard.reloadTime);
            _blackboard.RefillAmmo();
            _isReloading = false;
        }
    }
}