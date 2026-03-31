using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewWeaponSystem
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private CommonWeaponData _weaponData;
        [SerializeField] private Transform _portTf;
        private GameObject _scopePrefab;
        private InputSystem_Actions _input;
        private SpriteRenderer _sp;
        private Animator _animator;
        private WeaponBlackboard _blackboard;
        public WeaponBlackboard Blackboard { get => _blackboard; }
        private Camera _cam;
        private Vector2 _mousePos;
        private int _sortingOffset;
        private Vector2 _prePos;
        
        // 각종 Flags; state 로 하기도 참 애매하고...
        private bool _isRightFacing;
        private bool _isReloading;
        private bool _cannotFire;

        private void Awake()
        {
            _cam = Camera.main;
            _input = new InputSystem_Actions();
            _sp = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _blackboard = new WeaponBlackboard(_weaponData);
        }

        private void Update()
        {
            if (_scopePrefab != null) _scopePrefab.transform.position = _mousePos;
            transform.rotation = GetRotation();
        }
        
        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Attack.started += Fire;
            _input.Player.Move.performed += GetMovePos;
            _input.Player.MousePosition.performed += GetMousePosition;
        }

        private void OnDisable()
        {
            _input.Player.Attack.started -= Fire;
            _input.Player.Move.performed -= GetMovePos;
            _input.Player.MousePosition.performed -= GetMousePosition;
            _input.Disable();
        }

        public void SetScopePrefab(GameObject prefab)
        {
            _scopePrefab = prefab;
        }

        private void GetMousePosition(InputAction.CallbackContext context)
        {
            Vector2 screenPos = context.ReadValue<Vector2>();
            _mousePos = _cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
        }

        private Quaternion GetRotation()
        {
            Vector2 direction = _mousePos - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void GetMovePos(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _animator.SetFloat("Horizontal", input.x);
            _animator.SetFloat("Vertical", input.y);
            _sp.sortingOrder = Mathf.RoundToInt(transform.position.y * -1) + _sortingOffset;
            Facing();
        }

        private void Facing()
        {
            Vector2 direction = (Vector2)transform.position - _prePos;
            if (direction.x > 0) _isRightFacing = false;
            else _isRightFacing = true;

            if (direction.y < 0) _sortingOffset = 2;
            else _sortingOffset = -2;
            
            _prePos = transform.position;
        }
        
        private void Fire(InputAction.CallbackContext context)
        {
            Debug.Log($"{_blackboard.damage} damage");
            if (_cannotFire) return;
            if (_blackboard.currentAmmo <= 0)
            {
                if (!_isReloading) StartCoroutine(Reload());
                _isReloading = true;
                return;
            }
            Vector2 direction = _mousePos - (Vector2)transform.position;
            PostManager.Instance.Post(PostMessageKey.ProjectileSpawned, new ProjectileSpwanMsg()
            {
                startPos = _portTf.position,
                direction = direction,
                blackboard = _blackboard
            });
            _blackboard.currentAmmo--;
            _cannotFire = true;
            StartCoroutine(AttackRate());
        }

        private IEnumerator Reload()
        {
            Debug.Log("Reload");
            yield return new WaitForSecondsRealtime(_blackboard.reloadTime);
            _blackboard.currentAmmo = _blackboard.origin.magazineSize;
            _isReloading = false;
        }

        private IEnumerator AttackRate()
        {
            float rate = (1 / _blackboard.attackSpeed);
            yield return new WaitForSecondsRealtime(rate);
            _cannotFire = false;
        }
    }
}