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

        private readonly RaycastHit[] _hits = new RaycastHit[8];

        private Vector3 _direction;

        private void OnValidate()
        {
            if (!_origin)
                _origin = transform;
        }

        private void OnEnable()
        {
            _aimRotation.action.performed += GetAimFromRotation;
            _aimPosition.action.performed += GetAimFromPosition;
        }

        private void Awake()
        {
            _flashlightToggle.action.performed += ToggleFlashlight;
            _flashlight.SetState(FlashlightState.Off);
        }

        private void OnDisable()
        {
            _aimRotation.action.performed -= GetAimFromRotation;
            _aimPosition.action.performed -= GetAimFromPosition;
        }

        private void OnDestroy()
        {
            _flashlightToggle.action.performed -= ToggleFlashlight;
        }

        private void GetAimFromRotation(InputAction.CallbackContext ctx)
        {
            var dir = ctx.ReadValue<Vector2>();
            _direction = new Vector3(dir.x, 0, dir.y);
            _origin.LookAt(_origin.position + _direction, _origin.up);
        }

        private void GetAimFromPosition(InputAction.CallbackContext ctx)
        {
            var screenPos = ctx.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(screenPos, Camera.MonoOrStereoscopicEye.Mono);

            if (Physics.RaycastNonAlloc(ray, _hits, 100f, _targetLayers, QueryTriggerInteraction.Ignore) <= 0)
                return;

            _direction = Vector3.ProjectOnPlane(_hits[0].point - _origin.position, _origin.up);
            _origin.LookAt(_origin.position + _direction, _origin.up);
        }

        private void ToggleFlashlight(InputAction.CallbackContext _)
        {
            _flashlight.Toggle();
        }
    }
}