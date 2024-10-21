using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LTH.Player.Components
{
    public class FlashlightController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _aimRotation;
        [SerializeField] private InputActionReference _aimPosition;

        [Header("References")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _origin;
        [SerializeField] private Animator _animator;

        [Header("Parameters")]
        [SerializeField] private float _chargeDuration = 60f;

        [Header("Pointer Raycast")]
        [SerializeField] private LayerMask _targetLayers;

        private readonly RaycastHit[] _hits = new RaycastHit[8];

        private Vector3 _direction;

        private float _startTime;
        private float _currentCharge;
        private int _batteries;

        private readonly int DischargedTrigger = Animator.StringToHash("Discharged");
        private readonly int TurnOnTrigger = Animator.StringToHash("TurnOn");
        private readonly int TurnOffTrigger = Animator.StringToHash("TurnOff");

        public event Action<float> ChargeChanged;
        public event Action<int> BatteriesChanged;

        private float CurrentCharge
        {
            get => _currentCharge;
            set
            {
                _currentCharge = value;
                ChargeChanged?.Invoke(_currentCharge / _chargeDuration);
            }
        }

        private int Batteries
        {
            get => _batteries;
            set
            {
                _batteries = value;
                BatteriesChanged?.Invoke(value);
            }
        }

        private void OnValidate()
        {
            if (!_origin)
                _origin = transform;

            CurrentCharge = _chargeDuration;
        }

        private void Awake()
        {
            _startTime = Time.realtimeSinceStartup;
        }

        private void OnEnable()
        {
            _aimRotation.action.performed += GetAimFromRotation;
            _aimPosition.action.performed += GetAimFromPosition;
        }

        private void OnDisable()
        {
            _aimRotation.action.performed -= GetAimFromRotation;
            _aimPosition.action.performed -= GetAimFromPosition;
        }

        private void Update()
        {
            var t = Time.realtimeSinceStartup - _startTime;
            CurrentCharge = Mathf.Clamp(_chargeDuration - t, 0, _chargeDuration);

            if (t < _chargeDuration)
                return;

            if (Batteries > 0)
            {
                Recharge();
                return;
            }

            Discharge();
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

        private void Discharge()
        {
            this.enabled = false;
            _animator.SetTrigger(DischargedTrigger);
        }

        private void Recharge()
        {
            --Batteries;
            CurrentCharge = _chargeDuration;
            _startTime = Time.realtimeSinceStartup;
        }

        public void TurnOn()
        {
            if (!this.enabled && _currentCharge <= 0f && Batteries <= 0)
                return;

            this.enabled = true;
            _animator.SetTrigger(TurnOnTrigger);
        }

        public void TurnOff()
        {
            if (!this.enabled)
                return;

            this.enabled = false;
            _animator.SetTrigger(TurnOffTrigger);
        }

        public void AddBattery()
        {
            ++Batteries;
        }
    }
}