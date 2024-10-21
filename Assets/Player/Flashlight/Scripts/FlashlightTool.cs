using System;
using UnityEngine;

namespace LTH.Player.Components
{
    public class FlashlightTool : MonoBehaviour
    {
        public event Action<float> ChargeChanged;
        public event Action<int> BatteriesChanged;

        [Header("References")]
        [SerializeField] private Animator _animator;

        [Header("Parameters")]
        [SerializeField] private float _chargeDuration = 60f;

        private FlashlightState _state;

        private float _startTime;
        private float _currentCharge, _remainingCharge;
        private int _batteries;

        private readonly int DischargedTrigger = Animator.StringToHash("Discharged");
        private readonly int TurnOnTrigger = Animator.StringToHash("TurnOn");
        private readonly int TurnOffTrigger = Animator.StringToHash("TurnOff");

        private float Charge
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
            Charge = _chargeDuration;
            _remainingCharge = 0;
        }

        private void Update()
        {
            var t = Time.realtimeSinceStartup - _startTime;
            Charge = Mathf.Clamp(_chargeDuration - t, 0, _chargeDuration);

            if (t < _chargeDuration)
                return;

            if (_batteries > 0)
            {
                Recharge();
                return;
            }

            Discharge();
        }

        public void SetState(FlashlightState state)
        {
            var light = (Light)GetComponent(typeof(Light));
            switch (state)
            {
                case FlashlightState.Off:
                    light.intensity = 0;
                    this.enabled = light.enabled = false;
                    break;
                case FlashlightState.On:
                    light.intensity = 1;
                    this.enabled = light.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public FlashlightState Toggle()
        {
            switch (_state)
            {
                case FlashlightState.Off:
                    TurnOn();
                    break;
                case FlashlightState.On:
                    TurnOff();
                    break;
                default:
                    break;
            }

            return _state;
        }

        private void Discharge()
        {
            this.enabled = false;
            _state = FlashlightState.Off;
            _animator.SetTrigger(DischargedTrigger);
        }

        private void Recharge()
        {
            --Batteries;
            Charge = _chargeDuration;
            _startTime = Time.realtimeSinceStartup;
        }

        public void TurnOn()
        {
            if (_state == FlashlightState.On && _currentCharge <= 0f && Batteries <= 0)
                return;

            this.enabled = true;
            _state = FlashlightState.On;
            _startTime = Time.realtimeSinceStartup - _remainingCharge;
            _animator.SetTrigger(TurnOnTrigger);
        }

        public void TurnOff()
        {
            if (_state == FlashlightState.Off)
                return;

            this.enabled = false;
            _state = FlashlightState.Off;
            _remainingCharge = _chargeDuration - _currentCharge;
            _animator.SetTrigger(TurnOffTrigger);
        }

        public void AddBattery()
        {
            ++Batteries;
        }
    }

    public enum FlashlightState
    {
        Off = 0, On = 1
    }
}