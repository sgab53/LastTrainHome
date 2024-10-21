using LTH.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace LTH.Player.Components
{
    public class FlashlightUI : MonoBehaviour
    {
        [SerializeField] private FlashlightTool _flashlight;

        [Header("UI Elements")]
        [SerializeField] private UIDocument _document;

        private FlashlightInfo _flashlightInfo;

        private void Awake()
        {
            _flashlightInfo = _document.rootVisualElement.Q<FlashlightInfo>("FlashlightInfo");
        }

        private void OnEnable()
        {
            _flashlight.ChargeChanged += OnChargeChanged;
            _flashlight.BatteriesChanged += OnBatteriesChanged;
        }

        private void OnDisable()
        {
            _flashlight.ChargeChanged -= OnChargeChanged;
            _flashlight.BatteriesChanged -= OnBatteriesChanged;
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