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
        [SerializeField] private DialogueService _dialogueService;

        private DialoguePanel _dialoguePanel;

        private void ShowDialoguePrompt()
        {
            _dialoguePanel.Show();
        }

        private void HideDialoguePrompt()
        {
            _dialoguePanel.Hide();
        }

        private void UpdateDialogueEntry(TableReference table, TableEntryReference entry)
        {
            _dialoguePanel.UpdateDialogue(table, entry);
        }

        private void Awake()
        {
            _dialoguePanel = _document.rootVisualElement.Q<DialoguePanel>();

            if (!_dialogueService)
                _dialogueService = Service.Load<DialogueService>();
        }

        private void OnEnable()
        {
            _dialogueService.DialogueStarted += ShowDialoguePrompt;
            _dialogueService.DialogueEnded += HideDialoguePrompt;
            _dialogueService.NextLineChanged += UpdateDialogueEntry;

            _dialoguePanel.RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnDisable()
        {
            _dialogueService.DialogueStarted -= ShowDialoguePrompt;
            _dialogueService.DialogueEnded -= HideDialoguePrompt;
            _dialogueService.NextLineChanged -= UpdateDialogueEntry;

            _dialoguePanel.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnPointerDown(PointerDownEvent _)
        {
            _dialogueService.NextLine();
        }
    }
}