using UnityEngine;

namespace LTH.Gameplay.InventorySystem
{
    public class EquipmentSlots
    {
        public const int MaxToolSlots = 4;
        private static readonly Tool Empty = new();

        private readonly Tool[] _toolSlots = { Empty, Empty, Empty, Empty };

        public Tool Get(int slot) => _toolSlots[slot];

        public T Get<T>() where T : Tool
        {
            foreach (var slot in _toolSlots)
            {
                if (slot is T t) return t;
            }

            return null;
        }

        public bool IsEmpty(Tool tool) => tool == null || tool == Empty;

        public void Add(Tool tool, int slot)
        {
            Debug.Assert(slot is >= 0 and < MaxToolSlots, $"Selected slot is out of range: {slot}");
            _toolSlots[slot] = tool;
        }

        public void Add(Tool tool)
        {
            for (var i = 0; i < MaxToolSlots; ++i)
            {
                if (_toolSlots[i] == Empty)
                {
                    _toolSlots[i] = tool;
                    return;
                }
            }
        }

        public void Remove(int slot)
        {
            Debug.Assert(slot is >= 0 and < MaxToolSlots, $"Selected slot is out of range: {slot}");
            _toolSlots[slot] = Empty;
        }

        public void Remove(Tool tool)
        {
            for (var i = 0; i < MaxToolSlots; ++i)
            {
                if (_toolSlots[i] == tool)
                {
                    _toolSlots[i] = Empty;
                    return;
                }
            }
        }

        public void Equip(int index)
        {
            _toolSlots[index].Equip();
        }

        public void Unequip(int index)
        {
            _toolSlots[index].Unequip();
        }
    }
}