using UnityEngine;
using UnityEngine.Localization;

namespace LTH.DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue Entry", menuName = "DialogueSystem/Dialogue Entry")]
    public class DialogueEntry : ScriptableObject
    {
        [SerializeField] private LocalizedString[] _lines;
        public LocalizedString this[int index] => _lines[index];
        public int Length => _lines.Length;
    }
}