using UnityEngine;

namespace LTH.Gameplay.InventorySystem
{
    public sealed class FlashlightTool : Tool
    {
        public int BatteryCount { get; private set; }
        public int MaxBatteryCount { get; private set; }
        public float BatteryCharge { get; private set; }
        public float MaxCharge { get; private set; }

        public bool CanTurnOn => BatteryCharge > 0f || BatteryCount > 0;

        public FlashlightTool(ItemData data, int maxBatteries, float maxCharge) : base(data)
        {
            Initialize(data, maxBatteries, maxCharge);
        }

        public void Initialize(ItemData data, int maxBatteries, float maxCharge)
        {
            BatteryCount = 0;
            MaxBatteryCount = maxBatteries;
            BatteryCharge = 0f;
            MaxCharge = maxCharge;
        }

        public void AddBattery()
        {
            BatteryCount = Mathf.Clamp(++BatteryCount, 0, MaxBatteryCount);
        }

        public void ConsumeBattery()
        {
            BatteryCount = Mathf.Clamp(--BatteryCount, 0, MaxBatteryCount);
        }

        public void Recharge()
        {
            BatteryCharge = MaxCharge;
        }

        public void AddBattery(int amount)
        {
            Debug.Assert(amount > 0, "Battery count must be greater than zero");
            BatteryCount = Mathf.Clamp(BatteryCount + amount, 0, MaxBatteryCount);
        }

        public void ConsumeBattery(int amount)
        {
            Debug.Assert(amount > 0, "Battery count must be greater than zero");
            BatteryCount = Mathf.Clamp(BatteryCount - amount, 0, MaxBatteryCount);
        }

        public void Recharge(float delta)
        {
            Debug.Assert(delta > 0f, "Charge delta must be greater than zero");
            BatteryCharge = Mathf.Clamp(BatteryCharge + delta, 0, MaxCharge);
        }

        public void Discharge(float delta)
        {
            Debug.Assert(delta > 0f, "Charge delta must be greater than zero");
            BatteryCharge = Mathf.Clamp(BatteryCharge - delta, 0, MaxCharge);
        }
    }
}