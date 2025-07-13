using UnityEngine;
using UnityEngine.Localization;

namespace LTH.Gameplay.InventorySystem
{
    public abstract class AItemData : ScriptableObject
    {
        [SerializeField] private ItemSettings _itemSettings =
            ItemSettings.CanBeUsed | ItemSettings.CanBeDiscarded;

        [Space]
        [SerializeField] private LocalizedString _localizedName;
        [SerializeField] private LocalizedString _localizedDescription;
        [SerializeField] private LocalizedSprite _localizedIcon;

        public ItemSettings ItemSettings => _itemSettings;
        public string Name => _localizedName.GetLocalizedString();
        public string Description => _localizedDescription.GetLocalizedString();
        public Sprite Icon => _localizedIcon.LoadAsset();
    }
}