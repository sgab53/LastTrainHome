using UnityEngine;
using UnityEngine.InputSystem;

namespace LTH.Input
{
    public class TopDownMovement : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private InputActionReference _move;
        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _cameraTransform;

        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private LayerMask _groundLayers;

        private Vector2 _movement = Vector2.zero;
        private Vector3 _direction = Vector3.zero;
        private readonly RaycastHit[] _hits = new RaycastHit[8];

        private void OnEnable()
        {
            _move.action.performed += OnMoveActionPerformed;
            _move.action.canceled += OnMoveActionCanceled;
        }

        private void OnDisable()
        {
            _move.action.performed -= OnMoveActionPerformed;
            _move.action.canceled -= OnMoveActionCanceled;
        }

        private void Update()
        {
            var hitCount = Physics.SphereCastNonAlloc(transform.position + (transform.up * _controller.height),
                _controller.radius, -transform.up, _hits, _controller.height * 2f, _groundLayers);

            var normal = Vector3.zero;

            if (hitCount > 0 && _controller.isGrounded)
            {
                for (var i = 0; i < hitCount; ++i)
                {
                    normal += _hits[i].normal;
                }
                normal.Normalize();
            }
            else
            {
                normal = transform.up;
            }

            var forward = Vector3.Normalize(Vector3.Cross(_cameraTransform.right, normal));
            var right = Vector3.Normalize(Vector3.Cross(normal, _cameraTransform.forward));

            _direction = forward * _movement.y + right * _movement.x;

            if (_movement.sqrMagnitude > 1f)
                _direction.Normalize();

            if (_controller.isGrounded)
                _controller.Move(_direction * (_moveSpeed * Time.deltaTime));
            else
                _controller.Move(Vector3.down * (_moveSpeed * Time.deltaTime));
        }

        private void OnMoveActionPerformed(InputAction.CallbackContext ctx)
        {
            _movement = ctx.ReadValue<Vector2>();
        }

        private void OnMoveActionCanceled(InputAction.CallbackContext ctx)
        {
            _movement = Vector2.zero;
        }
    }
}