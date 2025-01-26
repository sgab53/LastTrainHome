using LTH.UI;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;
using Action = System.Action;

namespace LTH.DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;

        private DialoguePanel _dialoguePanel;

        public void RegisterNextLineCallback(Action onNextLine)
        {
            _dialoguePanel.RegisterCallback<PointerDownEvent>(OnPointerDown);
            return;

            void OnPointerDown(PointerDownEvent _)
            {
                onNextLine!();
            }
        }

        public void ShowBeforeDialoguePrompt()
        {
            _dialoguePanel.Show();
        }

        public void HideDialoguePrompt()
        {
            _dialoguePanel.Hide();
        }

        public void UpdateDialogueEntry(TableReference table, TableEntryReference entry)
        {
            _dialoguePanel.UpdateDialogue(table, entry);
        }

        private void Start()
        {
            _dialoguePanel = _document.rootVisualElement.Q<DialoguePanel>();
            DialogueService.BindUI(this);
        }
    }
}