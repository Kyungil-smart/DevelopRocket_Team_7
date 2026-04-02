using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace NewWeaponSystem
{
    public class ShotgunFire : FireAbstractClass
    {
        [SerializeField] private ShotgonData _shotgonData;
        
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
                if (_blackboard.currentAmmo <= 0)
                {
                    if (!_isReloading) StartCoroutine(Reload());
                }
                else
                {
                    List<Vector2> directions = GetDirections();
                    PostManager.Instance.Post(PostMessageKey.ProjectileSpawned, new ProjectileSpwanMsg()
                    {
                        startPos = _portTf.position,
                        direction = directions,
                        blackboard = _blackboard
                    });
                    _blackboard.currentAmmo -= directions.Count;
                }
                float rate = (1 / _blackboard.attackSpeed);
                yield return new WaitForSecondsRealtime(rate);    
            }
        }

        private List<Vector2> GetDirections()
        {
            List<Vector2> directions = new List<Vector2>();
            Vector2 baseDir = _mousePos - (Vector2)transform.position;
            int pelletCount = Mathf.Max(1, _shotgonData.pelletCount);
            float spread = _shotgonData.spreadAngle;
            float startAngle = -spread * 0.5f;
            for (int i = 0; i < pelletCount; i++)
            {
                float t = pelletCount == 1 ? 0.5f : (float)i / (pelletCount - 1);
                float angle = Mathf.Lerp(startAngle, -startAngle, t);
                directions.Add(Rotate(baseDir, angle));    
            }
            return directions;
        }
        
        private Vector2 Rotate(Vector2 dir, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;

            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);

            float x = dir.x * cos - dir.y * sin;
            float y = dir.x * sin + dir.y * cos;

            return new Vector2(x, y).normalized;
        }

        protected override IEnumerator Reload()
        {
            Debug.Log("Reload");
            _isReloading = true;
            yield return new WaitForSecondsRealtime(_blackboard.reloadTime);
            _blackboard.currentAmmo = _blackboard.origin.magazineSize;
            _isReloading = false;
        }
    }
}