using UnityEngine;
using UnityEngine.InputSystem;

namespace LTH.Gameplay
{
    public class FlashlightController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _aimRotation;
        [SerializeField] private InputActionReference _aimPosition;
        [SerializeField] private InputActionReference _flashlightToggle;

        [Header("References")]
        [SerializeField] private FlashlightToolController _flashlight;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _origin;

        private Vector3 _direction;

        private void InitializeMonoBehaviour()
        {
            if (!_flashlight)
                _flashlight = (FlashlightToolController)GetComponent(typeof(FlashlightToolController));

            if (!_origin)
                _origin = transform;

            if (!_camera)
            {
                Debug.Assert(Camera.main, "No Main Camera found.");
                _camera = Camera.main;
            }
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

            if (Mathf.Approximately(Vector2.SqrMagnitude(dir), 0))
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

            if (Mathf.Approximately(distance, 0))
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