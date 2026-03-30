using UnityEngine;
using UnityEngine.InputSystem;

namespace NewWeaponSystem
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private CommonWeaponData _weaponData;
        [SerializeField] private ProjectilePoolManager _ppm;
        [SerializeField] private Vector2 _projectileStartPos;
        [SerializeField] private GameObject _scopePrefab;
        private InputSystem_Actions _input;
        private WeaponBlackboard _blackboard;
        private Camera _cam;
        private Vector2 _mousePos;

        private void Awake()
        {
            _cam = Camera.main;
            _input = new InputSystem_Actions();
            _ppm = GetComponent<ProjectilePoolManager>();
        }

        private void Start()
        {
            _blackboard = new WeaponBlackboard(_weaponData);
            _ppm.Init(_blackboard);
        }

        private void Update()
        {
            _scopePrefab.transform.position = _mousePos;
        }
        
        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Attack.started += Fire;
            _input.Player.MousePosition.performed += GetMousePosition;
        }

        private void OnDisable()
        {
            _input.Player.Attack.started -= Fire;
            _input.Player.MousePosition.performed -= GetMousePosition;
            _input.Disable();
        }

        private void GetMousePosition(InputAction.CallbackContext context)
        {
            Vector2 screenPos = context.ReadValue<Vector2>();
            _mousePos = _cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
        }
        
        private void Fire(InputAction.CallbackContext context)
        {
            // 격발 이펙트도 있으려나..?
            Vector2 direction = _mousePos - _projectileStartPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            ProjectileSpwanMsg msg = new ProjectileSpwanMsg()
            {
                name = "",
                pos = _projectileStartPos,
                rot = rotation
            };
            PostManager.Instance.Post(PostMessageKey.ProjectileSpawned, msg);
        }
    }
}