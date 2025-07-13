using UnityEngine;
using UnityEngine.Localization;

namespace LTH.Gameplay.InventorySystem
{
    [CreateAssetMenu(fileName = "NoteData", menuName = "Inventory System/Note Data")]
    public sealed class NoteData : AItemData
    {
        [Space]
        [SerializeField] private LocalizedString _localizedContent;

        public string Content => _localizedContent.GetLocalizedString();
    }
}