using LTH.Core.Services;
using LTH.UI;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace LTH.DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;

        private DialoguePanel _dialoguePanel;

        public void RegisterNextLineCallback(System.Action callback)
        {
            _dialoguePanel.RegisterCallback<PointerDownEvent>(OnPointerDown);
            return;

            void OnPointerDown(PointerDownEvent _)
            {
                callback!();
            }
        }

        public void ShowDialoguePrompt()
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
            var dialogueService = (DialogueService)ServiceLocator.Instance.GetService(typeof(DialogueService));

            dialogueService.BindUI(this);
        }
    }
}