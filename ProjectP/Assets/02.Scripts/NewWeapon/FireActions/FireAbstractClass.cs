using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewWeaponSystem
{
    public abstract class FireAbstractClass : MonoBehaviour
    {
        protected Coroutine _fireCoroutine;
        protected bool _isReloading;
        protected WeaponBlackboard _blackboard;
        protected Transform _portTf;
        protected Vector2 _mousePos;

        public abstract void SetUp(WeaponBlackboard blackboard, Transform portTf);
        public abstract void Fire(InputAction.CallbackContext context);
        public abstract void FireStop(InputAction.CallbackContext context);
        protected abstract IEnumerator FireCoroutine();
        protected abstract IEnumerator Reload();
        
        public void UpdateMousePos(Vector2 mousePos)
        {
            _mousePos = mousePos;
        }
    }
}