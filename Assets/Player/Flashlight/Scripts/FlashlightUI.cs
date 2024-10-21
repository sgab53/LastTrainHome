using LTH.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace LTH.Player.Components
{
    public class FlashlightUI : MonoBehaviour
    {
        [SerializeField] private FlashlightController _controller;

        [Header("UI Elements")]
        [SerializeField] private UIDocument _document;

        private FlashlightInfo _flashlightInfo;

        private void Awake()
        {
            _flashlightInfo = _document.rootVisualElement.Q<FlashlightInfo>("FlashlightInfo");
        }

        private void OnEnable()
        {
            _controller.ChargeChanged += OnChargeChanged;
            _controller.BatteriesChanged += OnBatteriesChanged;
        }

        private void OnDisable()
        {
            _controller.ChargeChanged -= OnChargeChanged;
            _controller.BatteriesChanged -= OnBatteriesChanged;
        }

        private void OnChargeChanged(float percentage)
        {
            _flashlightInfo.SetChargeBarPercentage(percentage);
        }

        private void OnBatteriesChanged(int amount)
        {
            _flashlightInfo.SetBatteriesCount(amount);
        }
    }
}