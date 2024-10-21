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

        [Header("Pointer Raycast")]
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
            _flashlightToggle.action.performed += _flashlight.Toggle;
        }

        private void OnDisable()
        {
            _aimRotation.action.performed -= GetAimFromRotation;
            _aimPosition.action.performed -= GetAimFromPosition;
            _flashlightToggle.action.performed -= _flashlight.Toggle;
        }

        private void GetAimFromRotation(InputAction.CallbackContext ctx)
        {
            var dir = ctx.ReadValue<Vector2>();
            _direction = new Vector3(dir.x, 0, dir.y);
            _origin.LookAt(_origin.position + _direction, _origin.up);
        }

        private void GetAimFromPosition(InputAction.CallbackContext ctx)
        {
            Vector3 screenPos = ctx.ReadValue<Vector2>();
            screenPos.z = _camera.farClipPlane;

            var start = _camera.transform.position;
            var dir = Vector3.Normalize(_camera.ScreenToWorldPoint(screenPos, Camera.MonoOrStereoscopicEye.Mono) - start);

            var count = Physics.RaycastNonAlloc(start, dir, _hits, 100f, _targetLayers, QueryTriggerInteraction.Ignore);

            if (count <= 0)
                return;

            _direction = _hits[0].point - _origin.position;
            _direction.y = 0f;
            _direction.Normalize();
            _origin.LookAt(_origin.position + _direction, _origin.up);
        }
    }
}