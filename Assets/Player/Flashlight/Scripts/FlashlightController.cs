using UnityEngine;
using UnityEngine.InputSystem;

namespace LTH.Player.Components
{
    public class FlashlightController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _aimRotation;
        [SerializeField] private InputActionReference _aimPosition;
        [SerializeField] private InputActionReference _flashlightToggle;

        [Header("References")]
        [SerializeField] private FlashlightTool _flashlight;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _origin;

        [Header("Settings")]
        [SerializeField] private LayerMask _targetLayers;

        private Vector3 _direction;

        private void OnValidate()
        {
            if (!_origin)
                _origin = transform;
        }

        private void OnEnable()
        {
            _aimRotation.action.performed += OnAimRotation;
            _aimPosition.action.performed += OnAimPosition;
        }

        private void Awake()
        {
            _flashlightToggle.action.performed += ToggleFlashlight;
            _flashlight.SetState(FlashlightState.Off);
        }

        private void OnDisable()
        {
            _aimRotation.action.performed -= OnAimRotation;
            _aimPosition.action.performed -= OnAimPosition;
        }

        private void OnDestroy()
        {
            _flashlightToggle.action.performed -= ToggleFlashlight;
        }

        private void OnAimRotation(InputAction.CallbackContext ctx)
        {
            var dir = ctx.ReadValue<Vector2>();

            if (Vector2.SqrMagnitude(dir) < Mathf.Epsilon)
                return;

            _direction = new Vector3(dir.x, 0, dir.y);
            _origin.LookAt(_origin.position + _direction, _origin.up);
        }

        private void OnAimPosition(InputAction.CallbackContext ctx)
        {
            var screenPos = ctx.ReadValue<Vector2>();
            Vector2 target = _camera.WorldToScreenPoint(_origin.position);

            var direction = target - screenPos;
            var distance = Vector2.SqrMagnitude(direction);

            if (distance < Mathf.Epsilon)
                return;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

            _origin.rotation = Quaternion.AngleAxis(-angle, _origin.up);
        }

        private void ToggleFlashlight(InputAction.CallbackContext _)
        {
            _flashlight.Toggle();
        }
    }
}