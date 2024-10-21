using UnityEngine;
using UnityEngine.UIElements;

namespace LTH.UI
{
    public class FlashlightInfo : VisualElement
    {
        private readonly Label _batteriesCount;
        private readonly VisualElement _chargeBar;

        public FlashlightInfo()
        {
            var asset = Resources.Load<VisualTreeAsset>(nameof(FlashlightInfo));
            asset.CloneTree(this);

            _batteriesCount = this.Q<Label>("BatteriesCount");
            _chargeBar = this.Q<VisualElement>("ChargeBar");
        }

        public void SetBatteriesCount(int count)
        {
            _batteriesCount.text = count.ToString("00");
        }

        public void SetChargeBarPercentage(float value)
        {
            _chargeBar.style.width = Length.Percent(value * 100f);
        }

        public new class UxmlFactory : UxmlFactory<FlashlightInfo, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
    }
}