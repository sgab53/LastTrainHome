using System;

namespace LTH.Gameplay.InventorySystem
{
    public class Tool
    {
        public ItemData Data { get; }
        private ToolState _state;

        public Tool() {}

        protected Tool(ItemData data)
        {
            Data = data;
            _state = 0;
        }

        public void Equip()
        {
            _state |= ToolState.Equipped;
        }

        public void Unequip()
        {
            _state &= ~ToolState.Equipped;
        }

        [Flags]
        private enum ToolState
        {
            None = 0,
            Enabled = 1 << 0,
            Active = 1 << 1,
            Equipped = 1 << 2,
        }
    }

    [Flags]
    public enum ItemSettings
    {
        None           = 0,
        IsUnique       = 1 << 0,
        CanBeUsed      = 1 << 1,
        CanBeEquipped  = 1 << 2,
        CanBeDiscarded = 1 << 3,
        HasSingleUse   = 1 << 4,
    }
}