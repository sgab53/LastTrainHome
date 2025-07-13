using System.Collections.Generic;
using LTH.Core.Services;
using UnityEngine;

namespace LTH.Gameplay.InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryService", menuName = "Services/Inventory Service")]
    public class InventoryService : ServiceAsset
    {
        private readonly EquipmentSlots _equipment = new();
        private readonly Dictionary<ItemData, int> _items = new();

        public EquipmentSlots Equipment => _equipment;

        public void AddItem(ItemData item)
        {
            if (item.ItemSettings.HasFlag(ItemSettings.IsUnique) || _items.TryAdd(item, 1))
                return;

            ++_items[item];
        }

        public void RemoveItem(ItemData item)
        {
            if (_items.TryGetValue(item, out var count) && item.ItemSettings.HasFlag(ItemSettings.CanBeDiscarded))
            {
                if (item.ItemSettings.HasFlag(ItemSettings.IsUnique))
                {
                    _items.Remove(item);
                    return;
                }

                if (count > 1)
                    _items[item] = count - 1;
                else
                    _items.Remove(item);
            }
        }

        public Tool GetEquip(int slot) => _equipment.Get(slot);

        protected override void OnInit()
        {
            // Loading logic
        }

        protected override void OnShutdown()
        {
        }
    }
}