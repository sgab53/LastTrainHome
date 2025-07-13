using System;
using LTH.Gameplay.InventorySystem;
using UnityEngine;

namespace LTH.Gameplay
{
    [RequireComponent(typeof(Light))]
    public class FlashlightToolController : MonoBehaviour
    {
        public event Action<float> ChargeChanged;
        public event Action<int> BatteriesChanged;

        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Light _light;

        [Header("Parameters")]
        [SerializeField] private int _maxBatteryCount = 3;
        [SerializeField] private float _chargeDuration = 60f;

        private FlashlightTool _tool;

        private FlashlightState _state;

        private float _startTime;
        //private float _currentCharge, _remainingCharge;
        private float _remainingCharge;
        private int _batteries;

        private static readonly int DischargedTrigger = Animator.StringToHash("Discharged");
        private static readonly int TurnOnTrigger = Animator.StringToHash("TurnOn");
        private static readonly int TurnOffTrigger = Animator.StringToHash("TurnOff");

        public void SetTool(FlashlightTool tool) => _tool = tool;

        // private float Charge
        // {
        //     get => _currentCharge;
        //     set
        //     {
        //         _currentCharge = value;
        //         ChargeChanged?.Invoke(_currentCharge / _chargeDuration);
        //     }
        // }
        //
        // private int Batteries
        // {
        //     get => _batteries;
        //     set
        //     {
        //         _batteries = value;
        //         BatteriesChanged?.Invoke(value);
        //     }
        // }

        private void OnValidate()
        {
            //Charge = _chargeDuration;
            _remainingCharge = 0;
            _tool?.Initialize(_tool.Data, _maxBatteryCount, _chargeDuration);
        }

        private void Update()
        {
            var t = Time.realtimeSinceStartup - _startTime;
            //Charge = Mathf.Clamp(_chargeDuration - t, 0, _chargeDuration);

            _tool.Discharge(t);

            if (t < _chargeDuration)
            {
                ChargeChanged?.Invoke(_tool.BatteryCharge);
                return;
            }

            if (_batteries > 0)
            {
                Recharge();
                return;
            }

            ChargeChanged?.Invoke(_tool.BatteryCharge);
            Discharge();
        }

        public void SetState(FlashlightState state)
        {
            switch (state)
            {
                case FlashlightState.Off:
                    _light.intensity = 0;
                    this.enabled = _light.enabled = false;
                    break;
                case FlashlightState.On:
                    _light.intensity = 1;
                    this.enabled = _light.enabled = true;
                    break;
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
            _tool.ConsumeBattery();
            _tool.Recharge();
            // --Batteries;
            // Charge = _chargeDuration;
            _startTime = Time.realtimeSinceStartup;

            BatteriesChanged?.Invoke(_tool.BatteryCount);
            ChargeChanged?.Invoke(_tool.BatteryCharge);
        }

        private void TurnOn()
        {
            if (_state == FlashlightState.On || !_tool.CanTurnOn)
                return;

            this.enabled = true;
            _state = FlashlightState.On;
            _startTime = Time.realtimeSinceStartup - _remainingCharge;
            _animator.SetTrigger(TurnOnTrigger);
        }

        private void TurnOff()
        {
            if (_state == FlashlightState.Off)
                return;

            this.enabled = false;
            _state = FlashlightState.Off;
            _remainingCharge = _chargeDuration - _tool.BatteryCharge;
            _animator.SetTrigger(TurnOffTrigger);
        }

        public void AddBattery()
        {
            _tool.AddBattery();
            BatteriesChanged?.Invoke(_tool.BatteryCount);
        }
    }

    public enum FlashlightState
    {
        Off = 0, On = 1
    }
}