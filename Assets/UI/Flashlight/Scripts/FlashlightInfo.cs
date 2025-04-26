using UnityEngine.UIElements;

namespace LTH.UI
{
    [UxmlElement]
    public sealed partial class FlashlightInfo : VisualElement
    {
        private readonly Label _batteriesCount;
        private readonly VisualElement _chargeBar;

        public FlashlightInfo()
        {
            this.LoadVisualTreeAsset();

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
    }
}